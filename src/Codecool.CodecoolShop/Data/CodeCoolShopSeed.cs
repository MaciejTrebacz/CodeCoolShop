using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using System.Linq;

namespace Codecool.CodecoolShop.Data;

public class CodeCoolShopSeed
{
    private readonly CodeCoolShopDBContext _codeCoolShopDbContext;

    public CodeCoolShopSeed(CodeCoolShopDBContext codeCoolShopDbContext)
    {
        _codeCoolShopDbContext = codeCoolShopDbContext;
    }

    public void Seed()
    {
        if (_codeCoolShopDbContext.Database.CanConnect())
        {
            if (!_codeCoolShopDbContext.Products.Any())
            {
                var supplier = new Supplier()
                {
                    Name = "Amazon",
                    Description = "Digital content and services"
                };

                _codeCoolShopDbContext.AddRange(new List<Product>()
                {
                    new Product()
                    {
                        Name = "Amazon Fire",
                        Currency = "USD",
                        DefaultPrice = 49.9m,
                        ProductCategory = ProductCategory.Tablet,
                        Supplier = supplier
                    },
                    new Product()
                    {
                        Name = "Lenovo IdeaPad Miix 700",
                        Currency = "USD",
                        DefaultPrice = 479.0m,
                        ProductCategory = ProductCategory.Computer,
                        Supplier = new Supplier()
                        {
                            Name = "Lenovo",
                            Description = "Computers"
                        }
                    },
                    new Product()
                    {
                        Name = "Amazon Fire HD 8",
                        Currency = "USD",
                        DefaultPrice = 89.0m,
                        ProductCategory = ProductCategory.Tablet,
                        Supplier = supplier
                    }
                });
                _codeCoolShopDbContext.SaveChanges();
            }
        }
    }
}