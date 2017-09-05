﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using TeamSite.Models;
using System.IO;
using HtmlAgilityPack;
using System.Text;
using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using TeamSite.Infrastructure;
using TeamSite.Models.ViewModels;

namespace TeamSite.Controllers
{
    [Authorize]
    public class HelpTextController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;
        private readonly FileSystem _fs;
        private readonly ILogger<FileSystem> _fsLogger;
        public HelpTextController(IHostingEnvironment host, ILogger<HelpTextController> logger, FileSystem fs, ILogger<FileSystem> fsLogger)
        {
            _hostingEnvironment = host;
            _logger = logger;
            _fs = fs;
            _fsLogger = fsLogger;
        }

        public IActionResult Index(List<string> errorList) => View(errorList);

        [HttpPost]
        public IActionResult DocToZip(List<IFormFile> files)
        {
            FileSystem fileSystem = new FileSystem(_hostingEnvironment, _fsLogger);
            //load file into filesystem folder
            FileInfo fileInfo = fileSystem.LoadFile(files);
            try
            {
                //Convert file text to utf8 from windows 1252 encoding
                byte[] win1252Bytes = ReadFile(fileInfo.FullName);
                byte[] utf8Bytes = Utils.Win1252ToUtf8(win1252Bytes);
                string utf8String = Utils.byteArrToString(utf8Bytes);

                //Load string into htmlparser
                var HtmlDoc = new HtmlDocument();
                HtmlDoc.LoadHtml(utf8String);

                //extract head tag and all table tags
                //remove any nested tables from having their own file created
                var htmlHead = HtmlDoc.DocumentNode.SelectSingleNode("//head");
                var htmlTables = HtmlDoc.DocumentNode.SelectNodes("//table");
                htmlTables = cleanNestedTables(htmlTables);

                //adds head tag to each table
                //saves to individual files to filesystem/tempHelpText folder
                List<string> splitText = CombineHeadWithTables(htmlHead, htmlTables);
                List<string> saveErrors = SaveToFiles(splitText);

                //zipps filesystem/tempHelpText folder to /filesystem/zipFiles/HelpText.zip
                ZipFile.CreateFromDirectory(_hostingEnvironment.WebRootPath + "/filesystem/tempHelpText/", 
                    _hostingEnvironment.WebRootPath + "/filesystem/zipFiles/HelpText.zip");

                FileInfo ZipFileInfo = new FileInfo(_hostingEnvironment.WebRootPath + "/filesystem/zipFiles/HelpText.zip");

                UploadFileToUser(ZipFileInfo);

                return UploadFileToUser(ZipFileInfo);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error in DocsToFile: " + e.Message);
            }
            finally
            {
                //clean all created files from filesystem
                FileSystem.CleanTempFolders(_hostingEnvironment.WebRootPath + "/filesystem/tempHelpText/");
                FileSystem.CleanTempFolders(_hostingEnvironment.WebRootPath + "/filesystem/zipFiles/");
                FileSystem.CleanTempFile(_hostingEnvironment.WebRootPath + "/filesystem/" + fileInfo.Name);
            }
            return null;
        }

        private FileResult UploadFileToUser(FileInfo fileInfo)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileInfo.FullName);
            string fileName = fileInfo.Name;
            var mimeType = fileBytes.GetType();
            Response.Headers.Append("Content-Disposition", "inline; filename=" + fileName);
            return File(fileBytes, "application/zip", fileName);
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
                fileStream.Dispose();
            }
            return buffer;
        }

        private List<string> CombineHeadWithTables(HtmlNode head, HtmlNodeCollection tables)
        {
            List<string> result = new List<string>();
            foreach(var table in tables)
            {
                result.Add(head.OuterHtml + table.OuterHtml);
            }
            return result;
        }

        private List<string> SaveToFiles(List<string> splitText)
        {
            string fileName = "";
            List<string> errorLog = new List<string>();
            foreach (var helpText in splitText)
            {
                try
                {
                    var HtmlDoc = new HtmlDocument();
                    HtmlDoc.LoadHtml(helpText);

                    var html = HtmlDoc.DocumentNode.SelectSingleNode("//table/tr/td/p/b");
                    int index;
                    if (html.InnerText.IndexOf("&nbsp;") != -1)
                    {
                        index = html.InnerText.IndexOf("&nbsp;");
                    }
                    else if (html.InnerText.IndexOf(" ") != -1)
                    {
                        index = html.InnerText.IndexOf(" ");
                    }
                    else
                    {
                        index = -1;
                    }

                    if (index != -1)
                        fileName = html.InnerText.Substring(0, index);
                    else
                        fileName = html.InnerText;

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
                    //if file not created add reason to log
                    errorLog.Add("Error Saving file: " + fileName + "\n" +
                        "Error message:  " + e.Message);
                }
            }
            return errorLog;
        }
    }
}
