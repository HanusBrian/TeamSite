using Microsoft.AspNetCore.Mvc;
using TeamSite.Models;
using TeamSite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace TeamSite.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AADbContext _db;
        public HomeController(AADbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
