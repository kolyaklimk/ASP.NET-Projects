using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153504_Klimkovich.Domain.Entities;

namespace WEB_153504_Klimkovich.Areas.Cart.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Domain.Cart _cart;
        public IndexModel(Domain.Cart cart)
        {
            _cart = cart;
        }

        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        public void OnGet()
        {
            CartItems = _cart.CartItems;
        }
    }
}
