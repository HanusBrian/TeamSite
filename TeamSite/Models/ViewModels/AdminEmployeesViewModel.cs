using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models.ViewModels
{
    public class AdminEmployeesViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
    }
}
