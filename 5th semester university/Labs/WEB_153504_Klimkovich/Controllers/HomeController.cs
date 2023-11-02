using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153504_Klimkovich.Entities;

namespace WEB_153504_Klimkovich.Controllers
{
    public class HomeController : Controller
    {
        [ViewData]
        public string NameLab { get; set; }

        [ViewData]
        public SelectList ListDemo { get; set; }

        public IActionResult Index()
        {
            ListDemo = new SelectList(new List<ListDemo>
            {
                new ListDemo { Id = 1, Name = "Элемент 1" },
                new ListDemo { Id = 2, Name = "Элемент 2" },
                new ListDemo { Id = 3, Name = "Элемент 3" }
            }, "Id", "Name");
            NameLab = "Лабораторная работа №2";

            return View();
        }
    }
}
