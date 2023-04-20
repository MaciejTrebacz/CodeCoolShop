using Codecool.CodecoolShop.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ShoppingCartLogic _shoppingCartLogic;

        public CartApiController()
        {
            _shoppingCartLogic = new ShoppingCartLogic();
        }

        [HttpPost]
        public void AdjustCartQuantity(AdjustCartQuantityParameters parameters)
        {
            var cart = _shoppingCartLogic.GetCart(HttpContext);
            cart.Items[parameters.productId] = parameters.quantity;
            _shoppingCartLogic.SaveCart(cart, HttpContext);
        }

        [HttpPost]
        public void RemoveFromCart(RemoveFromCartParameters parameters)
        {
            var cart = _shoppingCartLogic.GetCart(HttpContext);
            cart.Items.Remove(parameters.productId);
            _shoppingCartLogic.SaveCart(cart, HttpContext);
        }
    }
}
