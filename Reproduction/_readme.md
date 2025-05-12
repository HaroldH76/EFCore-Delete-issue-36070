We are using EF Core with an existing database. We configured the data model in code.
We accidentally had an incorrect relation configuration. We configured a one to one relation:
```cs
        builder
            .HasOne(entity => entity.Blog)
            .WithOne()                              // Misconfiguration
            .HasForeignKey<Post>(x => x.BlogId);    // Misconfiguration
```

This should have been a one to many:
```cs
        builder
            .HasOne(entity => entity.Blog)
            .WithMany()                          // Correct configuration
            .HasForeignKey(x => x.BlogId);       // Correct configuration
```

Because of the misconfiguration during a `SELECT` query some of the entities are already marked for deletion.
When later `SaveChanges` is called these entities are deleted.

We would have expected in this scenario that EF Core would throw an error about incorrect database configuration instead of silently marking entities for deletion.

To reproduce: 
- Verify/update path to the existing `blogging.db` in ctor of `BloggingContext` in `Model.cs`
- Run the program. 
- It will query for 2 posts and after that it will log to the console that it retrieved 2 posts and one of them is marked for deletion.
