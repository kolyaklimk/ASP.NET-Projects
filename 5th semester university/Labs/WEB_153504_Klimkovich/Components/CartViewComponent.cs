using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Newtonsoft.Json;
using WEB_153504_Klimkovich.Domain;

namespace WEB_153504_Klimkovich.Components
{
    public class CartViewComponent : ViewComponent
    {
        public readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return new HtmlContentViewComponentResult(
                new HtmlString($"{_cart.TotalPrice} руб <i class=\"fa-solid fa-cart-shopping\"></i> ({_cart.Count})"));
        }
    }
}
