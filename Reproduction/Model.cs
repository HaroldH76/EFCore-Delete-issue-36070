using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        DbPath = @"C:\GitHub\EFCore-Delete-issue-36070\Reproduction\Reproduction\blogging.db";
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostMap());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}

internal class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder
            .ToTable("Blogs");

        builder
            .ToTable("Posts");

        builder
            .HasOne(entity => entity.Blog)
            // .WithMany()                          // Correct configuration
            // .HasForeignKey(x => x.BlogId);       // Correct configuration
            .WithOne()                              // Misconfiguration
            .HasForeignKey<Post>(x => x.BlogId);    // Misconfiguration
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    //public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}