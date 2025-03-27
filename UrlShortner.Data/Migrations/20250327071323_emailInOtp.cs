using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortner.Data.Migrations
{
    /// <inheritdoc />
    public partial class emailInOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserOtps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserOtps");
        }
    }
}
