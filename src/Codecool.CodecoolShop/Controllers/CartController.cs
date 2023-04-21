using Codecool.CodecoolShop.Logic;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.DTO;
using Codecool.CodecoolShop.Models.Payment;
using Codecool.CodecoolShop.Models.UserData;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Formatting.Json;
using Serilog;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Codecool.CodecoolShop.Controllers
{
    public class CartController : Controller
    {

        private ILogger<ProductController> _logger;
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<CartController> cartLogger;
        private readonly ShoppingCartLogic _shoppingCartLogic;


        public CartController(ILogger<ProductController> logger, ProductService productService, IMapper mapper, ILogger<CartController> cartLogger)
        {
            _logger = logger;
            _productService = productService;
            _mapper = mapper;
            this.cartLogger = cartLogger;
            _shoppingCartLogic = new ShoppingCartLogic();
        }

        public IActionResult ViewCart()
        {
            
            var cart = _shoppingCartLogic.GetCart(HttpContext);
            var productIds = cart.Items.Keys.ToList();
            var products = productIds.Select(productId => _productService.GetProductById(productId)).ToList();

            var model = new CartViewModel
            {
                Cart = cart,
                Products = products
            };

            return View(model);
        }

        public IActionResult AddToCart(int productId)
        {
            var cart = _shoppingCartLogic.GetCart(HttpContext);
            cart.Items.TryGetValue(productId, out var currentCount);
            cart.Items[productId] = currentCount + 1;
            _shoppingCartLogic.SaveCart(cart, HttpContext);
            return RedirectToAction("ViewCart");
        }

        public IActionResult Checkout()
        {
            var cart = JsonSerializer.Deserialize<ShoppingCart>(HttpContext.Session.Get("Cart"));

            if (cart.Items.Count == 0) return StatusCode(403);
            if (HttpContext.Session.Get("UserData") == null) return View();

            var userData = JsonSerializer.Deserialize<UserDataModel>(HttpContext.Session.Get("UserData"));

            return View(userData);
        }

        [HttpPost]
        public IActionResult Checkout(UserDataModel userData)
        {

            if (!ModelState.IsValid)
            {
                return View(userData);
            }

            var newOrder = new OrderModel
            {
                OrderStatus = OrderStatus.Received,
                UserData = userData
            };


            cartLogger.LogInformation("{@order}", newOrder);

            

            HttpContext.Session.SetString("UserData", JsonSerializer.Serialize(userData));
            HttpContext.Session.SetString("OrderModel", JsonSerializer.Serialize(newOrder));


            return RedirectToAction("Payment");
        }

        public IActionResult Payment()
        {
            if (HttpContext.Session.Get("UserData") == null) return StatusCode(403);

            return View();
        }

        [HttpPost]
        public IActionResult Payment(PaymentModel payment)
        {
            if (!ModelState.IsValid)
            {
                return View(payment);
            }


            HttpContext.Session.SetString("Payment", JsonSerializer.Serialize(payment));
            var newOrder = JsonSerializer.Deserialize<OrderModel>(HttpContext.Session.GetString("OrderModel"));
            newOrder.OrderStatus = OrderStatus.MoneyReceived;
            cartLogger.LogInformation("{@order}", newOrder);

            HttpContext.Session.SetString("OrderModel", JsonSerializer.Serialize(newOrder));


            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {

            if (HttpContext.Session.Get("UserData") == null
                || HttpContext.Session.Get("Payment") == null) return StatusCode(403);


            var cart = JsonSerializer.Deserialize<ShoppingCart>(HttpContext.Session.Get("Cart"));

            var newOrder = JsonSerializer.Deserialize<OrderModel>(HttpContext.Session.GetString("OrderModel"));
            newOrder.OrderStatus = OrderStatus.Success;
            cartLogger.LogInformation("{@ORDER}", newOrder);

            var order = new OrderModel()
            {
                Products = _productService.GetProductsCartByShoppingCart(cart),
                Payment = JsonSerializer.Deserialize<PaymentModel>(HttpContext.Session.Get("Payment")),
                UserData = JsonSerializer.Deserialize<UserDataModel>(HttpContext.Session.Get("UserData"))
            };


            if (Request.Method == "POST")
            {
                HttpContext.Session.Clear();

                var productsDto = _mapper.Map<List<ProductDto>>(order.Products.Products);
                productsDto.ForEach(x => x.Subtotal = x.PricePerUnit * x.Quantity);

                var jsonOrder = new OrderToFileModel()
                {
                    Payment = order.Payment,
                    UserData = order.UserData,
                    Products = productsDto
                };

                string filePath =
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\orders\\{cart.Id}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.json";

                SaveToFile.ToJson(jsonOrder, filePath);

                //TODO send email to user about order

                return RedirectToAction("Index", "Product");
            }
            return View(order);
        }
    }

    public class FilePath
    {
        public static string Path { get; set; }

    }
}
