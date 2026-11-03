using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Models.Api;
using webForwarder_VIETTHAI.Services;

namespace webForwarder_VIETTHAI.Controllers;

[Authorize(Roles = TaiKhoan.RoleKhachHang)]
public class XacNhanXuatKhoController(IHangNhapKhauApi api) : Controller
{
    private const string XlsxMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private long CurrentId => long.TryParse(User.FindFirstValue("KhachHangId"), out var id) ? id : 0;

    public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay, CancellationToken ct)
    {
        var (tu, den) = KhoangNgay(tuNgay, denNgay);
        var model = new HangXuatKhauListViewModel<XacNhanXuatKhoRow> { TuNgay = tu, DenNgay = den };

        try
        {
            model.Items = await api.GetXacNhanXuatKhoAsync(CurrentId, tu, den, ct);
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
            var rows = await api.GetXacNhanXuatKhoAsync(CurrentId, tu, den, ct);
            var bytes = ExcelExporter.XacNhanXuatKho(rows, tu, den);
            return File(bytes, XlsxMime, $"XacNhanXuatKho_{tu:yyyyMMdd}-{den:yyyyMMdd}.xlsx");
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
}

