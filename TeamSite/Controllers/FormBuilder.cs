using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace TeamSite.Controllers
{
    public class FormBuilder : Controller
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
