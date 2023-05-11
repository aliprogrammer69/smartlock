

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SmartLock.Infrastructure.Consts;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Infrastructure.Domain.Role {
    public sealed class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Entities.Role> {
        public void Configure(EntityTypeBuilder<Entities.Role> builder) {
            builder.ToTable("Roles", DataBaseSchema.SecuritySchema);

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Name)
                   .HasMaxLength(32)
                   .IsRequired();

            builder.Property(p => p.Key)
                   .HasMaxLength(32)
                   .IsRequired();

            builder.Property(p => p.CreateDate)
                   .IsRequired();


            builder.HasData(new List<Entities.Role>(2) {
                new(1,"Administrator","admin"),
                new(2,"User","user"),
                new(3,"System","system")
            });
        }
    }
}
