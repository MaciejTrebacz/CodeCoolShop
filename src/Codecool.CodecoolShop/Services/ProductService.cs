using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Data;
using Codecool.CodecoolShop.Logic;
using Codecool.CodecoolShop.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Codecool.CodecoolShop.Services
{
    public class ProductService
    {
        private readonly CodeCoolShopDBContext _dbContext;

        public ProductService(CodeCoolShopDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Product> GetProducts()
        {
         return _dbContext.Products
             .Include(x => x.Supplier)
             .ToList();
        }

        public ProductsCart GetProductsCartByShoppingCart(ShoppingCart cart)
        {
            var products = new ProductsCart();

            foreach (var (id, amount) in cart.Items)
            {
                var product = _dbContext.Products
                    .Include(supplier => supplier.Supplier)
                    .First(product => product.Id == id);

                products.Products.Add(product, amount);
            }
            return products;
        }

        public Product GetProductById(int id)
        {
            return _dbContext.Products
                .Include(product => product.Supplier)
                .First(product => product.Id == id);
        }
    }
}
