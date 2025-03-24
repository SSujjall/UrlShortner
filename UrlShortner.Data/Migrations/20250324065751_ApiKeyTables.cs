using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortner.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApiKeyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Urls",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApiUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    KeyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApiKeyHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_ApiKeys_ApiUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApiUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urls_UserID",
                table: "Urls",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_UserId",
                table: "ApiKeys",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_ApiUsers_UserID",
                table: "Urls",
                column: "UserID",
                principalTable: "ApiUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_ApiUsers_UserID",
                table: "Urls");

            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "ApiUsers");

            migrationBuilder.DropIndex(
                name: "IX_Urls_UserID",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Urls");
        }
    }
}
