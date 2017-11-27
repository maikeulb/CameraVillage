using Microsoft.EntityFrameworkCore;

namespace CameraVillage.Infra.Identity
{
    public interface IIdentityDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }
    }
}
