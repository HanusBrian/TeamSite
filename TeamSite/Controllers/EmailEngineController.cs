using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TeamSite.Models;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using Microsoft.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Linq;

namespace TeamSite.Controllers
{
    public class EmailEngineController : Controller
    {
        private readonly ILogger _logger;
        private readonly AADbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;
        public EmailEngineController(ILogger<EmailEngineController> logger, AADbContext db, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _db = db;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFiles(List<IFormFile> files, DateTime startDate, DateTime endDate)
        {
            long size = 0;
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename = _hostingEnvironment.WebRootPath + "\\" + file.FileName;
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            //try
            //{
                string sWebRootFolder = _hostingEnvironment.WebRootPath;
                string sFileName = files[0].FileName;
                FileInfo filePath = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

                String[,] data = Import(filePath);

                // Find TargetDate (Column 9 "I") that is between the chosen startDate and endDate
                List<String[]> result = FindRowsInDateRange(data, startDate, endDate);

                return View("UploadFiles", result);
            //}
            //catch(Exception e)
            //{
            //    return View("UploadFiles", null);
            //}
        }

        public List<String[]> FindRowsInDateRange(String[,] data, DateTime startDate, DateTime endDate)
        {
            List<String[]> result = new List<string[]>();
            var numRows = data.GetLength(0);
            for(int i = 2; i < numRows; i++)
            {
                if(data[i, 8] != null && data[i, 8] != "" && Convert.ToDateTime(data[i, 8]) >= startDate && Convert.ToDateTime(data[i, 8]) <= endDate)
                {
                    String[] temp = new String[data.GetLength(1)];
                    for(int j = 0; j < temp.Length; j++)
                    {
                        temp[j] = data[i, j];
                    }
                    result.Add(temp);
                }
            }
            return result;
        }

        [HttpGet]
        [Route("Import")]
        public String[,] Import(FileInfo filePath)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage(filePath))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["DeploymentRequests"];
                    int RowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    String[,] data = new String[RowCount, ColCount];

                    for (int row = 1; row <= RowCount; row++)
                    {
                        for (int col = 1; col <= ColCount; col++)
                        {
                            if (worksheet.Cells[row, col].Text != null)
                            {
                                data[row-1, col-1] = worksheet.Cells[row, col].Text.ToString();
                            }
                            else
                            {
                                data[row-1, col-1] = "";
                            }
                        }
                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error occured while importing." + ex.Message);
                return new String[0,0];
            }
        }

        public IActionResult SendMail(string name, string email, string msg)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("brian.hanus@outlook.com"));
            message.To.Add(new MailboxAddress("brian.hanus@ecolab.com"));
            message.Subject = name;
            message.Body = new TextPart("html")
            {
                Text = "From: " + name + "<br>" +
                "Contact Information: " + email + "<br>" +
                "Message: " + msg
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp-mail.outlook.com", 587);
                client.Authenticate("brian.hanus@outlook.com", "Bh167471");
                client.Send(message);
                client.Disconnect(false);
            }

            return View("Index");
        }
    }
}
