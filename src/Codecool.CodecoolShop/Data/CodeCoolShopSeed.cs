using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using System.Linq;
using Bogus;

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
                //var supplier = new Supplier()
                //{
                //    Name = "Amazon",
                //    Description = "Digital content and services"
                //};

                //_codeCoolShopDbContext.AddRange(new List<Product>()
                //{
                //    new Product()
                //    {
                //        Name = "Amazon Fire",
                //        Currency = "USD",
                //        DefaultPrice = 49.9m,
                //        ProductCategory = ProductCategory.Tablet,
                //        Supplier = supplier
                //    },
                //    new Product()
                //    {
                //        Name = "Lenovo IdeaPad Miix 700",
                //        Currency = "USD",
                //        DefaultPrice = 479.0m,
                //        ProductCategory = ProductCategory.Computer,
                //        Supplier = new Supplier()
                //        {
                //            Name = "Lenovo",
                //            Description = "Computers"
                //        }
                //    },
                //    new Product()
                //    {
                //        Name = "Amazon Fire HD 8",
                //        Currency = "USD",
                //        DefaultPrice = 89.0m,
                //        ProductCategory = ProductCategory.Tablet,
                //        Supplier = supplier
                //    }
                //});

                var supplierFaker = new Faker<Supplier>()
                    .RuleFor(p => p.Name, f => f.Company.CompanyName())
                    .RuleFor(p => p.Description, f => f.Company.CompanySuffix());

                var productFaker = new Faker<Product>()
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Currency, "USD")
                    .RuleFor(p => p.DefaultPrice, f => f.Random.Decimal(50, 1000))
                    .RuleFor(p => p.ProductCategory, f => (ProductCategory)f.Random.Int(0, 2))
                    .RuleFor(p => p.Description, f => f.Lorem.Sentence(5))
                    .RuleFor(p => p.Supplier, f => supplierFaker.Generate());


                var products = productFaker.Generate(100);
                _codeCoolShopDbContext.AddRange(products);
                _codeCoolShopDbContext.SaveChanges();
            }
        }
    }
}