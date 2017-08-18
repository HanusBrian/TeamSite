using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TeamSite.Models.ViewModels;
using TeamSite.Models;
using Microsoft.AspNetCore.Authorization;

namespace TeamSite.Controllers
{
    [Authorize]
    public class ScheduleController : Controller
    {
        private readonly TeamSiteDbContext db;

        public ScheduleController(TeamSiteDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            ScheduleIndexModel model = new ScheduleIndexModel();

            model.Schedules = from scheduleItem in db.Schedule
                              select scheduleItem;
            model.Programs = from program in db.Program
                             select program;

            return View(model);
        }
    }
}
