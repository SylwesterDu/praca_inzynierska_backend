using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class cascade_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Downvotes_Artworks_ArtworkId",
                table: "Downvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Downvotes_AspNetUsers_UserId",
                table: "Downvotes");

            migrationBuilder.AddForeignKey(
                name: "FK_Downvotes_Artworks_ArtworkId",
                table: "Downvotes",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Downvotes_AspNetUsers_UserId",
                table: "Downvotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Downvotes_Artworks_ArtworkId",
                table: "Downvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Downvotes_AspNetUsers_UserId",
                table: "Downvotes");

            migrationBuilder.AddForeignKey(
                name: "FK_Downvotes_Artworks_ArtworkId",
                table: "Downvotes",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Downvotes_AspNetUsers_UserId",
                table: "Downvotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
