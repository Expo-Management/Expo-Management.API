using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expo_Management.API.Migrations
{
    public partial class UpdateEventProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Event",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Event",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Event",
                newName: "Date");
        }
    }
}
