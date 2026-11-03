using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

/// <summary>API ảnh chụp theo lệnh (hệ thống cenCFS-ITS).</summary>
public interface IAnhLenhApi
{
    /// <summary>Danh sách URL ảnh của một lệnh. Rỗng nếu lệnh không có ảnh.</summary>
    Task<IReadOnlyList<string>> GetAnhAsync(LoaiLenh loai, string soLenh, CancellationToken ct = default);

    /// <summary>Tải 1 ảnh về (để web phát lại cho trình duyệt – tránh lỗi mixed content http/https). Null nếu không tải được.</summary>
    Task<(byte[] Bytes, string ContentType)?> TaiAnhAsync(string url, CancellationToken ct = default);
}