using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
         


            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "ABKWebsite",
                    Email = "ABKWebsite@abkegypt.com",
                    UserName = "ABKWebsite@abkegypt.com",

                };

                var admin = new AppUser
                {
                    DisplayName = "ABKWebsiteAdmin",
                    Email = "ABKWebsiteAdmin@abkegypt.com",
                    UserName = "ABKWebsiteAdmin@abkegypt.com",

                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "User");

                await userManager.CreateAsync(admin, "Pa$$w0rdAdmin@2022");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }


        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var adminrole = new IdentityRole("Admin");
                var userrole = new IdentityRole("User");

                await roleManager.CreateAsync(adminrole);
                await roleManager.CreateAsync(userrole);
            }
        }
    }
}