using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;


namespace TeamSite.Models
{
    public class FileSystem
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;

        public FileSystem(IHostingEnvironment hostingEnvironment, ILogger<FileSystem> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public FileInfo NewFile(string name)
        {
            string rootFolder = _hostingEnvironment.WebRootPath + "/filesystem/res/";
            FileInfo fInfo = new FileInfo(Path.Combine(rootFolder, name));

            return fInfo;
        }

        // Load the file into the server file system
        public FileInfo LoadFile(List<IFormFile> files)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath + "/filesystem/";
            string sFileName = files[0].FileName;
            FileInfo fileInfo = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

            LoadFilesToFS(files);

            return fileInfo;
        }

        public int LoadFilesToFS(List<IFormFile> files)
        {
            long size = 0;
            int countFiles = 0;
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                filename = _hostingEnvironment.WebRootPath + "/filesystem/" + file.FileName;
                size += file.Length;
                if (size > 0) countFiles++;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            return countFiles;
        }

        public void LoadFilesToFS(IFormFile file)
        {
            long size = 0;
            int countFiles = 0;
            var filename = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName
                            .Trim('"');
            filename = _hostingEnvironment.WebRootPath + "/filesystem/" + file.FileName;
            size += file.Length;
            if (size > 0) countFiles++;
            using (FileStream fs = System.IO.File.Create(filename))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        public static void CleanTempFolders(string path)
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

        public static void CleanTempFile(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            FileInfo file = new FileInfo(path);
            file.Delete();
        }
    }
}
