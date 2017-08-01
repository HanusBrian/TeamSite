using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public Employee Manager { get; set; }
    }
}
