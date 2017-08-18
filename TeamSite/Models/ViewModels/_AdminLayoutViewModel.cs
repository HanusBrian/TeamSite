using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models.ViewModels
{
    public class _AdminLayoutViewModel
    {
        public string Title { get; set; }
        public IEnumerable<Tab> Tabs { get; set; }
    }
}
