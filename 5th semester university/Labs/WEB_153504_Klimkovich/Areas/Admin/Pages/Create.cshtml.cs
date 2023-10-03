using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153504_Klimkovich.Services.ProductService;
using WEB_153504_Klimkovich.Domain.Entities;

namespace WEB_153504_Klimkovich.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;

        public CreateModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Electronics Electronics { get; set; } = default!;

        public IActionResult OnGet()
        {
            // Здесь вы можете получить список категорий из ProductService,
            // если это необходимо для вашей страницы создания.
            // ViewData["CategoryId"] = new SelectList(_productService.GetCategories(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Electronics == null)
            {
                return Page();
            }

            await _productService.CreateProductAsync(Electronics, null); // Вместо null можете передать форму для загрузки файла, если необходимо.

            return RedirectToPage("./Index");
        }
    }

}
