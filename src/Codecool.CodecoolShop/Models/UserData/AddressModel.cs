using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.Models.UserData
{
    public abstract class AddressModel
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Zipcode { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
