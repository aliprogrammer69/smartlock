using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartLock.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class init_sql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "Bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PaswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RefreshTokenExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LockId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "Bigint", nullable: false),
                    Action = table.Column<byte>(type: "tinyint", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Locks_LockId",
                        column: x => x.LockId,
                        principalTable: "Locks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLock",
                columns: table => new
                {
                    AccessedLocksId = table.Column<long>(type: "bigint", nullable: false),
                    AccessedUsersId = table.Column<long>(type: "Bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLock", x => new { x.AccessedLocksId, x.AccessedUsersId });
                    table.ForeignKey(
                        name: "FK_UserLock_Locks_AccessedLocksId",
                        column: x => x.AccessedLocksId,
                        principalTable: "Locks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLock_Users_AccessedUsersId",
                        column: x => x.AccessedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateDate", "Key", "LastUpdateDate", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 9, 14, 17, 4, 93, DateTimeKind.Local).AddTicks(648), "admin", null, "Administrator" },
                    { 2, new DateTime(2023, 5, 9, 14, 17, 4, 93, DateTimeKind.Local).AddTicks(694), "user", null, "User" },
                    { 3, new DateTime(2023, 5, 9, 14, 17, 4, 93, DateTimeKind.Local).AddTicks(697), "system", null, "System" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_LockId",
                table: "Transactions",
                column: "LockId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLock_AccessedUsersId",
                table: "UserLock",
                column: "AccessedUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UserLock");

            migrationBuilder.DropTable(
                name: "Locks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
