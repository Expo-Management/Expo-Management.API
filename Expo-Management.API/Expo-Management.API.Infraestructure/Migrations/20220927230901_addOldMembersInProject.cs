using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expo_Management.API.Infraestructure.Migrations
{
    public partial class addOldMembersInProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "oldMembers",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "oldMembers",
                table: "Projects");

        }
    }
}
