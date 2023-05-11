using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using SmartLock.Domain;
using SmartLock.Domain.Entities;
using SmartLock.Infrastructure.Consts;

namespace SmartLock.Infrastructure.Domain.Trasaction {
    public sealed class TrasanctionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction> {
        public void Configure(EntityTypeBuilder<Transaction> builder) {
            builder.ToTable("Transactions", DataBaseSchema.DataSchema);

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                   .HasColumnType("bigint");

            builder.Property(p => p.Action)
                   .HasColumnType("tinyint")
                   .HasConversion<EnumToNumberConverter<ActionType, byte>>()
                   .IsRequired();

            builder.HasOne(p => p.Lock)
                   .WithMany()
                   .HasForeignKey(p => p.LockId);

            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId);

            builder.Property(p => p.Success)
                   .IsRequired();

            builder.Property(p => p.CreateDate)
                   .IsRequired();

            builder.Ignore(p => p.LastUpdateDate);
        }
    }
}
