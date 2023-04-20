using Codecool.CodecoolShop.Logic;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Payment;
using Codecool.CodecoolShop.Models.UserData;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Codecool.CodecoolShop.Models.DTO;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductService _productService;
        private readonly SupplierService _supplierService;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger, ProductService productService, SupplierService supplierService, IMapper mapper)
        {
            _logger = logger;
            _productService = productService;
            _supplierService = supplierService;
            _mapper = mapper;
        }



        public void OneGet()
        {
            _logger.LogInformation("__________________________________________________________________________");
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
            var cart = JsonSerializer.Deserialize<ShoppingCart>(HttpContext.Session.Get("Cart"));

            if (cart.Items.Count == 0) return StatusCode(403);
            if (HttpContext.Session.Get("UserData") == null) return View();

            var userData = JsonSerializer.Deserialize<UserDataModel>(HttpContext.Session.Get("UserData"));

            return View(userData);
        }

        [HttpPost]
        public IActionResult CheckoutCart(UserDataModel userData)
        {
            if (!ModelState.IsValid)
            {
                return View(userData);
            }

            HttpContext.Session.SetString("UserData", JsonSerializer.Serialize(userData));

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

            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {
            if (HttpContext.Session.Get("UserData") == null
                || HttpContext.Session.Get("Payment") == null) return StatusCode(403);

            var cart = JsonSerializer.Deserialize<ShoppingCart>(HttpContext.Session.Get("Cart"));

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

                return RedirectToAction("Index");
            }

            return View(order);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    }
}
