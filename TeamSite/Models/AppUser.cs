using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace TeamSite.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
