namespace DataServices.Entities.Tst;

public class ImportHistory {
    public int Id { get; set; }
    public DateTime ImportedAt { get; set; } = default;
    public string Note { get; set; } = string.Empty;
}