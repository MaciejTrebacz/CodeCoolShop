using Codecool.CodecoolShop.Models.Payment;
using Codecool.CodecoolShop.Models.Products;
using Codecool.CodecoolShop.Models.UserData;

namespace Codecool.CodecoolShop.Models
{
    public class OrderModel
    {
        public ProductsCart Products { get; set; }
        public UserDataModel UserData { get; set; }
        public PaymentModel Payment { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    public enum OrderStatus
    {
        Success,
        AwaitingPayment,
        Received
    }
}
