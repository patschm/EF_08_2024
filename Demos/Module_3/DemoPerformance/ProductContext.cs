using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DemoPerformance;

public class ProductContext : DbContext
{
    // For Compiled models
    public ProductContext(): base()
    {
    }
    public ProductContext(DbContextOptions options) : base(options)
    {
    }
    

    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<Specification> Specifications => Set<Specification>();
    public DbSet<SpecificationDefinition> SpecificationDefinitions => Set<SpecificationDefinition>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Reviewer> Reviewers => Set<Reviewer>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Program.connectionString);
       // optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brands", "Core");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Website).HasMaxLength(1024);
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.ToTable("Prices", "Core");

            entity.Property(e => e.ShopName).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Product).WithMany(p => p.Prices).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products", "Core");

            entity.Property(e => e.Image).HasMaxLength(1024);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Brand).WithMany(p => p.Products).HasForeignKey(d => d.BrandId);

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.Products).HasForeignKey(d => d.ProductGroupId);
        });

        modelBuilder.Entity<ProductGroup>(entity =>
        {
            entity.ToTable("ProductGroups", "Core");

            entity.Property(e => e.Image).HasMaxLength(1024);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews", "Core");

           entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews).HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.Reviewer).WithMany(p => p.Reviews).HasForeignKey(d => d.ReviewerId);
        });

        modelBuilder.Entity<Reviewer>(entity =>
        {
            entity.ToTable("Reviewers", "Core");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Specification>(entity =>
        {
            entity.ToTable("Specifications", "Core");

            entity.Property(e => e.Key).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Product).WithMany(p => p.Specifications).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<SpecificationDefinition>(entity =>
        {
            entity.ToTable("SpecificationDefinitions", "Core");

            entity.Property(e => e.Key).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Unit).HasMaxLength(127);

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.SpecificationDefinitions).HasForeignKey(d => d.ProductGroupId);
        });
    }
}
