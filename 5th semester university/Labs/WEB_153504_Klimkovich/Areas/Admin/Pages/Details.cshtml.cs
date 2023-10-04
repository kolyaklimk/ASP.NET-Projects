using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Services.ProductService;

namespace WEB_153504_Klimkovich.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

        public Electronics Electronics { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _productService.GetProductByIdAsync(id.Value);

            if (result == null || result.Data == null)
            {
                return NotFound();
            }
            else
            {
                Electronics = result.Data;
            }

            return Page();
        }
    }

}
