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
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Electronics> Electronics { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var result = await _productService.GetProductListAsync(null);

            if (result != null && result.Data != null)
            {
                Electronics = result.Data.Items;
            }
        }
    }

}
