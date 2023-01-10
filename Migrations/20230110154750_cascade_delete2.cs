using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class cascade_delete2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upvotes_Artworks_ArtworkId",
                table: "Upvotes");

            migrationBuilder.AddForeignKey(
                name: "FK_Upvotes_Artworks_ArtworkId",
                table: "Upvotes",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upvotes_Artworks_ArtworkId",
                table: "Upvotes");

            migrationBuilder.AddForeignKey(
                name: "FK_Upvotes_Artworks_ArtworkId",
                table: "Upvotes",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id");
        }
    }
}
