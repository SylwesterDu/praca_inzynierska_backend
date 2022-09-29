using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace praca_inzynierska_backend.Migrations
{
    public partial class filedata2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UploadProcessId",
                table: "FilesData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FilesData_UploadProcessId",
                table: "FilesData",
                column: "UploadProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilesData_UploadProcesses_UploadProcessId",
                table: "FilesData",
                column: "UploadProcessId",
                principalTable: "UploadProcesses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilesData_UploadProcesses_UploadProcessId",
                table: "FilesData");

            migrationBuilder.DropIndex(
                name: "IX_FilesData_UploadProcessId",
                table: "FilesData");

            migrationBuilder.DropColumn(
                name: "UploadProcessId",
                table: "FilesData");
        }
    }
}
