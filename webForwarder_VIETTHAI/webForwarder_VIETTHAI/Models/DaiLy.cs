namespace webForwarder_VIETTHAI.Models;

/// <summary>Đại lý (DanhMucDaiLy) – từ GET api/DanhMucDaiLy/List: [{ ID, Ma, Ten }].</summary>
public sealed class DaiLy
{
    public long ID { get; set; }
    public string Ma { get; set; } = "";
    public string Ten { get; set; } = "";

    /// <summary>dropdown / bảng.</summary>
    public string HienThi => string.IsNullOrWhiteSpace(Ten) ? Ma : $"{Ma} – {Ten}";
}

