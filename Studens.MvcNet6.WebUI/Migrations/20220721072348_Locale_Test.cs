using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studens.MvcNet6.WebUI.Migrations
{
    public partial class Locale_Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookLocales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLocales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookLocales_Book_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookLocales_LanguageCode_ParentId",
                table: "BookLocales",
                columns: new[] { "LanguageCode", "ParentId" },
                unique: true,
                filter: "[LanguageCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookLocales_ParentId",
                table: "BookLocales",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookLocales");

            migrationBuilder.DropTable(
                name: "Book");
        }
    }
}
