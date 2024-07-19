

using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApp.Infrastructure.TableConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);


        }
    }
}
