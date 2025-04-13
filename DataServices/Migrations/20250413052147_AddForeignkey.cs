using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataServices.Migrations {
    /// <inheritdoc />
    public partial class AddForeignkey : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_QuarterScoreDepts_DeptId",
                table: "QuarterScoreDepts",
                column: "DeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterScoreDepts_Departments_DeptId",
                table: "QuarterScoreDepts",
                column: "DeptId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuarterScoreDepts_Departments_DeptId",
                table: "QuarterScoreDepts");

            migrationBuilder.DropIndex(
                name: "IX_QuarterScoreDepts_DeptId",
                table: "QuarterScoreDepts");
        }
    }
}