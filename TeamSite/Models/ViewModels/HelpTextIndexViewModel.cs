using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models.ViewModels
{
    public class HelpTextIndexViewModel
    {
        public IEnumerable<string> SaveErrors { get; set; }
        public FileInfo ZipFileInfo { get; set; }
    }
}
