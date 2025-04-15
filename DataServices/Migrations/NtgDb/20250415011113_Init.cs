using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataServices.Migrations.NtgDb
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoQuanBhxhs",
                columns: table => new
                {
                    MaHuyen = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaHuyenHgd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaCqct = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoQuanBhxhs", x => x.MaHuyen);
                });

            migrationBuilder.CreateTable(
                name: "Ntgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDvi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaHuyen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoBhxh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoKcb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoaiDt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DanToc = table.Column<int>(type: "int", nullable: false),
                    SoCmnd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiLh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiHk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TyLeBhtn = table.Column<int>(type: "int", nullable: true),
                    CongViec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaXaLh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaHuyenLh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HanTheDen = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DangNhapVssid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaDangKyVssid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiXacThuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDdcnCccdBca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThongTinKhongChinhXac = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VssEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoQuanBhxhMaHuyen = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ntgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ntgs_CoQuanBhxhs_CoQuanBhxhMaHuyen",
                        column: x => x.CoQuanBhxhMaHuyen,
                        principalTable: "CoQuanBhxhs",
                        principalColumn: "MaHuyen");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ntgs_CoQuanBhxhMaHuyen",
                table: "Ntgs",
                column: "CoQuanBhxhMaHuyen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ntgs");

            migrationBuilder.DropTable(
                name: "CoQuanBhxhs");
        }
    }
}
