using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Contact()
        {
            ViewData["Message"] = "A&A contact page";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
