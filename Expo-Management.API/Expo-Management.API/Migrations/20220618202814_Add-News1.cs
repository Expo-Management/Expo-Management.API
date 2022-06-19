using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expo_Management.API.Migrations
{
    public partial class AddNews1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AspNetUsers_PublisherId",
                table: "News");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Fair_FairId",
                table: "News");

            migrationBuilder.DropPrimaryKey(
                name: "PK_News",
                table: "News");

            migrationBuilder.RenameTable(
                name: "News",
                newName: "New");

            migrationBuilder.RenameIndex(
                name: "IX_News_PublisherId",
                table: "New",
                newName: "IX_New_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_News_FairId",
                table: "New",
                newName: "IX_New_FairId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_New",
                table: "New",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_New_AspNetUsers_PublisherId",
                table: "New",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_New_Fair_FairId",
                table: "New",
                column: "FairId",
                principalTable: "Fair",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_New_AspNetUsers_PublisherId",
                table: "New");

            migrationBuilder.DropForeignKey(
                name: "FK_New_Fair_FairId",
                table: "New");

            migrationBuilder.DropPrimaryKey(
                name: "PK_New",
                table: "New");

            migrationBuilder.RenameTable(
                name: "New",
                newName: "News");

            migrationBuilder.RenameIndex(
                name: "IX_New_PublisherId",
                table: "News",
                newName: "IX_News_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_New_FairId",
                table: "News",
                newName: "IX_News_FairId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_News",
                table: "News",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AspNetUsers_PublisherId",
                table: "News",
                column: "PublisherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Fair_FairId",
                table: "News",
                column: "FairId",
                principalTable: "Fair",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
