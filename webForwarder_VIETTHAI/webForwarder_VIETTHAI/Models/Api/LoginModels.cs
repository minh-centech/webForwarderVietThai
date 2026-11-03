using System.Text.Json.Serialization;

namespace webForwarder_VIETTHAI.Models.Api;

/// <summary>Body gửi lên POST api/DanhMucKhachHangForwarder/Login.</summary>
public sealed record LoginRequest(
    [property: JsonPropertyName("MaKH")] string MaKH,
    [property: JsonPropertyName("MatKhau")] string MatKhau);
public sealed class KhachHangLoginData
{
    public string? ID { get; set; }
    public string? MaKH { get; set; }
    public string? TenKhachHang { get; set; }
    public string? TenKH { get; set; }
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public int? LoaiTaiKhoan { get; set; }
    public long? IDDanhMucDaiLy { get; set; }
}

/// <summary>Kết quả đăng nhập đã chuẩn hoá cho phía web.</summary>
public sealed record LoginResult(long Id, string MaKH, string TenKH, string Role, string? Token, long? IDDanhMucDaiLy = null);

/// <summary>Lỗi do API trả về (status != 0).</summary>
public sealed class ApiException(string message) : Exception(message);
