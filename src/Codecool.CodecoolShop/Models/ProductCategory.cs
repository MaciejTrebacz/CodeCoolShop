using System.Collections.Generic;

namespace Codecool.CodecoolShop.Models
{
    public class ProductCategory: BaseModel
    {
        public List<Product> Products { get; set; }
        public string Department { get; set; }
    }
}
