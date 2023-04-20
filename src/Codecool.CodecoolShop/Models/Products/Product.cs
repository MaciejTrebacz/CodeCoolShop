namespace Codecool.CodecoolShop.Models.Products
{
    public class Product : BaseModel
    {
        public string Currency { get; set; }
        public decimal DefaultPrice { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public Supplier Supplier { get; set; }
        public string Image { get; set; }
    }
}
