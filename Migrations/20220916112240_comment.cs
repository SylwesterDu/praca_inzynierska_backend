using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtWorks_AspNetUsers_OwnerId",
                table: "ArtWorks");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ArtWorks_ArtWorkId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ArtWorks_ArtWorkId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArtWorks",
                table: "ArtWorks");

            migrationBuilder.RenameTable(
                name: "ArtWorks",
                newName: "Artworks");

            migrationBuilder.RenameColumn(
                name: "ArtWorkId",
                table: "Tags",
                newName: "ArtworkId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_ArtWorkId",
                table: "Tags",
                newName: "IX_Tags_ArtworkId");

            migrationBuilder.RenameColumn(
                name: "ArtWorkId",
                table: "Comments",
                newName: "ArtworkId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ArtWorkId",
                table: "Comments",
                newName: "IX_Comments_ArtworkId");

            migrationBuilder.RenameIndex(
                name: "IX_ArtWorks_OwnerId",
                table: "Artworks",
                newName: "IX_Artworks_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artworks",
                table: "Artworks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_AspNetUsers_OwnerId",
                table: "Artworks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_AspNetUsers_OwnerId",
                table: "Artworks");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Artworks_ArtworkId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Artworks_ArtworkId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artworks",
                table: "Artworks");

            migrationBuilder.RenameTable(
                name: "Artworks",
                newName: "ArtWorks");

            migrationBuilder.RenameColumn(
                name: "ArtworkId",
                table: "Tags",
                newName: "ArtWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_ArtworkId",
                table: "Tags",
                newName: "IX_Tags_ArtWorkId");

            migrationBuilder.RenameColumn(
                name: "ArtworkId",
                table: "Comments",
                newName: "ArtWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ArtworkId",
                table: "Comments",
                newName: "IX_Comments_ArtWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_Artworks_OwnerId",
                table: "ArtWorks",
                newName: "IX_ArtWorks_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArtWorks",
                table: "ArtWorks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtWorks_AspNetUsers_OwnerId",
                table: "ArtWorks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ArtWorks_ArtWorkId",
                table: "Comments",
                column: "ArtWorkId",
                principalTable: "ArtWorks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_ArtWorks_ArtWorkId",
                table: "Tags",
                column: "ArtWorkId",
                principalTable: "ArtWorks",
                principalColumn: "Id");
        }
    }
}
