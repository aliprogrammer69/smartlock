﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartLock.Infrastructure.DataBase;

#nullable disable

namespace SmartLock.Infrastructure.Database.Migrations
{
    [DbContext(typeof(SmartLockDbContext))]
    partial class SmartLockDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SmartLock.Domain.Entities.Lock", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsLocked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsPublic")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Locks", (string)null);
                });

            modelBuilder.Entity("SmartLock.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreateDate = new DateTime(2023, 5, 9, 14, 17, 4, 93, DateTimeKind.Local).AddTicks(648),
                            Key = "admin",
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            CreateDate = new DateTime(2023, 5, 9, 14, 17, 4, 93, DateTimeKind.Local).AddTicks(694),
                            Key = "user",
                            Name = "User"
                        },
                        new
                        {
                            Id = 3,
                            CreateDate = new DateTime(2023, 5, 9, 14, 17, 4, 93, DateTimeKind.Local).AddTicks(697),
                            Key = "system",
                            Name = "System"
                        });
                });

            modelBuilder.Entity("SmartLock.Domain.Entities.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<byte>("Action")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("LockId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<long>("UserId")
                        .HasColumnType("Bigint");

                    b.HasKey("Id");

                    b.HasIndex("LockId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions", (string)null);
                });

            modelBuilder.Entity("SmartLock.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("Bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("PaswordSalt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("RefreshToken");

                    b.Property<DateTime?>("RefreshTokenExpireDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("RefreshTokenExpireDate");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("UserLock", b =>
                {
                    b.Property<long>("AccessedLocksId")
                        .HasColumnType("bigint");

                    b.Property<long>("AccessedUsersId")
                        .HasColumnType("Bigint");

                    b.HasKey("AccessedLocksId", "AccessedUsersId");

                    b.HasIndex("AccessedUsersId");

                    b.ToTable("UserLock");
                });

            modelBuilder.Entity("SmartLock.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("SmartLock.Domain.Entities.Lock", "Lock")
                        .WithMany()
                        .HasForeignKey("LockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLock.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lock");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartLock.Domain.Entities.User", b =>
                {
                    b.HasOne("SmartLock.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("UserLock", b =>
                {
                    b.HasOne("SmartLock.Domain.Entities.Lock", null)
                        .WithMany()
                        .HasForeignKey("AccessedLocksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartLock.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("AccessedUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
