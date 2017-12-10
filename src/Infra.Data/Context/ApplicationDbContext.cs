using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CameraVillage.Domain.Models;
using CameraVillage.Infra.Data.Configurations;

namespace CameraVillage.Infra.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CatalogBrandConfiguration());
            builder.ApplyConfiguration(new CatalogTypeConfiguration());
            builder.ApplyConfiguration(new CatalogItemConfiguration());
        }
    }
}
