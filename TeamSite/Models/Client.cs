using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public string Name { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public int? RevenueRank { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
