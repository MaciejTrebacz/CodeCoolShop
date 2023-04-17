using System.Collections.Generic;

namespace Codecool.CodecoolShop.Models
{
    public class Supplier : BaseModel
    {
        public List<Product> Products { get; set; }
        
        public override string ToString()
        {
            return new string($"Id: {Id} Name: {Name} Description: {Description}");
        }
    }
}
