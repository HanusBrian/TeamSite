using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace TeamSite.Models
{
    public class TeamSiteDbContext : IdentityDbContext<AppUser>
    {
        public TeamSiteDbContext(DbContextOptions<TeamSiteDbContext> options) : base(options) { }

        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Program> Program { get; set; }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string phone = configuration["Data:AdminUser:Phone"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    //await roleManager.CreateAsync(new IdentityRole(role));
                    await roleManager.CreateAsync(new IdentityRole("Admins"));
                    await roleManager.CreateAsync(new IdentityRole("Account"));
                    await roleManager.CreateAsync(new IdentityRole("Deployment"));
                    await roleManager.CreateAsync(new IdentityRole("Training"));
                }
                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email,
                    PhoneNumber = phone
                };
                IdentityResult result = await userManager
                .CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
