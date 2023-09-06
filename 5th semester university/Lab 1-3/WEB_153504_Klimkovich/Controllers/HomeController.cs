using Microsoft.AspNetCore.Mvc;

namespace WEB_153504_Klimkovich.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
