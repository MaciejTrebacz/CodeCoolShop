using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codecool.CodecoolShop.Logic;

public class ShoppingCartLogic
{

    public ShoppingCart GetCart(HttpContext httpContext)
    {
        ShoppingCart cart;
        if (httpContext.Session.Get("Cart") != null)
        {
            Debug.WriteLine("Found existing cart");
            cart = JsonSerializer.Deserialize<ShoppingCart>(httpContext.Session.Get("Cart"));
        }
        else
        {
            Debug.WriteLine("Created new cart");
            cart = new ShoppingCart();
            SaveCart(cart, httpContext);
        }

        return cart;
    }

    public void SaveCart(ShoppingCart cart, HttpContext httpContext)
    {
        Debug.WriteLine("Saved cart");
        foreach (var item in cart.Items.Keys.ToList().Where(key => cart.Items[key] == 0))
        {
            cart.Items.Remove(item);
        }
        httpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
    }
}