using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTranfer.Parameter;
/// <summary>
/// Tham số biên bản họp quý
/// </summary>
public record BienBanHopQuyPara {
    public string ThoiGian { get; set; } = string.Empty;
    public int SoThanhVienVangMat { get; set; }
    public string XepLoaiPhong { get; set; } = "A";
    public string DeXuatKhenThuongTapThe { get; set; } = "hoàn thành xuất sắc nhiệm vụ";
    public string ChuTri { get; set; } = string.Empty;
    public string ChucVuChuTri { get; set; } = string.Empty;
    public string ThuKy { get; set; } = string.Empty;
    public string ChucVuThuKy { get; set; } = string.Empty;
    public string KetLuan { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}