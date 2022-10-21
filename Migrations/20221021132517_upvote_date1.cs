using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class upvote_date1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upvote_Artworks_ArtworkId",
                table: "Upvote");

            migrationBuilder.DropForeignKey(
                name: "FK_Upvote_AspNetUsers_UserId",
                table: "Upvote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Upvote",
                table: "Upvote");

            migrationBuilder.RenameTable(
                name: "Upvote",
                newName: "Upvotes");

            migrationBuilder.RenameIndex(
                name: "IX_Upvote_UserId",
                table: "Upvotes",
                newName: "IX_Upvotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Upvote_ArtworkId",
                table: "Upvotes",
                newName: "IX_Upvotes_ArtworkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Upvotes",
                table: "Upvotes",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Upvotes_Artworks_ArtworkId",
                table: "Upvotes",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Upvotes_AspNetUsers_UserId",
                table: "Upvotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upvotes_Artworks_ArtworkId",
                table: "Upvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Upvotes_AspNetUsers_UserId",
                table: "Upvotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Upvotes",
                table: "Upvotes");

            migrationBuilder.RenameTable(
                name: "Upvotes",
                newName: "Upvote");

            migrationBuilder.RenameIndex(
                name: "IX_Upvotes_UserId",
                table: "Upvote",
                newName: "IX_Upvote_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Upvotes_ArtworkId",
                table: "Upvote",
                newName: "IX_Upvote_ArtworkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Upvote",
                table: "Upvote",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Upvote_Artworks_ArtworkId",
                table: "Upvote",
                column: "ArtworkId",
                principalTable: "Artworks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Upvote_AspNetUsers_UserId",
                table: "Upvote",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
