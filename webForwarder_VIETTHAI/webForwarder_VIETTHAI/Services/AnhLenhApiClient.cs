using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

/// <summary>
/// Gọi API ảnh của cenCFS-ITS (BaseAddress = AnhApi:BaseUrl trong appsettings.json, ví dụ http://123.30.235.18/cenCFS-ITS/).
/// Body dạng form-urlencoded: SoLenhNhap=LNX26092800001 
/// </summary>
public sealed class AnhLenhApiClient(HttpClient http) : ApiClientBase(http), IAnhLenhApi
{
    public async Task<IReadOnlyList<string>> GetAnhAsync(LoaiLenh loai, string soLenh, CancellationToken ct = default)
    {
        var form = new Dictionary<string, string> { [LoaiLenhInfo.TenThamSo(loai)] = soLenh.Trim() };
        var data = await PostFormAsync<AnhLenhResult>(LoaiLenhInfo.ApiPath(loai), form, ct);

        return data?.FilePaths?.Where(u => !string.IsNullOrWhiteSpace(u)).ToList() ?? [];
    }

    public async Task<(byte[] Bytes, string ContentType)?> TaiAnhAsync(string url, CancellationToken ct = default)
    {
        // Chỉ tải ảnh nằm trên đúng server API ảnh (chặn lợi dụng web làm proxy sang nơi khác)
        if (Http.BaseAddress is null || !url.StartsWith(Http.BaseAddress.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        using var response = await Http.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
        if (!contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            contentType = "image/jpeg";
        }

        return (await response.Content.ReadAsByteArrayAsync(ct), contentType);
    }
}
