using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Models.Api;
using webForwarder_VIETTHAI.Services;

namespace webForwarder_VIETTHAI.Controllers;

[Authorize(Roles = TaiKhoan.RoleAdmin)]
public class QuanLyKhachHangController(IKhachHangApi api, IDaiLyApi daiLyApi, ILogger<QuanLyKhachHangController> logger) : Controller
{
    private const int PageSize = 10;

    public async Task<IActionResult> Index(string? keyword, int page = 1, CancellationToken ct = default)
    {
        page = Math.Max(page, 1);
        var model = new KhachHangListViewModel { Keyword = keyword };
        try
        {
            model.Result = await api.ListAsync(keyword, page, PageSize, ct);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            TempData["Error"] = "Không lấy được danh sách từ API: " + ex.Message;
        }
        try
        {
            model.DaiLy = (await daiLyApi.ListAsync(ct)).ToDictionary(x => x.ID);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            logger.LogWarning("Không lấy được danh mục đại lý: {Message}", ex.Message);
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Them(CancellationToken ct) => View("Form", await NapDaiLyAsync(new KhachHangFormViewModel(), ct));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Them(KhachHangFormViewModel model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(model.MatKhau))
        {
            ModelState.AddModelError(nameof(model.MatKhau), "Vui lòng nhập mật khẩu cho tài khoản mới");
        }

        if (model.LoaiTaiKhoan == TaiKhoan.LoaiKhachHang && model.IDDanhMucDaiLy is null or <= 0)
        {
            ModelState.AddModelError(nameof(model.IDDanhMucDaiLy), "Tài khoản khách hàng phải gắn với một đại lý (nhập ID trong danh mục đại lý)");
        }

        if (model.LoaiTaiKhoan == TaiKhoan.LoaiAdmin)
        {
            model.IDDanhMucDaiLy = null; 
        }

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        try
        {
            var id = await api.InsertAsync(ToEntity(model), model.MatKhau!, ct);
            logger.LogInformation("Admin {User} thêm khách hàng {MaKH} (ID {Id}).", User.Identity?.Name, model.MaKH, id);
            TempData["Success"] = $"Đã thêm khách hàng {model.MaKH}.";
            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Form", model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Sua(long id, CancellationToken ct)
    {
        var kh = await api.GetByIdAsync(id, ct);
        if (kh is null)
        {
            return NotFound();
        }

        return View("Form", new KhachHangFormViewModel
        {
            ID = kh.ID,
            MaKH = kh.MaKH,
            TenKH = kh.TenKH,
            Email = kh.Email,
            SoDienThoai = kh.SoDienThoai,
            LoaiTaiKhoan = kh.LoaiTaiKhoan,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sua(long id, KhachHangFormViewModel model, CancellationToken ct)
    {
        model.ID = id;

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        var matKhauMoi = string.IsNullOrEmpty(model.MatKhau) ? null : model.MatKhau;

        try
        {
            await api.UpdateAsync(ToEntity(model), matKhauMoi, ct);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, ex is HttpRequestException ? "Không kết nối được API." : ex.Message);
            return View("Form", model);
        }

        TempData["Success"] = matKhauMoi is null
            ? $"Đã cập nhật khách hàng {model.MaKH}."
            : $"Đã cập nhật khách hàng {model.MaKH} và đặt mật khẩu mới.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Xoa(long id, string? maKH, CancellationToken ct)
    {
        if (id.ToString() == User.FindFirst("KhachHangId")?.Value)
        {
            TempData["Error"] = "Không thể xoá tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            await api.DeleteAsync(id, ct);
            logger.LogWarning("Admin {User} xoá khách hàng {MaKH} (ID {Id}).", User.Identity?.Name, maKH, id);
            TempData["Success"] = $"Đã xoá khách hàng {maKH}.";
        }
        catch (ApiException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<KhachHangFormViewModel> NapDaiLyAsync(KhachHangFormViewModel model, CancellationToken ct)
    {
        try
        {
            model.DanhSachDaiLy = await daiLyApi.ListAsync(ct);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            model.LoiDaiLy = ex is HttpRequestException ? "Không kết nối được API đại lý." : ex.Message;
        }

        return model;
    }

    private static KhachHang ToEntity(KhachHangFormViewModel m) => new()
    {
        ID = m.ID,
        MaKH = m.MaKH.Trim(),
        TenKH = m.TenKH.Trim(),
        Email = string.IsNullOrWhiteSpace(m.Email) ? null : m.Email.Trim(),
        SoDienThoai = string.IsNullOrWhiteSpace(m.SoDienThoai) ? null : m.SoDienThoai.Trim(),
        LoaiTaiKhoan = m.LoaiTaiKhoan,
        IDDanhMucDaiLy = m.IDDanhMucDaiLy
    };
    [HttpGet]
    public async Task<IActionResult> DatLaiMatKhau(long id, CancellationToken ct)
    {
        var kh = await api.GetByIdAsync(id, ct);
        if (kh is null)
        {
            return NotFound();
        }

        return View(new DatLaiMatKhauViewModel { ID = kh.ID, MaKH = kh.MaKH, TenKH = kh.TenKH });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DatLaiMatKhau(long id, DatLaiMatKhauViewModel model, CancellationToken ct)
    {
        model.ID = id;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await api.ResetPasswordAsync(id, model.MatKhauMoi, ct);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, ex is HttpRequestException ? "Không kết nối được API." : ex.Message);
            return View(model);
        }

        logger.LogWarning("Admin {User} đặt lại mật khẩu cho {MaKH}.", User.Identity?.Name, model.MaKH);
        TempData["Success"] = $"Đã đặt mật khẩu mới cho {model.MaKH}";
        return RedirectToAction(nameof(Index));
    }
}
