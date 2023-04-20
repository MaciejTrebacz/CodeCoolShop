using Codecool.CodecoolShop.Logic;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Codecool.CodecoolShop.Controllers
{
    public class CartController : Controller
    {

        private readonly ILogger _logger;
        private readonly ProductService _productService;

        public CartController(ILogger<ProductController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult ViewCart()
        {
            var cart = GetCart();
            var productIds = cart.Items.Keys.ToList();
            var products = productIds.Select(productId => _productService.GetProduct(productId)).ToList();

            var model = new CartViewModel
            {
                Cart = cart,
                Products = products
            };

            return View(model);
        }

        public IActionResult AddToCart(int productId)
        {
            var cart = GetCart();
            cart.Items.TryGetValue(productId, out var currentCount);
            cart.Items[productId] = currentCount + 1;
            SaveCart(cart);
            return RedirectToAction("ViewCart");
        }

        public ShoppingCart GetCart()
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

        public void SaveCart(ShoppingCart cart)
        {
            Debug.WriteLine("Saved cart");
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }
    }
}
