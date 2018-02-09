using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RolleiShop.Models.Entities;
using RolleiShop.Data.Configurations;

namespace RolleiShop.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BasketConfiguration());
            builder.ApplyConfiguration(new CatalogBrandConfiguration());
            builder.ApplyConfiguration(new CatalogTypeConfiguration());
            builder.ApplyConfiguration(new CatalogItemConfiguration());
            builder.ApplyConfiguration(new OrderTypeConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());
        }
    }
}
