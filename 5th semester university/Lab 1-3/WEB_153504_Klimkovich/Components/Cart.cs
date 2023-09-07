using Microsoft.AspNetCore.Mvc;

namespace WEB_153504_Klimkovich.Components
{
    public class Cart : ViewComponent
    {
        private IRepository repository;
        public Cart(IRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            return View(repository.GetAll());
        }

    }
}
