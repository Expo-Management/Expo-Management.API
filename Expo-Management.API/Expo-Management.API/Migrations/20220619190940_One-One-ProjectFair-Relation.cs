using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expo_Management.API.Migrations
{
    public partial class OneOneProjectFairRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FairId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FairId",
                table: "Projects",
                column: "FairId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Fair_FairId",
                table: "Projects",
                column: "FairId",
                principalTable: "Fair",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Fair_FairId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FairId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FairId",
                table: "Projects");
        }
    }
}
