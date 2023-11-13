using Microsoft.AspNetCore.Mvc;
using WEB_153504_Klimkovich.Extensions;
using WEB_153504_Klimkovich.Services.CategoryService;
using WEB_153504_Klimkovich.Services.ProductService;

namespace WEB_153504_Klimkovich.Controllers
{
    public class Product : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public Product(IProductService product, ICategoryService category)
        {
            _categoryService = category;
            _productService = product;
        }

        [Route("Catalog")]
        [Route("Catalog/{category}")]
        public async Task<IActionResult> Index(string? category, int pageno)
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Success)
                return NotFound(categoriesResponse.ErrorMessage);

            var productResponse = await _productService.GetProductListAsync(category, pageno);
            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);

            ViewData["currentCategory"] = category == null ? "Всё" : categoriesResponse.Data.Find(arg => arg.NormalizedName == category).Name;
            ViewData["categories"] = categoriesResponse.Data;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductListPartial", productResponse.Data);
            }

            return View(productResponse.Data);
        }
    }
}
