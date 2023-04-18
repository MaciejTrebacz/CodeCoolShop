using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductService _productService;
        private readonly SupplierService _supplierService;

        public ProductController(ILogger<ProductController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var model = _productService.GetProducts();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Sort()
        {
            var model = new ProductViewModel
            {
                Products = _productService.GetProducts()
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Sort(ProductCategory? productCategory)
        {
            var products = _productService.GetProducts();
            if (productCategory.HasValue)
            {
                products = products.Where(p => p.ProductCategory == productCategory.Value).ToList();
            }

            var model = new ProductViewModel
            {
                ProductCategory = productCategory,
                Products = products
            };

            return View(model);
        }


        public async Task<IActionResult> SortBySupplier()
        {
            return View();
        }
    }
}
