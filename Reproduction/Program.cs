using Microsoft.EntityFrameworkCore;

using var db = new BloggingContext();

Console.WriteLine("This sample uses an exising database in a fixed location. " +
                  "Please update the path in the BloggingContext ctor in Model.cs to the correct location.");
Console.WriteLine($"Database path: {db.DbPath}.");

var posts = await db.Posts.Include(p => p.Blog).ToListAsync();

var deletedPosts = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();

Console.WriteLine($"{posts.Count} posts found. {deletedPosts.Count} will be deleted");
