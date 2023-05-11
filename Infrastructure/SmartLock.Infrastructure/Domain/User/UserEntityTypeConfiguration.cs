using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SmartLock.Infrastructure.Consts;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Infrastructure.Domain.User {
    internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<Entities.User> {
        public void Configure(EntityTypeBuilder<Entities.User> builder) {
            builder.ToTable("Users", DataBaseSchema.DataSchema);

            builder.HasKey(t => t.Id);

            builder.Property(p => p.Id)
                   .HasColumnType("Bigint");

            builder.Property(p => p.UserName)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(p => p.Password)
                   .HasMaxLength(128)
                   .IsRequired();

            builder.HasMany(e => e.AccessedLocks)
                  .WithMany(e => e.AccessedUsers)
                  .UsingEntity("UserLock");

            builder.Property(p => p.RefreshToken)
                   .HasMaxLength(128)
                   .HasColumnName("RefreshToken");

            builder.Property(p => p.RefreshTokenExpireDate)
                   .HasColumnName("RefreshTokenExpireDate");

            builder.Property(p => p.CreateDate)
                   .IsRequired();
        }
    }
}
