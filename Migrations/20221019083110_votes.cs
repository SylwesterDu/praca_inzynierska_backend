using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace praca_inzynierska_backend.Migrations
{
    public partial class votes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownVotes",
                table: "Artworks");

            migrationBuilder.DropColumn(
                name: "Upvotes",
                table: "Artworks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Artworks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Artworks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArtworkUser",
                columns: table => new
                {
                    UpvotedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpvotesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkUser", x => new { x.UpvotedById, x.UpvotesId });
                    table.ForeignKey(
                        name: "FK_ArtworkUser_Artworks_UpvotesId",
                        column: x => x.UpvotesId,
                        principalTable: "Artworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtworkUser_AspNetUsers_UpvotedById",
                        column: x => x.UpvotedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtworkUser1",
                columns: table => new
                {
                    DownVotedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DownVotesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkUser1", x => new { x.DownVotedById, x.DownVotesId });
                    table.ForeignKey(
                        name: "FK_ArtworkUser1_Artworks_DownVotesId",
                        column: x => x.DownVotesId,
                        principalTable: "Artworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtworkUser1_AspNetUsers_DownVotedById",
                        column: x => x.DownVotedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkUser_UpvotesId",
                table: "ArtworkUser",
                column: "UpvotesId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkUser1_DownVotesId",
                table: "ArtworkUser1",
                column: "DownVotesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtworkUser");

            migrationBuilder.DropTable(
                name: "ArtworkUser1");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Artworks");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Artworks");

            migrationBuilder.AddColumn<int>(
                name: "DownVotes",
                table: "Artworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Upvotes",
                table: "Artworks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
