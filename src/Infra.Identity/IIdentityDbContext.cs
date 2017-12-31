using Microsoft.EntityFrameworkCore;

namespace RolleiShop.Infra.Identity
{
    public interface IIdentityDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }
    }
}
