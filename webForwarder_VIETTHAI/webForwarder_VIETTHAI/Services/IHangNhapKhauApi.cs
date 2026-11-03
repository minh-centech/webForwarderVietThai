using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

public interface IHangNhapKhauApi
{
    Task<IReadOnlyList<XacNhanXuatKhoRow>> GetXacNhanXuatKhoAsync(long idUser, DateTime tuNgay, DateTime denNgay, CancellationToken ct = default);
}