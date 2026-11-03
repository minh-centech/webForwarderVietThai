using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

public interface IHangXuatKhauApi
{
    Task<IReadOnlyList<XacNhanNhapKhoRow>> GetXacNhanNhapKhoAsync(long idUser, DateTime tuNgay, DateTime denNgay, CancellationToken ct = default);
    Task<IReadOnlyList<XacNhanDongHangRow>> GetXacNhanDongHangAsync(long idUser, DateTime tuNgay, DateTime denNgay, CancellationToken ct = default);
}