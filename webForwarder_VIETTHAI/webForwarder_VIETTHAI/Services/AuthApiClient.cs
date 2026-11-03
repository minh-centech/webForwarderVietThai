using System.Text.Json;
using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Models.Api;

namespace webForwarder_VIETTHAI.Services;

public sealed class AuthApiClient(HttpClient http, IKhachHangApi khachHangApi, ILogger<AuthApiClient> logger)
    : ApiClientBase(http), IAuthApi
{
    public async Task<LoginResult?> LoginAsync(string maKH, string matKhau, CancellationToken ct = default)
    {
        KhachHangLoginData? data;
        try
        {
            data = await PostAsync<KhachHangLoginData>(
                "api/DanhMucKhachHangForwarder/Login", new LoginRequest(maKH, matKhau), ct);
        }
        catch (ApiException ex)
        {
            logger.LogInformation("Đăng nhập {MaKH} thất bại: {Message}", maKH, ex.Message);
            return null;
        }

        if (data is null || string.IsNullOrWhiteSpace(data.MaKH))
        {
            return null;
        }

        _ = long.TryParse(data.ID, out var id);
        var loai = data.LoaiTaiKhoan;
        var ten = string.IsNullOrWhiteSpace(data.TenKhachHang) ? data.TenKH : data.TenKhachHang;

        if (loai is null || string.IsNullOrWhiteSpace(ten))
        {
            try
            {
                var kh = await khachHangApi.GetByIdAsync(id, ct);
                loai ??= kh?.LoaiTaiKhoan;
                ten = string.IsNullOrWhiteSpace(ten) ? kh?.TenKH : ten;
            }
            catch (Exception ex) when (ex is ApiException or HttpRequestException or JsonException)
            {
                logger.LogWarning("Không lấy được thông tin chi tiết khách hàng {Id}: {Message}", id, ex.Message);
            }
        }

        return new LoginResult(id, data.MaKH, string.IsNullOrWhiteSpace(ten) ? data.MaKH : ten, TaiKhoan.RoleFrom(loai), Token: null, data.IDDanhMucDaiLy);
    }
}
