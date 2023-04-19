using System.Collections.Generic;

namespace Codecool.CodecoolShop.Models.NewFolder
{
    public class ProductsCart
    {
        public Dictionary<Product, int> Products { get; set; }

        public ProductsCart()
        {
            Products = new Dictionary<Product, int>();
        }
    }
}
