using Microsoft.EntityFrameworkCore.Migrations;
using praca_inzynierska_backend.Misc;

#nullable disable

namespace backend.Migrations
{
    public partial class new_db2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ArtType>(
                name: "ArtType",
                table: "Artworks",
                type: "art_type",
                nullable: false,
                defaultValue: ArtType.MUSIC);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtType",
                table: "Artworks");
        }
    }
}
