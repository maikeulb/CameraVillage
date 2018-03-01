using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace RolleiShop.Identity
{
    public class IdentityDbInitializer
    {
        public static async Task Initialize(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            // Default User
            var userPassword = configuration.GetSection("AppSettings")["UserPassword"];
            var userEmail = configuration.GetSection("AppSettings")["UserEmail"];

            var user = new ApplicationUser 
            { 
                UserName = userEmail,
                Email = userEmail
            };

            var existUser = await userManager.FindByEmailAsync(userEmail);

            if (existUser == null)
                await userManager.CreateAsync(user, userPassword);

            // Admin User
            var adminPassword = configuration.GetSection("AppSettings")["AdminPassword"];
            var adminEmail = configuration.GetSection("AppSettings")["AdminEmail"];
            string adminRole = "Admin";
            IdentityResult adminRoleResult;

            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            var adminRoleExist = await roleManager.RoleExistsAsync(adminRole);
            if (!adminRoleExist)
                adminRoleResult = await roleManager.CreateAsync(new IdentityRole(adminRole));

            var existAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existAdmin == null)
            {
                var createAdmin = await userManager.CreateAsync(admin, adminPassword);
                if (createAdmin.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Demo Admin User
            var demoAdminPassword = configuration.GetSection("AppSettings")["DemoAdminPassword"];
            var demoAdminEmail = configuration.GetSection("AppSettings")["DemoAdminEmail"];
            string demoAdminRole = "DemoAdmin";
            IdentityResult demoAdminRoleResult;

            var demoAdmin = new ApplicationUser
            {
                UserName = demoAdminEmail,
                Email = demoAdminEmail
            };

            var demoAdminRoleExist = await roleManager.RoleExistsAsync(demoAdminRole);
            if (!demoAdminRoleExist)
                demoAdminRoleResult = await roleManager.CreateAsync(new IdentityRole(demoAdminRole));

            var existDemoAdmin = await userManager.FindByEmailAsync(demoAdminEmail);
            if (demoAdmin == null)
            {
                var createDemoAdmin = await userManager.CreateAsync(demoAdmin, demoAdminPassword);
                if (createDemoAdmin.Succeeded)
                    await userManager.AddToRoleAsync(demoAdmin, "DemoAdmin");
            }
        }
    }
}
