using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Core.Data.Entities.EntityConfigurations
{
    public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.CustomerId).IsRequired().HasMaxLength(255); 
            builder.Property(c => c.Name).IsRequired().HasMaxLength(255);
            builder.Property(c => c.ContactInformation).IsRequired().HasMaxLength(255);

            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);
        }
    }

}
