using Microsoft.AspNetCore.Mvc;
using WEB_153504_Klimkovich.Domain.Entities;
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

        public async Task<IActionResult> Index(string? category)
        {
            var productResponse = await _productService.GetProductListAsync(category);
            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);

            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Success)
                return NotFound(productResponse.ErrorMessage);

            ViewData["currentCategory"] = category == null ? "Всё" : categoriesResponse.Data.Find(arg => arg.NormalizedName == category).Name;
            ViewData["categories"] = categoriesResponse.Data;
            return View(productResponse.Data.Items);
        }
    }
}
