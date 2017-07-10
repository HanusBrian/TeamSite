using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeamSite.Models;

namespace TeamSite.Controllers
{
    public class TestDbController : Controller
    {
        private readonly ILogger _logger;
        private readonly AADbContext _db;
        public TestDbController(ILogger<TestDbController> logger, AADbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(string name, string email)
        {
            _logger.LogInformation("***You have pressed the new button!!***");
            _logger.LogWarning("***This is a warning!!!***");
            _logger.LogWarning(name + " " + email);
            Contact contact = new Contact()
            {
                Name = name,
                Email = email
            };
            _db.Add<Contact>(contact);
            await _db.SaveChangesAsync();
            return View("Index");
        }
    }
}
