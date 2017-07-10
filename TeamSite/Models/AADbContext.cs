using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TeamSite.Models
{
    public class AADbContext : DbContext
    {
        public AADbContext(DbContextOptions<AADbContext> options) : base(options)
        { }

        public DbSet<Contact> Contacts { get; set; }
    }
}
