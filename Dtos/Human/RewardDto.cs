namespace Dtos.Human;

public record RewardDto {
    public int Id { get; set; }
    /// <summary>
    /// Tên phần thưởng.
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Tên viết tắt của phần thưởng.
    /// </summary>
    public string? ShortName { get; set; }
    /// <summary>
    /// Phân loại phần thưởng của cá nhân hay tập thể, chỉ có 2 giá trị: CN, TT.
    /// </summary>
    public string? Classify { get; set; }

    /// <summary>
    /// Phân loại Khen thưởng hay kỷ luật, chỉ có 2 giá trị: KT, KL.
    /// </summary>
    public string? Type { get; set; }
}