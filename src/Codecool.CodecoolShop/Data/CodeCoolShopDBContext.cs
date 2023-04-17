using Codecool.CodecoolShop.Models;
using Microsoft.EntityFrameworkCore;

namespace Codecool.CodecoolShop.Data;

public class CodeCoolShopDBContext : DbContext
{
    public CodeCoolShopDBContext(DbContextOptions<CodeCoolShopDBContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Lenovo IdeaPad Miix 700",
                Currency = "USD",
                DefaultPrice = 479.0m,
                ProductCategory = ProductCategory.Computer,
                //Supplier = new Supplier()
                //{
                //    Name = "Lenovo"
                //}
            });
    }
}