using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataServices.Entities.Tst;

public class Ntg {

    [Key]
    public int Id { get; set; }

    public string MaDvi { get; set; } = string.Empty;

    [ForeignKey(nameof(MaHuyen))]
    public string MaHuyen { get; set; } = string.Empty;

    public string SoBhxh { get; set; } = string.Empty;
    public string SoKcb { get; set; } = string.Empty;
    public string LoaiDt { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string GioiTinh { get; set; } = string.Empty;
    public string NgaySinh { get; set; } = string.Empty;
    public int DanToc { get; set; }
    public string SoCmnd { get; set; } = string.Empty;
    public string DiaChiLh { get; set; } = string.Empty;
    public string DiaChiHk { get; set; } = string.Empty;
    public int? TyLeBhtn { get; set; }
    public string? CongViec { get; set; }
    public string MaXaLh { get; set; } = string.Empty;
    public string MaHuyenLh { get; set; } = string.Empty;
    public DateTime? HanTheDen { get; set; }
    public string SoDienThoai { get; set; } = string.Empty;
    public string DangNhapVssid { get; set; } = string.Empty;
    public string DaDangKyVssid { get; set; } = string.Empty;
    public string TrangThaiXacThuc { get; set; } = string.Empty;
    public string SoDdcnCccdBca { get; set; } = string.Empty;
    public string ThongTinKhongChinhXac { get; set; } = string.Empty;
    public string VssEmail { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public virtual CoQuanBhxh? CoQuanBhxh { get; set; }
}