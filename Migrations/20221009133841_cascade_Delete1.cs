using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace praca_inzynierska_backend.Migrations
{
    public partial class cascade_Delete1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesData_UploadProcesses_UploadProcessId",
                table: "FilesData");

            migrationBuilder.AddForeignKey(
                name: "FK_FilesData_UploadProcesses_UploadProcessId",
                table: "FilesData",
                column: "UploadProcessId",
                principalTable: "UploadProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesData_UploadProcesses_UploadProcessId",
                table: "FilesData");

            migrationBuilder.AddForeignKey(
                name: "FK_FilesData_UploadProcesses_UploadProcessId",
                table: "FilesData",
                column: "UploadProcessId",
                principalTable: "UploadProcesses",
                principalColumn: "Id");
        }
    }
}
