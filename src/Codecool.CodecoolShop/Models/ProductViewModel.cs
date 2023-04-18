using System.Collections.Generic;

namespace Codecool.CodecoolShop.Models
{
    public class ProductViewModel
    {
        public ProductCategory? ProductCategory { get; set; }
        public List<Product> Products { get; set; }

        public int IndexOf(Product element)
        {
            return Products.IndexOf(element);
        }
    }
}