using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace RolleiShop.Identity
{
    public class IdentityDbInitializer
    {
        public static async Task Initialize(
            UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "user@example.com", Email = "user@example.com" };
            await userManager.CreateAsync(defaultUser, "P@ssw0rd!");
        }
    }
}
