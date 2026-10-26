using Microsoft.EntityFrameworkCore;
using ProductLab.Models;

namespace ProductLab.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
      
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            // Explicitly map to public schema in Supabase
            entity.ToTable("products", "public");

            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.ProductId)
                .HasColumnName("product_id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            // Precision for 'numeric' type matches database
            entity.Property(p => p.Price)
                .HasColumnName("price")
                .HasColumnType("numeric")
                .HasPrecision(10, 2);

            entity.Property(p => p.StockQuantity)
                .HasColumnName("stock_quantity")
                .HasDefaultValue(0);

            entity.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone") // Supabase timestamptz mapping
                .HasDefaultValueSql("now()");
        });
    }
}
