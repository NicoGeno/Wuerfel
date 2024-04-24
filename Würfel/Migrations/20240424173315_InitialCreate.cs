using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Würfel.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Benutzer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Benutzername = table.Column<string>(type: "TEXT", nullable: false),
                    Passwort = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Balance = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benutzer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoreBoard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BenutzerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Winstreak = table.Column<int>(type: "INTEGER", nullable: false),
                    Bet = table.Column<double>(type: "REAL", nullable: false),
                    Win = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreBoard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreBoard_Benutzer_BenutzerId",
                        column: x => x.BenutzerId,
                        principalTable: "Benutzer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoreBoard_BenutzerId",
                table: "ScoreBoard",
                column: "BenutzerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreBoard");

            migrationBuilder.DropTable(
                name: "Benutzer");
        }
    }
}
