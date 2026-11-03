using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

public sealed class DaiLyApiClient(HttpClient http) : ApiClientBase(http), IDaiLyApi
{
    public async Task<IReadOnlyList<DaiLy>> ListAsync(CancellationToken ct = default)
    {
        var list = await GetAsync<List<DaiLy>>("api/DanhMucDaiLy/List", ct) ?? [];
        return list.OrderBy(x => x.Ma, StringComparer.OrdinalIgnoreCase).ToList();
    }
}
