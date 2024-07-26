using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence.EntityConfiguration
{
    public class ShopProductConfiguration : IEntityTypeConfiguration<ShopProduct>
    {
        public void Configure(EntityTypeBuilder<ShopProduct> builder)
        {
            builder.HasKey(sp => new { sp.ShopId, sp.ProductId });
        }
    }
}
