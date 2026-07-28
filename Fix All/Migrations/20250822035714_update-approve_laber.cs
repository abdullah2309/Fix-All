using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace Fix_All.Migrations
{
    /// <inheritdoc />
    public partial class updateapprove_laber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "approve_labers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImagePath",
                table: "approve_labers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "approve_labers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Headline",
                table: "approve_labers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "approve_labers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioUrl",
                table: "approve_labers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "approve_labers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "approve_labers");

            migrationBuilder.DropColumn(
                name: "CoverImagePath",
                table: "approve_labers");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "approve_labers");

            migrationBuilder.DropColumn(
                name: "Headline",
                table: "approve_labers");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "approve_labers");

            migrationBuilder.DropColumn(
                name: "PortfolioUrl",
                table: "approve_labers");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "approve_labers");
        }
    }
}
