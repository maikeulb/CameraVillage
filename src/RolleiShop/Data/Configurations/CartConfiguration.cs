using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RolleiShop.Entities;

namespace RolleiShop.Data.Configurations
{
    class CartConfiguration
        : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Cart.Items));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
