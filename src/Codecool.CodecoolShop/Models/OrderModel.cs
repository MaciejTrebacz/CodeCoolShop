using Codecool.CodecoolShop.Models.NewFolder;
using Codecool.CodecoolShop.Models.Payment;
using Codecool.CodecoolShop.Models.UserData;

namespace Codecool.CodecoolShop.Models
{
    public class OrderModel
    {
        public ProductsCart Products { get; set; }
        public UserDataModel UserData { get; set; }
        public PaymentModel Payment { get; set; }
    }
}
