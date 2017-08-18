using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models.ViewModels
{
    public class AdminEmployeesViewModel : _SubNavViewModel
    {
        public IEnumerable<AppUser> AppUsers { get; set; }
    }
}
