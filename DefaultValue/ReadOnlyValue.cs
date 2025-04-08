namespace DefaultValue;

public record ReadOnlyValue {
    /// <summary>
    /// Vì xếp loại quý là sau quý hiện tại nên phải trừ đi một số ngày đẻ lấy quý hiện tại
    /// </summary>
    public const int SubDay = -25;
}