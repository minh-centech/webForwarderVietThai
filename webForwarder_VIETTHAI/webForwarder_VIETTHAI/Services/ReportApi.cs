using webForwarder_Report.Models;

namespace webForwarder_VIETTHAI.Services
{
    public class ReportApi : IReportApi
    {
        private readonly HttpClient _httpClient;

        public ReportApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<byte[]> TaiPDFAsync(TaiPDFRequest request, CancellationToken ct = default)
        {
            using var response = await _httpClient.PostAsJsonAsync("api/Report/TaiPDF", request, ct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync(ct);
        }
    }
}
