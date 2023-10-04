using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;
using WEB_153504_Klimkovich.Services.ProductService;

namespace WEB_153504_Klimkovich.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public ListModel<Electronics> Electronics { get; set; } = default!;

        public async Task OnGetAsync(int pageNo = 1)
        {
            var result = await _productService.GetProductListAsync(null, pageNo);

            if (result.Success)
            {
                Electronics = result.Data;
            }
        }
    }
}
