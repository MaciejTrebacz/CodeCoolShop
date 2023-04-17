using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Data;
using Codecool.CodecoolShop.Models;
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
    }
}
