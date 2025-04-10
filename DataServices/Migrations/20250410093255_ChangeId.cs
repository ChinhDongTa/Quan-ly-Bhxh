using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataServices.Migrations
{
    /// <inheritdoc />
    public partial class ChangeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalaryCoefficientId",
                table: "SalaryCoefficients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RewardId",
                table: "Rewards",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "QuarterEmployeeRankId",
                table: "QuarterEmployeeRanks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "QuarterDepartmentRankId",
                table: "QuarterDepartmentRanks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "Positions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LevelId",
                table: "Level",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Employees",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Departments",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SalaryCoefficients",
                newName: "SalaryCoefficientId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Rewards",
                newName: "RewardId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "QuarterEmployeeRanks",
                newName: "QuarterEmployeeRankId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "QuarterDepartmentRanks",
                newName: "QuarterDepartmentRankId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Positions",
                newName: "PositionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Level",
                newName: "LevelId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Departments",
                newName: "DepartmentId");
        }
    }
}
