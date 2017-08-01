using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TeamSite.Models;
using TeamSite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TeamSite.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AADbContext _db;
        public EmployeeController(AADbContext db)
        {
            _db = db;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            AdminEmployeesViewModel viewModel = new AdminEmployeesViewModel()
            {
                Employees = from e in _db.Employees
                            select e
            };

            return View(viewModel);
        }

        public IActionResult Show(int employeeid)
        {
            AdminEditEmployeeViewModel viewModel = new AdminEditEmployeeViewModel
            {
                Employee = (from e in _db.Employees
                            where e.EmployeeID == employeeid
                            select e).SingleOrDefault()
            };
            return View(viewModel);
        }

        public IActionResult TeamAlignment()
        {
            return View();
        }
    }
}
