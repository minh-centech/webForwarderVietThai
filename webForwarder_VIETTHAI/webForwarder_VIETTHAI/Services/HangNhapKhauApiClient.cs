using System.Globalization;
using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

public sealed class HangNhapKhauApiClient(HttpClient http) : ApiClientBase(http), IHangNhapKhauApi
{
    private const string DateFormat = "yyyy-MM-dd";

    public async Task<IReadOnlyList<XacNhanXuatKhoRow>> GetXacNhanXuatKhoAsync(long idUser, DateTime tuNgay, DateTime denNgay, CancellationToken ct = default)
        => await PostAsync<List<XacNhanXuatKhoRow>>(
               "api/ctXacNhanHoanThanhXuatKhoHangNhapKhau/List",
               new
               {
                   IDUser = idUser.ToString(CultureInfo.InvariantCulture),
                   TuNgay = tuNgay.ToString(DateFormat, CultureInfo.InvariantCulture),
                   DenNgay = denNgay.ToString(DateFormat, CultureInfo.InvariantCulture),
               },
               ct) ?? [];
}
