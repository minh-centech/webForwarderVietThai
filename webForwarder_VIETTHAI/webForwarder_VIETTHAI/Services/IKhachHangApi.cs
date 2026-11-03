using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

/// <summary>Các thao tác trên bảng DanhMucKhachHangForwarder (qua Web API).</summary>
public interface IKhachHangApi
{
    Task<PagedResult<KhachHang>> ListAsync(string? keyword, int page, int pageSize, CancellationToken ct = default);
    Task<KhachHang?> GetByIdAsync(long id, CancellationToken ct = default);

    /// <summary>Thêm mới; trả về ID vừa tạo.</summary>
    Task<long> InsertAsync(KhachHang kh, string matKhau, CancellationToken ct = default);

    /// <summary>Cập nhật thông tin.</summary>
    Task UpdateAsync(KhachHang kh, string? matKhau, CancellationToken ct = default);

    Task DeleteAsync(long id, CancellationToken ct = default);

    /// <summary>Admin đặt mật khẩu mới cho khách hàng.</summary>
    Task ResetPasswordAsync(long id, string matKhauMoi, CancellationToken ct = default);

}
