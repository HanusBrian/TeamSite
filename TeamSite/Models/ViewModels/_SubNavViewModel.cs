using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models.ViewModels
{
    public class _SubNavViewModel
    {
        public string Title { get; internal set; }
        public IEnumerable<Tab> Tabs { get; set; }
    }
}
