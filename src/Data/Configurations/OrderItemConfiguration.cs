using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RolleiShop.Models.Entities;

namespace RolleiShop.Data.Configurations
{
    class OrderItemConfiguration
        : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.OwnsOne(i => i.ItemOrdered);
        }
    }
}
