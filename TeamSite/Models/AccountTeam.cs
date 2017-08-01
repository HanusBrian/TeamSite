using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models
{
    public class AccountTeam
    {
        public int AccountTeamID { get; set; }
        public string TeamName { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }
}
