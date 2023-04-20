using System.Collections.Generic;
using Codecool.CodecoolShop.Models.DTO;
using Codecool.CodecoolShop.Models.Payment;
using Codecool.CodecoolShop.Models.UserData;

namespace Codecool.CodecoolShop.Models
{
    public class OrderToFileModel
    {
        public List<ProductDto> Products { get; set; }
        public UserDataModel UserData { get; set; }
        public PaymentModel Payment { get; set; }
    }
}
