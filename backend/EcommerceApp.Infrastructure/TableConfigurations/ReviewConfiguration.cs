

using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApp.Infrastructure.TableConfigurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(e => e.Comment)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasOne(e => e.OrderLine).WithMany(e => e.Reviews)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
