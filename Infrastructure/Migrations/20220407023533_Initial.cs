using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Examples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examples", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "varchar(130)", maxLength: 130, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChangedEmail = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastAccess = table.Column<DateTime>(type: "datetime", nullable: true),
                    EmailToken = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserRole = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Uid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Examples",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "example 1", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "example 2", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Uid", "ChangedEmail", "CreatedAt", "Email", "EmailConfirmed", "EmailToken", "FullName", "LastAccess", "Password", "RefreshToken", "RefreshTokenExpiryTime", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("14f091bc-cdb7-494a-bf4c-1f23da9244e5"), null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "visitante@teste.com", true, null, "Visitante", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9", null, null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Visitante" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Uid", "ChangedEmail", "CreatedAt", "Email", "EmailConfirmed", "EmailToken", "FullName", "LastAccess", "Password", "RefreshToken", "RefreshTokenExpiryTime", "UpdatedAt", "UserName", "UserRole" },
                values: new object[,]
                {
                    { new Guid("34c619e1-efeb-4e39-be1b-73f58a3bc443"), null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cliente@teste.com", true, null, "Cliente", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9", null, null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cliente", 1 },
                    { new Guid("7e085fa2-3726-4f90-a72e-7062e99d4e27"), null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@teste.com", true, null, "Admin", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9", null, null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Examples");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
