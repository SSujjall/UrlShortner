using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortner.Data.Migrations
{
    /// <inheritdoc />
    public partial class otpUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OtpUsed",
                table: "UserOtps",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpUsed",
                table: "UserOtps");
        }
    }
}
