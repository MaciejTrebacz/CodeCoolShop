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
    public DbSet<LineItem> LineItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasOne(x => x.Supplier);
    }
}