using Microsoft.AspNetCore.Mvc;

namespace TeamSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tools()
        {
            ViewData["Message"] = "Productivity Apps";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
