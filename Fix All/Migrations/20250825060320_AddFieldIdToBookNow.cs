using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fix_All.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldIdToBookNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "BookNow",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookNow_FieldId",
                table: "BookNow",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookNow_LaborFields_FieldId",
                table: "BookNow",
                column: "FieldId",
                principalTable: "LaborFields",
                principalColumn: "FieldId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookNow_LaborFields_FieldId",
                table: "BookNow");

            migrationBuilder.DropIndex(
                name: "IX_BookNow_FieldId",
                table: "BookNow");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "BookNow");
        }
    }
}
