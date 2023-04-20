using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Codecool.CodecoolShop.Models.Products
{
    public class ProductViewModel
    {
        public ProductCategory? ProductCategory { get; set; }
        public int? SupplierId { get; set; }
        public List<Product> Products { get; set; }
        public IEnumerable<SelectListItem> Suppliers { get; set; }

        public int IndexOf(Product element)
        {
            return Products.IndexOf(element);
        }
    }
}