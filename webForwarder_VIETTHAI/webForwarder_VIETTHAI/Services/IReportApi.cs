using webForwarder_Report.Models;

namespace webForwarder_VIETTHAI.Services
{
    public interface IReportApi
    {
        Task<byte[]> TaiPDFAsync(TaiPDFRequest request, CancellationToken ct = default);
    }
}
