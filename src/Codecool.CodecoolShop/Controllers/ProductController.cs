using Codecool.CodecoolShop.Logic;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Products;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductService _productService;
        private readonly SupplierService _supplierService;
        private readonly ShoppingCartLogic _shoppingCartLogic;

        public ProductController(ILogger<ProductController> logger, ProductService productService, SupplierService supplierService)
        {
            _logger = logger;
            _productService = productService;
            _supplierService = supplierService;
            _shoppingCartLogic = new ShoppingCartLogic();
        }

        public IActionResult Index()
        {
            var cart = _shoppingCartLogic.GetCart(HttpContext);
            var model = _productService.GetProducts();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Product/{productId}/Details")]
        public IActionResult Details(int productId)
        {
            var product = _productService.GetProductById(productId);
            return View(product);
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
