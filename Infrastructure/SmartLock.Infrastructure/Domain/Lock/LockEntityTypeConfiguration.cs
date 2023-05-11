using Microsoft.EntityFrameworkCore;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Infrastructure.Domain.Lock {
    internal sealed class LockEntityTypeConfiguration : IEntityTypeConfiguration<Entities.Lock> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Lock> builder) {
            builder.ToTable("Locks");

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                   .HasColumnType("bigint");

            builder.Property(p => p.Name)
                   .HasMaxLength(128)
                   .IsRequired();

            builder.Property(p => p.Address)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(p => p.IsPublic)
                   .HasDefaultValue(false);

            builder.Property(p => p.IsLocked)
                   .HasDefaultValue(false);

            builder.Property(p => p.CreateDate)
                   .IsRequired();

            builder.HasMany(e => e.AccessedUsers)
                   .WithMany(e => e.AccessedLocks)
                   .UsingEntity("UserLock");

        }
    }
}
