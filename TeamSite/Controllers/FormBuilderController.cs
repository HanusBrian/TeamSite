using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TeamSite.Infrastructure;
using TeamSite.Models;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace TeamSite.Controllers
{
    [Authorize]
    public class FormBuilderController : Controller
    {

        private readonly IHostingEnvironment _he;
        private readonly ILogger<FileSystem> _fslog;
        private readonly ILogger<FileSystem> _excelLog;
        private readonly FileSystem _fs;

        public FormBuilderController(IHostingEnvironment _hostingEnvironment, ILogger<FileSystem> fslog, FileSystem fs, IHostingEnvironment he)
        {

            _fslog = fslog;
            _fs = _fs;
            _he = he;
        }

        const int SEQNUM = 2;
        const int CATID = 3;
        const int CAT = 4;
        const int QID = 5;
        const int UID = 6;
        const int QTEXT = 7;
        const int PLISTTEXT = 8;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadTemplate
                (List<IFormFile> files,
                int seqcol,
                int catidcol,
                int catnamecol,
                int qid,
                int qtext,
                int plist,
                int thold,
                int acrit)
        {

            FileSystem fs = new FileSystem(_he, _fslog);
            fs.LoadFile(files);

            string sWebRootFolder = _he.WebRootPath + "/filesystem/";
            string sFileName = files[0].FileName;
            FileInfo fileInfo = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

            string masterRootFolder = _he.WebRootPath + "/filesystem/res/";
            FileInfo masterFileInfo = new FileInfo(Path.Combine(sWebRootFolder + "res/", "MasterForm.xlsx"));

            //FileSystem fs = new FileSystem(_he, _excelLog);
            // Set up helptext file
            FileInfo helpTextInfo = fs.NewFile("HelpTextMaster.htm");
            using (System.IO.File.Create(helpTextInfo.FullName)){ };
            StartHelptextFile(helpTextInfo);

            // Create excel packages
            Excel inputExcel = new Excel(fileInfo, _excelLog, _he);
            Excel masterExcel = new Excel(masterFileInfo, _excelLog, _he);

            try
            {
                // set active excel worksheets
                inputExcel.SetActiveWorkSheet("Sheet1");
                masterExcel.SetActiveWorkSheet("Template");

                int masterNumRows = masterExcel._ws.Dimension.Rows;
                int masterNumCols = masterExcel._ws.Dimension.Columns;

                int inputNumRows = inputExcel._ws.Dimension.Rows;
                int inputNumCols = inputExcel._ws.Dimension.Columns;

                for (int i = 1; i < inputNumRows; i++)
                {

                    // Clean the picklists
                    string picklistCell = CleanPicklistCell(inputExcel._ws.Cells[i + 1, plist].Value);

                    // Add clean picklist and question info to the helptext
                    AddLineToHelpText(helpTextInfo, inputExcel._ws.Cells[i + 1, qid].Value.ToString(),
                                        inputExcel._ws.Cells[i + 1, qtext].Value.ToString(),
                                        inputExcel._ws.Cells[i + 1, thold].Value.ToString(),
                                        inputExcel._ws.Cells[i + 1, acrit].Value.ToString(),
                                        picklistCell);

                    // Edit picklist text with PL ID tag
                    picklistCell = AddTagsToPicklistCells(picklistCell, inputExcel._ws.Cells[i + 1, qid].Value);

                    masterExcel._ws.Cells[masterNumRows + i, SEQNUM].Value = inputExcel._ws.Cells[i + 1, seqcol].Value;
                    masterExcel._ws.Cells[masterNumRows + i, CATID].Value = inputExcel._ws.Cells[i + 1, catidcol].Value;
                    masterExcel._ws.Cells[masterNumRows + i, CAT].Value = inputExcel._ws.Cells[i + 1, catnamecol].Value;
                    masterExcel._ws.Cells[masterNumRows + i, QID].Value = inputExcel._ws.Cells[i + 1, qid].Value;
                    masterExcel._ws.Cells[masterNumRows + i, UID].Value = inputExcel._ws.Cells[i + 1, qid].Value;
                    masterExcel._ws.Cells[masterNumRows + i, QTEXT].Value = inputExcel._ws.Cells[i + 1, qtext].Value;
                    masterExcel._ws.Cells[masterNumRows + i, PLISTTEXT].Value = picklistCell;

                    masterExcel.SaveAs("test");
                }
                EndHelptextFile(helpTextInfo);
            }
            catch (Exception ex)
            {
                _excelLog.LogError("Some error occured in UploadFiles." + ex.Message + " " + ex.StackTrace);
                return RedirectToAction("Import");
            }
            finally
            {
                FileSystem.CleanTempFile(fileInfo.ToString());
            }

            return View();
        }

        private string CleanPicklistCell(object text)
        {
            string value = text.ToString();

            value = value.Replace("_x000D_", "");
            value = value.Replace("\r", "");
            value = value.Replace("\n.\n", "\n");
            value = value.Replace("\n.", "\n");
            value = value.Replace("\n\n", "\n");
            while (value[value.Length - 1] == '\n')
                value = value.Substring(0, value.Length - 1);

            return value;
        }

        private string AddTagsToPicklistCells(string value, object qid)
        {
            // at any instance of \n or end of string add [[PL1.2.3a]] tag
            char c = 'a';
            int i = 0;
            int t;
            while ((t = value.IndexOf("\n", i)) > -1)
            {
                String ins = "[[PL" + qid + c + "]]";
                value = value.Insert(t, ins);
                c++;
                i = value.IndexOf(ins) + ins.Length + 1;
            }
            value = value + "[[PL" + qid + c + "]]";

            return value;
        }

        private void EndHelptextFile(FileInfo helpTextInfo)
        {
            using (StreamWriter file = new StreamWriter(System.IO.File.Open(helpTextInfo.FullName, FileMode.Append)))
            {
                string html = "<p class=\"MsoNormal\">&nbsp;</p></div></body></html>";

                file.Write(html);
            }
        }

        private void StartHelptextFile(FileInfo helpTextInfo)
        {
            //System.IO.File.Create(helpTextInfo.FullName)
            using (StreamWriter file = new StreamWriter(System.IO.File.Open(helpTextInfo.FullName, FileMode.Open)))
            {
                string html = "<html><head><meta http-equiv = \"Content-Type\" content = \"text/html; charset=utf-8\"><meta name = \"Generator\" content = \"Microsoft Word 14 (filtered)\"><style> < !-- /* Font Definitions */ @font-face{font-family:Calibri; panose-1:2 15 5 2 2 2 4 3 2 4;}/* Style Definitions */ p.MsoNormal, li.MsoNormal, div.MsoNormal{margin-top:0in; margin-right:0in; margin-bottom:10.0pt; margin-left:0in; line-height:115 %; font-size:11.0pt; font-family:\"Arial\",\"sans-serif\";}p.MsoListParagraph, li.MsoListParagraph, div.MsoListParagraph{margin-top:0in; margin-right:0in; margin-bottom:0in; margin-left:.5in; margin-bottom:.0001pt; font-size:12.0pt; font-family:\"Times New Roman\",\"Arial\";}span.MsoIntenseEmphasis{mso-style-name:\"Intense Emphasis, Ecosure_colorHeader\"; font-family:\"Arial\",\"sans-serif\"; color: white; font-weight:bold; font-style:normal;}span.EcoBoldChar{mso-style-name:\"Eco_Bold Char\"; mso-style-link:Eco_Bold; font-family:\"Arial\",\"sans-serif\"; font-weight:bold;}p.EcoBold, li.EcoBold, div.EcoBold{mso-style-name:Eco_Bold; mso-style-link:\"Eco_Bold Char\"; margin-top:2.0pt; margin-right:0in; margin-bottom:2.0pt; margin-left:0in; font-size:11.0pt; font-family:\"Arial\",\"sans-serif\"; font-weight:bold;}span.EcoSectionHeadingChar{mso-style-name:\"Eco_SectionHeading Char\"; mso-style-link:Eco_SectionHeading; font-family:\"Arial\",\"sans-serif\"; color:#4D4F53; font-weight:bold;}p.EcoSectionHeading, li.EcoSectionHeading, div.EcoSectionHeading{mso-style-name:Eco_SectionHeading; mso-style-link:\"Eco_SectionHeading Char\"; margin-top:0in; margin-right:0in; margin-bottom:10.0pt; margin-left:0in; line-height:115 %; font-size:11.0pt; font-family:\"Arial\",\"sans-serif\"; color:#4D4F53;font-weight:bold;}.MsoChpDefault{font-family:\"Calibri\",\"sans-serif\";}.MsoPapDefault{margin-bottom:10.0pt; line-height:115 %;}@page WordSection1{size: 8.5in 11.0in; margin: 1.0in 1.0in 1.0in 1.0in;}div.WordSection1{page: WordSection1;}--> </style> </head> <body lang=\"EN-US\"> <div class=\"WordSection1\"><p class=\"EcoSectionHeading\"><span style=\"font-size:18.0pt; line-height:115 % \"> &nbsp;</span></p>";
                file.Write(html);
            }
        }

        private void AddLineToHelpText(FileInfo helpTextInfo, string qid, string qtext, string thresh, string standard, string picklists)
        {
            standard = standard.Replace("\n", "<br>");
            using (StreamWriter file = new StreamWriter(System.IO.File.Open(helpTextInfo.FullName, FileMode.Append)))
            {
                string html = "<table class=\"MsoNormalTable\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"border-collapse:collapse\"><tbody><tr style = \"height:33.25pt\"><td width=\"798\" valign=\"top\" style=\"width:6.65in;border:solid windowtext 1.0pt;background:#EEAF00;padding:0in 5.4pt 0in 5.4pt;height:33.25pt\"><p class=\"MsoNormal\" style=\"text-align:justify\"><b>" + qid;

                html += "</b><span style = \"color:green\" > &nbsp;</span><span class=\"MsoIntenseEmphasis\">" + qtext;

                html += "</span></b></p></td></tr><tr><td width = \"798\" valign=\"top\" style=\"width:6.65in;border:solid windowtext 1.0pt;  border-top:none; padding:0in 5.4pt 0in 5.4pt\"><p class=\"EcoBold\">Threshold:&nbsp; <span style =\"font-weight:normal\" > " + thresh;

                html += "</span>&nbsp;&nbsp;&nbsp;</p></td></tr><tr><td width=\"798\" valign=\"top\" style=\"width:6.65in;border:solid windowtext 1.0pt; border-top:none;padding:0in 5.4pt 0in 5.4pt\"><p class=\"EcoBold\">Standard:</p><p class=\"MsoNormal\" style=\"text-align:left\"> " + standard;

                html += "</p></td></tr><tr><td width = \"798\" valign=\"top\" style=\"width:6.65in;border:solid windowtext 1.0pt; border-top:none;padding:0in 5.4pt 0in 5.4pt\"><p class=\"EcoBold\">Picklists:</p>";

                string[] splitPicklists = picklists.Split('\n');
                for (int i = 0; i < splitPicklists.Length; i++)
                {
                    html += "<p class=\"MsoListParagraph\" style=\"text-indent:-.25in\"><span style =\"font-size: 11.0pt; font-family:Symbol\" >·</span><span style = \"font-size:7.0pt\" > &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</span><span style=\"font-size:11.0pt; font-family:Arial\"> ";
                    html += splitPicklists[i];
                    html += "</span></p>";
                }

                html += "</td></tr></tbody></table><p class=\"EcoSectionHeading\">&nbsp;<span style =\"font-size:9.0pt;line-height:115%\" > &nbsp;</span></p><p class=\"MsoNormal\" style=\"text-align:justify\"><span style=\"font-size:12.0pt;line-height:115%;font-family:&quot;Times New Roman&quot;,&quot;serif&quot;;color:black\">&nbsp;</span></p>";
                file.Write(html);
            }
        }
    }
}