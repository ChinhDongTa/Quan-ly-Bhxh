using System.ComponentModel.DataAnnotations;

namespace DataServices.Entities.Tst;

public class CoQuanBhxh {

    [Key]
    public string MaHuyen { get; set; } = string.Empty;

    public string Ten { get; set; } = string.Empty;

    public string MaHuyenHgd { get; set; } = string.Empty;

    public string MaCqct { get; set; } = string.Empty;

    public virtual ICollection<Ntg>? Ntgs { get; set; }
}