using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using TeamSite.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using TeamSite.Models.ViewModels;

namespace TeamSite.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admins")]
    [Area("Admin")]
    public class ScheduleController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;
        private readonly TeamSiteDbContext db;
        private readonly UserManager<AppUser> userManager;
        private readonly _SubNavViewModel subNavViewModel;

        public ScheduleController(TeamSiteDbContext _db, IHostingEnvironment _hostingEnvironment, ILogger<ScheduleController> _logger, UserManager<AppUser> usrmgr)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            logger = _logger;
            userManager = usrmgr;

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
                        Name = "Upload Schedule",
                        Area = "Admin",
                        Controller = "Schedule",
                        Action = "Upload"
                    }
                }
            };
        }

        public IActionResult Index()
        {
            return View(subNavViewModel);
        }

        public ViewResult Upload()
        {
            return View(subNavViewModel);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> Upload(List<IFormFile> file)
        {
            ExcelTools excelTools = new ExcelTools(logger, hostingEnvironment);

            FileSystem fs = new FileSystem(hostingEnvironment, logger);

            string sWebRootFolder = hostingEnvironment.WebRootPath + "/filesystem/";
            string sFileName = file[0].FileName;
            FileInfo fileInfo = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                string[,] array = excelTools.ExcelToStringArray(fileInfo, "DeploymentRequests");

                int numRows = array.GetLength(0);

                String[] row;
                for (int i = 1; i < numRows; i++)
                {
                    row = excelTools.GetRowFromArray(array, i);

                    int excelIndex = CastToInt(row[0]);
                    string clientName = row[1];
                    string programName = row[2];
                    string stringDate = row[8];
                    bool complete = row[11] == "Y" ? true : false;
                    int revRank = CastToInt(row[14]);

                    DateTime targetDate;
                    DateTime.TryParse(row[8], out targetDate);
                    if (targetDate == null)
                    {
                        targetDate = new DateTime(01 / 01 / 1000);
                    }

                    if (!db.Client.Any(x => x.Name == clientName))
                    {
                        Client newClient = new Client
                        {
                            Name = clientName,
                            RevenueRank = revRank,
                            CreateDate = DateTime.Now,
                            CreateUser = userManager.GetUserName(User),
                            ChangeDate = null,
                            ChangeUser = null
                        };
                        await db.AddAsync(newClient);
                        await db.SaveChangesAsync();
                    }

                    if (!db.Program.Any(x => x.Name == programName))
                    {
                        Models.Program newProgram = new Models.Program
                        {
                            Name = programName,
                            ClientId = db.Client.Where(x => x.Name == clientName).Select(x => x).SingleOrDefault(),
                            CreateDate = DateTime.Now,
                            CreateUser = userManager.GetUserName(User),
                            ChangeDate = null,
                            ChangeUser = null
                        };
                        await db.AddAsync(newProgram);
                        await db.SaveChangesAsync();
                    }

                    if (!db.Schedule.Any(x => x.ExcelIndex == excelIndex))
                    {
                        Schedule schedule = new Schedule
                        {
                            ExcelIndex = excelIndex,
                            ClientId = db.Client.Where(x => x.Name == clientName).Select(x => x).SingleOrDefault().ClientID,
                            LaunchDate = targetDate,
                            ProgramId = db.Program.Where(x => x.Name == programName).Select(x => x).SingleOrDefault().ProgramId,
                            Complete = complete,
                            CreateDate = DateTime.Now,
                            CreateUser = userManager.GetUserName(User),
                            ChangeDate = null,
                            ChangeUser = null
                        };
                        await db.AddAsync(schedule);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Some error occured in UploadFiles." + ex.Message + " " + ex.StackTrace);
                return RedirectToAction("Import");
            }
            finally
            {
                FileSystem.CleanTempFile(fileInfo.ToString());
            }

            return RedirectToAction("Index");
        }

        private int CastToInt(string input)
        {
            int result;
            if (!Int32.TryParse(input, out result))
            {
                result = -1;
            }
            return result;
        }
    }
}
