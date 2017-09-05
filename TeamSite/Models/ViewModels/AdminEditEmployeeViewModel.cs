using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models.ViewModels
{
    public class AdminEditEmployeeViewModel
    {
        public AppUser AppUser { get; set; }
        public IEnumerable<Tab> Tabs { get; set; }
    }
}
