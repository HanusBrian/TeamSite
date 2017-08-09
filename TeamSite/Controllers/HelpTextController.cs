using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using TeamSite.Models;
using System.IO;
using HtmlAgilityPack;
using System.Text;
using System.IO.Compression;


namespace TeamSite.Controllers
{
    public class HelpTextController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;
        public HelpTextController(IHostingEnvironment host, ILogger<HelpTextController> logger)
        {
            _hostingEnvironment = host;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public FileResult DocToFiles(List<IFormFile> files)
        {
            try
            {
                FileSystem fileSystem = new FileSystem(_hostingEnvironment, _logger);
                FileInfo fileInfo = fileSystem.LoadFile(files);

                Encoding wind1252 = Encoding.GetEncoding(1252);
                Encoding utf8 = Encoding.UTF8;
                byte[] wind1252Bytes = ReadFile(fileInfo.FullName);
                byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                string utf8String = Encoding.UTF8.GetString(utf8Bytes);

                var HtmlDoc = new HtmlDocument();
                HtmlDoc.LoadHtml(utf8String);

                var htmlHead = HtmlDoc.DocumentNode.SelectSingleNode("//head");
                var htmlTables = HtmlDoc.DocumentNode.SelectNodes("//table");

                htmlTables = cleanNestedTables(htmlTables);

                List<string> splitText = SplitFiles(htmlHead, htmlTables);
                SaveToFiles(splitText);

                ZipFile.CreateFromDirectory(_hostingEnvironment.WebRootPath + "/filesystem/tempHelpText/", 
                    _hostingEnvironment.WebRootPath + "/filesystem/zipFiles/HelpText.zip");

                FileInfo ZipFileInfo = new FileInfo(_hostingEnvironment.WebRootPath + "/filesystem/zipFiles/HelpText.zip");

                return UploadFileToUser(ZipFileInfo);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error in DocsToFile: " + e.Message);
            }
            finally
            {
                CleanTempFiles(_hostingEnvironment.WebRootPath + "/filesystem/tempHelpText/");
                CleanTempFiles(_hostingEnvironment.WebRootPath + "/filesystem/zipFiles/");
            }
            return null;
        }

        private FileResult UploadFileToUser(FileInfo fileInfo)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileInfo.FullName);
            string fileName = fileInfo.Name;
            var mimeType = fileBytes.GetType();
            return File(fileBytes, "application/zip", fileName);
        }

        private void CleanTempFiles(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private HtmlNodeCollection cleanNestedTables(HtmlNodeCollection html)
        {
            HtmlNodeCollection result = new HtmlNodeCollection(html.FirstOrDefault());
            int skip = 0; // Set to 1 because parent node already assigned to first node in collection
            foreach (var file in html)
            {
                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                result.Add(file);

                var HtmlDoc = new HtmlDocument();
                HtmlDoc.LoadHtml(file.OuterHtml);

                int numTablesFound = HtmlDoc.DocumentNode.SelectNodes("//table").Count();
                
                if(numTablesFound > 1)
                {
                    skip = numTablesFound - 1;
                }
            }
            return result;
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length    
                buffer = new byte[length];            // create buffer     
                int count;                            // actual number of bytes read     
                int sum = 0;                          // total number of bytes read    

                // read until Read method returns 0 (end of the stream has been reached)    
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                //fileStream.Close();
            }
            return buffer;
        }

        private List<string> SplitFiles(HtmlNode head, HtmlNodeCollection tables)
        {
            List<string> result = new List<string>();
            foreach(var table in tables)
            {
                result.Add(head.OuterHtml + table.OuterHtml);
            }
            return result;
        }

        private void SaveToFiles(List<string> splitText)
        {
            foreach (var helpText in splitText)
            {
                try
                {
                    var HtmlDoc = new HtmlDocument();
                    HtmlDoc.LoadHtml(helpText);

                    var html = HtmlDoc.DocumentNode.SelectSingleNode("//table/tr/td/p/b");
                    int index = html.InnerText.IndexOf("&nbsp;") != -1 ? html.InnerText.IndexOf("&nbsp;") : html.InnerText.IndexOf(" ");
                    string fileName = html.InnerText.Substring(0, index);

                    using (FileStream fs = new FileStream(_hostingEnvironment.WebRootPath + "/filesystem/tempHelpText/" + fileName + ".htm", FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine(helpText);
                        }
                    }
                }
                catch(Exception e)
                {
                    _logger.LogCritical("Error in SaveToFiles: " + e.Message);
                }
            }
        }
    }
}
