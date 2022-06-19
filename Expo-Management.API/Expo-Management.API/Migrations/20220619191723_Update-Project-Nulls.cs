using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expo_Management.API.Migrations
{
    public partial class UpdateProjectNulls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Files_FilesId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "FilesId",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Files_FilesId",
                table: "Projects",
                column: "FilesId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Files_FilesId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "FilesId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Files_FilesId",
                table: "Projects",
                column: "FilesId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
