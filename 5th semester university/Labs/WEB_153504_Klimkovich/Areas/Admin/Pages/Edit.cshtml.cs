using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Services.ProductService;

namespace WEB_153504_Klimkovich.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;

        public EditModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty] public Electronics Electronics { get; set; } = default!;
        [BindProperty] public IFormFile? Image { get; set; }
        public SelectList CategoryList { get; set; }

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

            // Загрузка списка категорий из ProductService, если необходимо
            // CategoryList = new SelectList(await _productService.GetCategories(), "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.UpdateProductAsync(Electronics.Id, Electronics, Image); // Вместо null можете передать форму для загрузки файла, если необходимо.


            return RedirectToPage("./Index");
        }
    }

}
