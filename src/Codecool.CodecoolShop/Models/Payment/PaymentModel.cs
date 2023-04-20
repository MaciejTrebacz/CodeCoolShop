using System;
using System.ComponentModel.DataAnnotations;
using Codecool.CodecoolShop.CustomValidation.Payment;

namespace Codecool.CodecoolShop.Models.Payment
{
    public class PaymentModel
    {
        [Required]
        public PaymentProviders PaymentProvider { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string CardNumber { get; set; }
        [Required]
        public string CardHolder { get; set; }
        [Required]
        [ExpiryDateValidation(ErrorMessage = "Wrong expiry date")]
        public DateTime ExpiryDate { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string CVV { get; set; }
    }
}
