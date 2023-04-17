using Codecool.CodecoolShop.Models;
using System.Collections.Generic;

namespace Codecool.CodecoolShop.Daos
{
    public interface IProductDao : IDao<Product>
    {
        IEnumerable<Product> GetBy(Supplier supplier);
        IEnumerable<Product> GetBy(ProductCategory productCategory);
    }
}
