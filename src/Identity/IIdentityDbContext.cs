using Microsoft.EntityFrameworkCore;

namespace RolleiShop.Identity
{
    public interface IIdentityDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }
    }
}
