using Microsoft.AspNetCore.Mvc;

namespace WEB_153504_Klimkovich.Controllers
{
    public class HomeController : Controller
    {
        [ViewData]
        public string NameLab { get; set; }

        public IActionResult Index()
        {
            NameLab = "Лабораторная работа №2";
            return View();
        }
    }
}
