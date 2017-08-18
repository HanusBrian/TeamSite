using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TeamSite.Models.ViewModels
{
    public class AdminRoleIndexViewModel : _SubNavViewModel
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
