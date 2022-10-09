using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace praca_inzynierska_backend.Migrations
{
    public partial class cascade_Delete2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesData_Artworks_ArtworkId",
                table: "FilesData");

            migrationBuilder.AddForeignKey(
                name: "FK_FilesData_Artworks_ArtworkId",
                table: "FilesData",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesData_Artworks_ArtworkId",
                table: "FilesData");

            migrationBuilder.AddForeignKey(
                name: "FK_FilesData_Artworks_ArtworkId",
                table: "FilesData",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id");
        }
    }
}
