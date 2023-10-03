using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_153504_Klimkovich.Services.ProductService;
using WEB_153504_Klimkovich.Domain.Entities;

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
