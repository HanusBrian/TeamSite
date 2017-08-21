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
        public int PageSize = 50;

        public ScheduleController(TeamSiteDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index(int page = 1)
        {
            ScheduleIndexModel model = new ScheduleIndexModel();

            model.Schedules = db.Schedule
                                .OrderBy(id => id.ScheduleId)
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize);
            model.Programs = from program in db.Program
                             select program;

            return View(model);
        }
    }
}
