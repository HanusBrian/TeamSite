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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using Microsoft.Net.Http.Headers;

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
        public async Task<IActionResult> UploadFiles(List<IFormFile> files, DateTime startDate, DateTime endDate)
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
            try
            {
                string sWebRootFolder = _hostingEnvironment.WebRootPath;
                string sFileName = files[0].FileName;
                FileInfo filePath = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

                String[,] data = Import(filePath);

                // Find TargetDate (Column 9 "I") that is between the chosen startDate and endDate
                List<String[]> result = FindRowsInDateRange(data, startDate, endDate);

                await GenerateEmails(result);

                String resultString = ListToString(result);

                return View("UploadFiles", result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some error occured in UploadFiles." + ex.Message);
                return View("UploadFiles", null);
            }
        }

        public async Task GenerateEmails(List<String[]> excelTable)
        {
            foreach(var row in excelTable)
            {
                //Get Email To address by concatenating first and last name with @ecolab.com
                String emailTo = row[4];

                //Get email from, determined by Initials
                String emailFrom = row[7];

                String programName = row[2];

                bool isNewLaunch = (row[5] == "New Launch"? true : false);

                DateTime launchDate = Convert.ToDateTime(row[8]);

                await SendMail(emailTo, programName, emailFrom, launchDate);
            }
        }

        public String ListToString(List<String[]> result)
        {
            String output = "<table>";
    
            foreach(var row in result)
            {
                output += "<tr>";
                for(int j = 0; j < row.GetLength(0); j++)
                {
                    output += "<td>" + row[j] + "</td>";
                }
                output += "</tr>";
            }

            output += "</table>";

            return output;
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

        public Task<int> SendMail(string emailTo, string programName, string emailFrom, DateTime launchDate)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("brian.hanus@outlook.com"));
                message.To.Add(new MailboxAddress("brian.hanus@ecolab.com"));
                message.Subject = programName + " - MileStone";
                message.Body = new TextPart("html")
                {
                    Text = "From: " + emailFrom + "<br>" +
                    "To: " + emailTo + "<br>" +
                    "Team - <br>" +
                    "Please review the dates below for the " +
                    programName +
                    " " +
                    launchDate +
                    " launch.<br><br>" +
                    "<html><head><meta http - equiv=\"Content - Type\" content=\"text / html; charset = windows - 1252\"></head><body lang=\"EN - US\" ><div class=\"WordSection1\"><table class=\"MsoTableGrid\" border=\"1\" cellspacing=\"0\" cellpadding=\"0\" style=\"border - collapse:collapse; border: none\"> <tbody><tr> <td width=\"779\" colspan=\"4\" valign=\"top\" style=\"width: 467.5pt; border: solid windowtext 1.0pt; background: black; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Bucket 1: Items Due Wednesday, June 7th, 2017</b></p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Task</b></p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Deliver To</b></p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Owner</b></p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Date Delivered</b></p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Final form changes</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Training</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Official name of form</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">CAM due dates</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Score / Rating logic</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"779\" colspan=\"4\" valign=\"top\" style=\"width: 467.5pt; border: solid windowtext 1.0pt; border - top:none; background: black; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Bucket 2: Items Due Wednesday, June 14th, 2017</b></p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Task</b></p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Deliver To</b></p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Owner</b></p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Date Delivered</b></p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Create Client / Project / Reference Numbers </p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">-</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Store / Hierarchy information</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Nick Chon</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Inform Nick if MSBI setup needed</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Nick Chon</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Email report info – subject, body, conditions</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">CAM reminder info – subject, body, conditions</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Other email rules / alerts needed</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"779\" colspan=\"4\" valign=\"top\" style=\"width: 467.5pt; border: solid windowtext 1.0pt; border - top:none; background: black; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Bucket 3: Items Due Wednesday, June 21st, 2017</b></p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Task</b></p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Deliver To</b></p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Owner</b></p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Date Delivered</b></p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Deliver final form content</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Training</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Final base report listing</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"779\" colspan=\"4\" valign=\"top\" style=\"width: 467.5pt; border: solid windowtext 1.0pt; border - top:none; background: black; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Bucket 4: Items Due Wednesday, June 28th, 2017</b></p></td></tr><tr> <td width=\"779\" colspan=\"4\" valign=\"top\" style=\"width: 467.5pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b><i>Testing Begins (Form changes are finalized; any new form content updates may affect the launch date of the form)</i></b></p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Task</b></p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Deliver To</b></p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Owner</b></p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\"><b>Date Delivered</b></p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Violation Unit Report (view and sign off or ask for changes; validate correct score appears)</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">CAM Unit Report if in scope (view and sign off or ask for changes; validate correct score appears)</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Base Reports (view/run all base reports, validate expected data returns and correct columns display in Evaluation Results)</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Nick Chon</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">User Testing (log in as a user from every level of the hierarchy and verify access/ability to run reports, view unit reports, enter CAM, validate correct scores appear)</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr><tr> <td width=\"352\" valign=\"top\" style=\"width: 211.25pt; border: solid windowtext 1.0pt; border - top:none; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Notify AT to uncheck any re-audit assignments generated in PROD if applicable</p></td><td width=\"118\" valign=\"top\" style=\"width: 70.9pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">Account Team</p></td><td width=\"148\" valign=\"top\" style=\"width: 88.85pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">A&amp;A</p></td><td width=\"161\" valign=\"top\" style=\"width: 96.5pt; border - top:none; border - left: none; border - bottom:solid windowtext 1.0pt; border - right:solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt\"> <p class=\"MsoNormal\" style=\"margin - bottom:0in; margin - bottom:.0001pt; line - height: normal\">&nbsp;</p></td></tr></tbody></table><p class=\"MsoNormal\">&nbsp;</p></div></body></html>"


                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp-mail.outlook.com", 587);
                    client.Authenticate("brian.hanus@outlook.com", "Bh167471");
                    client.Send(message);
                    client.Disconnect(false);
                }
            }
            catch(Exception e)
            {
                _logger.LogCritical("Email not sent: " + e.Message);
            }
            return null;
        }
    }
}
