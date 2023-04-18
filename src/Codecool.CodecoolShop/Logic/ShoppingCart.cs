using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Codecool.CodecoolShop.Logic
{
    public class ShoppingCart 
    {
        public int Id { get; set; }
        public Dictionary<int, int> Items { get; set; }

        public ShoppingCart()
        {
            Items = new Dictionary<int, int>();
        }

    }
}
