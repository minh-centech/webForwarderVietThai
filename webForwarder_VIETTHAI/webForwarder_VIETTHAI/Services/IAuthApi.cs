using webForwarder_VIETTHAI.Models.Api;

namespace webForwarder_VIETTHAI.Services;

public interface IAuthApi
{
    Task<LoginResult?> LoginAsync(string maKH, string matKhau, CancellationToken ct = default);
}