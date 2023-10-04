using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Services.CategoryService;
using WEB_153504_Klimkovich.Services.ProductService;

namespace WEB_153504_Klimkovich.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
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

            var categoryList = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryList.Data, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.UpdateProductAsync(Electronics.Id, Electronics, Image);

            return RedirectToPage("./Index");
        }
    }

}
