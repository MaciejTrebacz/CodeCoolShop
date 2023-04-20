using System;
using Codecool.CodecoolShop.Logic;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Payment;
using Codecool.CodecoolShop.Models.UserData;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductService _productService;
        private readonly SupplierService _supplierService;
        public string FilePath { get; set; } 

        public ProductController(ILogger<ProductController> logger, ProductService productService,
            SupplierService supplierService)
        {
            _logger = logger;
            _productService = productService;
            _supplierService = supplierService;
        }



        public void OneGet()
        {

        }

        public IActionResult Index()
        {
            OneGet();
            var cart = GetCart();
            var model = _productService.GetProducts();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CheckoutCart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckoutCart(UserDataModel userData)
        {
            if (!ModelState.IsValid)
            {
                return View(userData);
            }

            var newOrder = new OrderModel
            {
                OrderStatus = OrderStatus.Received
            };


            HttpContext.Session.SetString("UserData", JsonSerializer.Serialize(userData));
            HttpContext.Session.SetString("OrderModel", JsonSerializer.Serialize(newOrder));


            return RedirectToAction("Payment");
        }
        
        public IActionResult Payment()
        {
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
            newOrder.OrderStatus = OrderStatus.Success;




            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {
     

            var cart = JsonSerializer.Deserialize<ShoppingCart>(HttpContext.Session.Get("Cart"));

            var order = new OrderModel()
            {
                Products = _productService.GetProductsCartByShoppingCart(cart),
                Payment = JsonSerializer.Deserialize<PaymentModel>(HttpContext.Session.Get("Payment")),
                UserData = JsonSerializer.Deserialize<UserDataModel>(HttpContext.Session.Get("UserData"))
            };
                //FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Data\\Log\\{cart.Id}.json";
            FilePath = Path.Combine(Environment.CurrentDirectory, "Data", "Log", $"{cart.Id}.json");
            Console.WriteLine();
            var log = new LoggerConfiguration()
                .WriteTo.File(new JsonFormatter(), FilePath)
                .CreateLogger();

            log.Information("Order UserName: {@name} " +
                            "Shipping Address: {@Address}" +
                            "Order Status : {@orderstatus}", order.UserData.Name, order.UserData.ShippingAddress, order.OrderStatus);

            if (Request.Method == "POST")
            {
                HttpContext.Session.Clear();

                var filePath = AppDomain.CurrentDomain.BaseDirectory + "\\orders\\";
                var fileName = $"{cart.Id}_{DateTime.UtcNow}";
                //TODO save to JSON file
                //SaveToFile.ToJson(order, filePath, fileName);
                //TODO send email to user about order
                return RedirectToAction("Index");
            }
            return View(order);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddToCart(int productId)
        {
            var cart = GetCart();
            cart.Items.TryGetValue(productId, out var currentCount);
            cart.Items[productId] = currentCount + 1;
            SaveCart(cart);
            return RedirectToAction("ViewCart");
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
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        public IActionResult Sort()
        {
            var model = new ProductViewModel
            {
                Products = _productService.GetProducts(),
                Suppliers = _supplierService.GetAllAsync().Result
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Sort(ProductCategory? productCategory,int? supplierId)
        {
            var products = _productService.GetProducts();
            if (productCategory.HasValue)
            {
                products = products.Where(p => p.ProductCategory == productCategory.Value).ToList();
            }

            if (supplierId.HasValue)
            {
                products = products.Where(p=>p.Supplier.Id == supplierId.Value).ToList();
            }

            var model = new ProductViewModel
            {
                ProductCategory = productCategory,
                SupplierId = supplierId,
                Products = products,
                Suppliers = _supplierService.GetAllAsync().Result
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
            };

            return View(model);
        }



        public async Task<IActionResult> SortBySupplier()
        {
            return View();
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
    }
}
