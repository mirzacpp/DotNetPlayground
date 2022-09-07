using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studens.MultitenantApp.Web.Migrations
{
    public partial class InitAppSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataKey = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookTeamMember",
                columns: table => new
                {
                    BooksId = table.Column<long>(type: "bigint", nullable: false),
                    TeamMembersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTeamMember", x => new { x.BooksId, x.TeamMembersId });
                    table.ForeignKey(
                        name: "FK_BookTeamMember_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTeamMember_TeamMembers_TeamMembersId",
                        column: x => x.TeamMembersId,
                        principalTable: "TeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_DataKey",
                table: "Books",
                column: "DataKey");

            migrationBuilder.CreateIndex(
                name: "IX_BookTeamMember_TeamMembersId",
                table: "BookTeamMember",
                column: "TeamMembersId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_DataKey",
                table: "TeamMembers",
                column: "DataKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookTeamMember");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "TeamMembers");
        }
    }
}
