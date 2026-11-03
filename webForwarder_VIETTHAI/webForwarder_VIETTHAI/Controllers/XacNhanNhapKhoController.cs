using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webForwarder_Report.Models;
using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Models.Api;
using webForwarder_VIETTHAI.Services;

namespace webForwarder_VIETTHAI.Controllers;

[Authorize]
public class XacNhanNhapKhoController(IHangXuatKhauApi api, IReportApi reportApi) : Controller
{
    private const string XlsxMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private long CurrentId => long.TryParse(User.FindFirstValue("KhachHangId"), out var id) ? id : 0;
    public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay, CancellationToken ct)
    {
        var den = denNgay?.Date ?? DateTime.Today;
        var tu = tuNgay?.Date ?? den.AddDays(-30); 
        if (tu > den)
        {
            (tu, den) = (den, tu);
        }

        var model = new HangXuatKhauListViewModel<XacNhanNhapKhoRow> { TuNgay = tu, DenNgay = den };

        try
        {
            model.Items = await api.GetXacNhanNhapKhoAsync(CurrentId, tu, den, ct);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            TempData["Error"] = ex is HttpRequestException ? "Không kết nối được API." : "Không lấy được dữ liệu: " + ex.Message;
        }

        return View(model);
    }

    public async Task<IActionResult> XuatExcel(DateTime? tuNgay, DateTime? denNgay, CancellationToken ct)
    {
        var (tu, den) = KhoangNgay(tuNgay, denNgay);

        try
        {
            var rows = await api.GetXacNhanNhapKhoAsync(CurrentId, tu, den, ct);
            var bytes = ExcelExporter.XacNhanNhapKho(rows, tu, den);
            return File(bytes, XlsxMime, $"XacNhanNhapKho_{tu:yyyyMMdd}-{den:yyyyMMdd}.xlsx");
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            TempData["Error"] = ex is HttpRequestException ? "Không kết nối được API." : "Không xuất được Excel: " + ex.Message;
            return RedirectToAction(nameof(Index), new { tuNgay = tu.ToString("yyyy-MM-dd"), denNgay = den.ToString("yyyy-MM-dd") });
        }
    }

    private static (DateTime Tu, DateTime Den) KhoangNgay(DateTime? tuNgay, DateTime? denNgay)
    {
        var den = denNgay?.Date ?? DateTime.Today;
        var tu = tuNgay?.Date ?? den.AddDays(-30); 
        return tu > den ? (den, tu) : (tu, den);
    }

    [HttpGet]
    public async Task<IActionResult> TaiPDF(long id, bool isKMG, DateTime tuNgay, DateTime denNgay, CancellationToken ct)
    {
        try
        {
            if (tuNgay > denNgay)
                (tuNgay, denNgay) = (denNgay, tuNgay);

            var rows = await api.GetXacNhanNhapKhoAsync(CurrentId, tuNgay, denNgay, ct);

            var item = rows.FirstOrDefault(x => x.ID == id);

            if (item == null)
            {
                return Content(
                    $"Không tìm thấy dữ liệu có ID = {id} " +
                    $"trong khoảng {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}");
            }

            var request = new TaiPDFRequest
            {
                ID = item.ID,
                IDDanhMucDonVi = 1,
                IDDanhMucChungTu = 534,
                So = item.So,
                IDDanhMucDaiLy = CurrentId
            };

            var bytes = await reportApi.TaiPDFAsync(request, ct);

            return File(
                bytes,
                "application/pdf",
                $"Report_{item.So}.pdf");
        }
        catch (Exception ex)
        {
            return Content("Error: " + ex.Message);
        }
    }
}

