using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AviationSalon.Core.Data.Entities.EntityConfigurations
{

    public class WeaponEntityConfiguration : IEntityTypeConfiguration<WeaponEntity>
    {
        public void Configure(EntityTypeBuilder<WeaponEntity> builder)
        {
            builder.HasKey(w => w.WeaponId);

            builder.Property(w => w.Name).IsRequired().HasMaxLength(255);
            builder.Property(w => w.Type).IsRequired();
            builder.Property(w => w.GuidedSystem).IsRequired();
            builder.Property(w => w.Range).IsRequired();
            builder.Property(w => w.FirePower).IsRequired();

            builder.HasOne(w => w.Aircraft)
                .WithMany(a => a.Weapons)
                .HasForeignKey(w => w.AircraftId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
