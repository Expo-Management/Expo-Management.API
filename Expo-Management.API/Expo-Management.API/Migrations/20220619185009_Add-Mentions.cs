using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expo_Management.API.Migrations
{
    public partial class AddMentions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mention",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mention", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MentionProjectModel",
                columns: table => new
                {
                    MentionsId = table.Column<int>(type: "int", nullable: false),
                    ProjectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentionProjectModel", x => new { x.MentionsId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_MentionProjectModel_Mention_MentionsId",
                        column: x => x.MentionsId,
                        principalTable: "Mention",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentionProjectModel_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MentionProjectModel_ProjectsId",
                table: "MentionProjectModel",
                column: "ProjectsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentionProjectModel");

            migrationBuilder.DropTable(
                name: "Mention");
        }
    }
}
