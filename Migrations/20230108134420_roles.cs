using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3ff684c2-4d7f-40d2-9420-5363d63b7641"), "8a04333c-dd30-4ea1-8c00-a2329aa57235", "creator", "CREATOR" },
                    { new Guid("607d37f0-ce0b-4706-b64f-a24ac6659733"), "6abcf120-edd0-4e25-8932-da764971a509", "admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3ff684c2-4d7f-40d2-9420-5363d63b7641"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("607d37f0-ce0b-4706-b64f-a24ac6659733"));
        }
    }
}
