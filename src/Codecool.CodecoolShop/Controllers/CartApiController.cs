﻿using Codecool.CodecoolShop.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        [HttpPost]
        public void AdjustCartQuantity(AdjustCartQuantityParameters parameters)
        {
            var cart = GetCart();
            cart.Items[parameters.productId] = parameters.quantity;
            SaveCart(cart);
        }

        [HttpPost]
        public void RemoveFromCart(RemoveFromCartParameters parameters)
        {
            var cart = GetCart();
            cart.Items.Remove(parameters.productId);
            SaveCart(cart);
        }

        private ShoppingCart GetCart()
        {
            ShoppingCart cart;
            if (HttpContext.Session.Get("Cart") != null)
            {
                Debug.WriteLine("Found existing cart");
                cart = JsonSerializer.Deserialize<ShoppingCart>(HttpContext.Session.Get("Cart"));
            }
            else
            {
                Debug.WriteLine("Created new cart");
                cart = new ShoppingCart();
                SaveCart(cart);
            }

            return cart;
        }

        private void SaveCart(ShoppingCart cart)
        {
            Debug.WriteLine("Saved cart");
            foreach (var item in cart.Items.Keys.ToList().Where(key => cart.Items[key] == 0))
            {
                cart.Items.Remove(item);
            }
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }
    }
}
