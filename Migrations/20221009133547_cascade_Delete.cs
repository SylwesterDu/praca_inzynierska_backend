using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace praca_inzynierska_backend.Migrations
{
    public partial class cascade_Delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Artworks_ArtworkId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Artworks_ArtworkId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "published",
                table: "Artworks",
                newName: "Published");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Artworks_ArtworkId",
                table: "Comments",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Artworks_ArtworkId",
                table: "Tags",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Artworks_ArtworkId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Artworks_ArtworkId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "Published",
                table: "Artworks",
                newName: "published");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Artworks_ArtworkId",
                table: "Comments",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Artworks_ArtworkId",
                table: "Tags",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id");
        }
    }
}
