using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace TeamSite.Controllers
{
    [Authorize]
    public class FormBuilderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadTemplate(List<IFormFile> files)
        {
            
            return View();
        }
    }
}
