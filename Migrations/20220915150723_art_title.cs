using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace praca_inzynierska_backend.Migrations
{
    public partial class art_title : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ArtWorks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ArtWorks");
        }
    }
}
