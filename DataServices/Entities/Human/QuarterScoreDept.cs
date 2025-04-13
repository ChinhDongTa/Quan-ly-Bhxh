using System.ComponentModel.DataAnnotations;

namespace DataServices.Entities.Human;

/// <summary>
/// Bảng điểm của phòng được chấm bởi các huyện
/// </summary>
public partial class QuarterScoreDept {

    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Đơn vị chấm
    /// </summary>
    public int DeptId { get; set; }

    /// <summary>
    /// Đơn vị được chấm là đơn vị đã được đăng ký xếp loại quý
    /// </summary>
    public int QuarterDeptRankId { get; set; }

    public int? Score1 { get; set; }
    public int? Score2 { get; set; }
    public int? Score3 { get; set; }
    public int? Score4 { get; set; }
    public string? Note { get; set; }

    public virtual QuarterDepartmentRank? QuarterDeptRank { get; set; }
    public virtual Department? Dept { get; set; }
}