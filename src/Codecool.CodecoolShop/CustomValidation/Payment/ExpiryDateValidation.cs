using System;
using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.CustomValidation.Payment
{
    public class ExpiryDateValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            return dateTime > DateTime.Now;
        }
    }
}
