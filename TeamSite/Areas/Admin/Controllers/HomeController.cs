using Microsoft.AspNetCore.Mvc;
using TeamSite.Models;
using Microsoft.AspNetCore.Authorization;
using TeamSite.Models.ViewModels;
using System.Collections.Generic;

namespace TeamSite.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admins")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly TeamSiteDbContext db;
        private _SubNavViewModel subNavViewModel;

        public HomeController(TeamSiteDbContext _db)
        {
            db = _db;
            subNavViewModel = new _SubNavViewModel
            {
                Title = "Admin",
                Tabs = new List<Tab>
                {
                    new Tab
                    {
                        Name = "Users",
                        Area = "Admin",
                        Controller = "User",
                        Action = "Index"
                    },
                    new Tab
                    {
                        Name = "Roles",
                        Area = "Admin",
                        Controller = "Role",
                        Action = "Index"
                    },
                    new Tab
                    {
                        Name = "Schedule",
                        Area = "Admin",
                        Controller = "Schedule",
                        Action = "Index"
                    }
                }
            };
        }

        public IActionResult Index()
        {
            return View(subNavViewModel);
        }
    }
}
