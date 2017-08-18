using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamSite.Models
{
    public class Program
    {
        [Key]
        public int ProgramId { get; set; }
        public string Name { get; set; }
        [Required]
        [ForeignKey("Client")]
        public Client ClientId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime? ChangeDate { get; set; }
    }
}
