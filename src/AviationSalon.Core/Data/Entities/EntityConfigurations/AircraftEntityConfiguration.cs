using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AviationSalon.Core.Data.Entities.EntityConfigurations
{
    public class AircraftEntityConfiguration : IEntityTypeConfiguration<AircraftEntity>
    {
        public void Configure(EntityTypeBuilder<AircraftEntity> builder)
        {
            builder.HasKey(a => a.AircraftId);

            builder.Property(c => c.AircraftId).IsRequired().HasMaxLength(255);
            builder.Property(a => a.Model).IsRequired().HasMaxLength(255);
            builder.Property(a => a.ImageFileName).IsRequired().HasMaxLength(255);
            builder.Property(a => a.Range).IsRequired();
            builder.Property(a => a.MaxHeight).IsRequired(); 
            builder.Property(a => a.Role).IsRequired();
            builder.Property(a => a.MaxWeaponsCapacity).IsRequired().HasDefaultValue(0);

            builder.HasMany(a => a.Weapons)
                .WithOne(w => w.Aircraft)
                .HasForeignKey(w => w.AircraftId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
