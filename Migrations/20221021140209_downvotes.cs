using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class downvotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Upvotes",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "Downvotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArtworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Downvotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Downvotes_Artworks_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "Artworks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Downvotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Downvotes_ArtworkId",
                table: "Downvotes",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Downvotes_UserId",
                table: "Downvotes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Downvotes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Upvotes",
                newName: "id");
        }
    }
}
