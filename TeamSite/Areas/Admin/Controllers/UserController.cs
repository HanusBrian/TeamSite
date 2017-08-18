using Microsoft.AspNetCore.Mvc;
using TeamSite.Models;
using TeamSite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace TeamSite.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admins")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly TeamSiteDbContext db;
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;
        private readonly IHostingEnvironment hostingEnvironment;
        private _SubNavViewModel subNavViewModel;

        public UserController(TeamSiteDbContext _db, UserManager<AppUser> usrmgr, RoleManager<IdentityRole> _roleManager, ILogger<UserController> _logger, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            userManager = usrmgr;
            roleManager = _roleManager;
            logger = _logger;
            hostingEnvironment = _hostingEnvironment;

            subNavViewModel = new _SubNavViewModel
            {
                Title = "Admin",
                Tabs = new List<Tab>
                {
                    new Tab
                    {
                        Name = "Upload Users",
                        Area = "Admin",
                        Controller = "User",
                        Action = "Upload"
                    },
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
            AdminUserIndexViewModel viewModel = new AdminUserIndexViewModel()
            {
                Title = subNavViewModel.Title,
                Tabs = subNavViewModel.Tabs,
                AppUsers = userManager.Users
            };

            return View(viewModel);
        }

        public IActionResult Upload()
        {
            _SubNavViewModel viewModel = new _SubNavViewModel()
            {
                Title = subNavViewModel.Title,
                Tabs = subNavViewModel.Tabs
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> file)
        {
            FileSystem fs = new FileSystem(hostingEnvironment, logger);

            string sWebRootFolder = hostingEnvironment.WebRootPath + "/filesystem/";
            string sFileName = file[0].FileName;
            FileInfo fileInfo = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                ExcelTools excelTools = new ExcelTools(logger, hostingEnvironment);
                string[,] array = excelTools.ExcelToStringArray(fileInfo, "Users");

                int numRows = array.GetLength(0);

                IdentityResult result;

                for (int i = 1; i < numRows; i++)
                {
                    string[] row = excelTools.GetRowFromArray(array, i);
                    string roleName = row[7];
                    string password = row[8];

                    AppUser user = new AppUser
                    {
                        UserName = row[1],
                        Email = row[2],
                        FirstName = row[3],
                        LastName = row[4],
                        PhoneNumber = row[5],
                        Position = row[6],
                    };

                    result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddToRoleAsync(user, roleName);

                    }

                    if (i == numRows - 1 && result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError("Some error occured in UploadFiles." + ex.Message + " " + ex.StackTrace);
                return View("UploadFiles", null);
            }
            finally
            {
                FileSystem.CleanTempFile(fileInfo.ToString());
            }

            return View();
        }

        public async Task<IActionResult> Show(string id)
        {
            AdminEditEmployeeViewModel viewModel = new AdminEditEmployeeViewModel
            {
                AppUser = await userManager.FindByIdAsync(id),
                Tabs = subNavViewModel.Tabs
            };
            return View(viewModel);
        }

        public IActionResult TeamAlignment()
        {
            return View(subNavViewModel);
        }
    }
}
