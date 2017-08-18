using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamSite.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public int ExcelIndex { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        [ForeignKey("Program")]
        public int ProgramId { get; set; }

        public int AccountManager { get; set; }
        public int OpsSpecialist { get; set; }

        public DateTime LaunchDate { get; set; }

        [Required]
        public bool Complete { get; set; }

        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
