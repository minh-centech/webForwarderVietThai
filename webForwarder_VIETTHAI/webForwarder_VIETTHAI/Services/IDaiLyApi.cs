using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services;

public interface IDaiLyApi
{
    /// <summary>Danh sách đại lý để gắn vào tài khoản khách hàng (GET api/DanhMucDaiLy/List).</summary>
    Task<IReadOnlyList<DaiLy>> ListAsync(CancellationToken ct = default);
}
