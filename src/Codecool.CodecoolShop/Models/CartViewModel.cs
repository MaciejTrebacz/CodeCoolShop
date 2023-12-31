﻿using System.Collections.Generic;
using Codecool.CodecoolShop.Logic;

namespace Codecool.CodecoolShop.Models
{
    public class CartViewModel
    {
        public ShoppingCart Cart { get; set; }
        public List<Product> Products { get; set; }
    }
}