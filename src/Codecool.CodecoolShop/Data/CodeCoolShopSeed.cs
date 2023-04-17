using System.Linq;
using System.Threading;
using Codecool.CodecoolShop.Models;

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
        if (!_codeCoolShopDbContext.Products.Any())
        {
            var product1 = new Product()
            {
                Name = "Lenovo IdeaPad Miix 700",
                Currency = "USD",
                DefaultPrice = 479.0m,
                ProductCategory = ProductCategory.Computer,
                Supplier = new Supplier()
                {
                    Name = "Lenovo"
                }
            };
            _codeCoolShopDbContext.Add(product1);
            _codeCoolShopDbContext.SaveChanges();
        }
    }
}