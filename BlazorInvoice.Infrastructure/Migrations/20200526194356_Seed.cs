using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorInvoice.Infrastructure.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountGuid", "AccountId", "Checksum", "ConcurrencyStamp", "Created", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Salt", "SecurityStamp", "TwoFactorEnabled", "Updated", "UserName", "Verified" },
                values: new object[] { "7a7e88c4-0eb8-49b5-8704-e44e5834cb33", 0, new Guid("00000000-0000-0000-0000-000000000000"), 0, "c8394a41-8278-45e7-9dd8-0dd3ab5e4698", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bosbosjohan@gmail.com", false, false, "welkom2020", false, "9b3f73e4-d8c6-4afc-9647-b0a1f6de4b7c", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7a7e88c4-0eb8-49b5-8704-e44e5834cb33");
        }
    }
}
