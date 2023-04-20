using System;
using Codecool.CodecoolShop.Models.Payment;
using Codecool.CodecoolShop.Models.Products;
using Codecool.CodecoolShop.Models.UserData;

namespace Codecool.CodecoolShop.Models
{
    public class OrderModel
    {
        public ProductsCart? Products { get; set; } 
        public UserDataModel UserData { get; set; } 
        public PaymentModel Payment { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public DateTime OrderDateTime { get; set; } = DateTime.UtcNow;
    }

    public enum OrderStatus
    {
        Success,
        MoneyReceived,
        Received
    }
}
