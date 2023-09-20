using Microsoft.AspNetCore.Mvc;

namespace WEB_153504_Klimkovich.Services.CategoryService
{
    public class ApiCategoryService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
