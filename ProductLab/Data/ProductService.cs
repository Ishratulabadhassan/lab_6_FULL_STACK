using Microsoft.EntityFrameworkCore;
using ProductLab.Models;

namespace ProductLab.Data;

public class ProductService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ProductService(IDbContextFactory<AppDbContext> factory)
        => _factory = factory;

    // READ: Retrieve products
    public async Task<List<Product>> GetAllAsync()
    {
        await using var db =
            await _factory.CreateDbContextAsync();

        return await db.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    // CREATE: Add a product (CORRECTED)
    public async Task AddAsync(Product product)
    {
        await using var db =
            await _factory.CreateDbContextAsync();

        // 1. Database constraint crash se bachne ke liye naye product ko fresh UUID dein
        product.ProductId = Guid.NewGuid();

        // 2. PostgreSQL timestamptz ke liye explicit UTC time set karein
        product.CreatedAt = DateTime.UtcNow;

        db.Products.Add(product);
        await db.SaveChangesAsync();
    }
    // UPDATE: Modify a product
    public async Task UpdateAsync(Product product)
    {
        await using var db = await _factory.CreateDbContextAsync();

        // PostgreSQL timestamptz compatibility validation
        if (product.CreatedAt.Kind != DateTimeKind.Utc)
        {
            product.CreatedAt = DateTime.SpecifyKind(product.CreatedAt, DateTimeKind.Utc);
        }

        db.Products.Update(product);
        await db.SaveChangesAsync();
    }


    // DELETE: Remove a product
    public async Task DeleteAsync(Guid id)
    {
        await using var db =
            await _factory.CreateDbContextAsync();

        var product = await db.Products.FindAsync(id);

        if (product is null)
            return;

        db.Products.Remove(product);
        await db.SaveChangesAsync();
    }
}
