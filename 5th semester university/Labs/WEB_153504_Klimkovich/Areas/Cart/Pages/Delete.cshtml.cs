using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153504_Klimkovich.Domain.Entities;

namespace WEB_153504_Klimkovich.Areas.Cart.Pages
{
    public class DeleteModel : PageModel
    {

        private readonly Domain.Cart _cart;
        public DeleteModel(Domain.Cart cart)
        {
            _cart = cart;
        }

        public CartItem CartItem { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _cart.CartItems[id.Value];

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                CartItem = result;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _cart.RemoveItem(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
