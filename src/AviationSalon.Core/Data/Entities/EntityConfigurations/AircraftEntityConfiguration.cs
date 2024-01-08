using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Core.Data.Entities.EntityConfigurations
{
    public class AircraftEntityConfiguration : IEntityTypeConfiguration<AircraftEntity>
    {
        public void Configure(EntityTypeBuilder<AircraftEntity> builder)
        {
            builder.HasKey(a => a.AircraftId);

            builder.Property(a => a.Model).IsRequired().HasMaxLength(255);
            builder.Property(a => a.Manufacturer).IsRequired().HasMaxLength(255);
            builder.Property(a => a.YearOfManufacture);
            builder.Property(a => a.MaxWeaponsCapacity);

            builder.HasMany(a => a.Weapons)
                .WithOne(w => w.Aircraft)
                .HasForeignKey(w => w.AircraftId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
