using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataServices.Migrations
{
    /// <inheritdoc />
    public partial class WSUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkSchedules",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_UserId",
                table: "WorkSchedules",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedules_AspNetUsers_UserId",
                table: "WorkSchedules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedules_AspNetUsers_UserId",
                table: "WorkSchedules");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedules_UserId",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkSchedules");
        }
    }
}
