using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

public sealed class HangXuatKhauApiClient(HttpClient http, IHttpContextAccessor httpContextAccessor)
    : ApiClientBase(http), IHangXuatKhauApi
{

    public async Task<IReadOnlyList<XacNhanNhapKhoRow>> GetXacNhanNhapKhoAsync(long idUser, DateTime tuNgay, DateTime denNgay, CancellationToken ct = default)
        => await PostAsync<List<XacNhanNhapKhoRow>>(
               "api/ctXacNhanHoanThanhLenhNhapKhoHangXuatKhau/List", Body(idUser,tuNgay, denNgay), ct) ?? [];

    public async Task<IReadOnlyList<XacNhanDongHangRow>> GetXacNhanDongHangAsync(long idUser, DateTime tuNgay, DateTime denNgay, CancellationToken ct = default)
        => await PostAsync<List<XacNhanDongHangRow>>(
               "api/ctXacNhanHoanThanhDongHangXuatKhau/List" , Body(idUser, tuNgay, denNgay), ct) ?? [];

    private static object Body(long idUser, DateTime tuNgay, DateTime denNgay)
    => new
    {
        IDUser = idUser.ToString(),
        TuNgay = tuNgay.ToString(DateFormat),
        DenNgay = denNgay.ToString(DateFormat),
    };


    private const string DateFormat = "yyyy-MM-dd";

    private static object Body(DateTime tuNgay, DateTime denNgay)
        => new { TuNgay = tuNgay.ToString(DateFormat), DenNgay = denNgay.ToString(DateFormat) };
}
