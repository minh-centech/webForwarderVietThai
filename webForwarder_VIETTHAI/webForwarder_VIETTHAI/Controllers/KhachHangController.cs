using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Models.Api;
using webForwarder_VIETTHAI.Services;

namespace webForwarder_VIETTHAI.Controllers;

/// <summary>Khách hàng đăng nhập tự cập nhật thông tin (kèm đổi mật khẩu).</summary>
[Authorize]
public class KhachHangController(IKhachHangApi api, IAuthApi authApi) : Controller
{
    private long CurrentId => long.TryParse(User.FindFirstValue("KhachHangId"), out var id) ? id : 0;

    // GET /KhachHang/CapNhat
    [HttpGet]
    public async Task<IActionResult> CapNhat(CancellationToken ct)
    {
        if (User.IsInRole(TaiKhoan.RoleAdmin))
        {
            return RedirectToAction("Sua", "QuanLyKhachHang", new { id = CurrentId });
        }

        var kh = await TryGetCurrentAsync(ct);
        if (kh is null)
        {
            TempData["Error"] = "Chưa lấy được thông tin tài khoản từ API.";
            return RedirectToAction("Index", "Home");
        }

        return View(new CapNhatThongTinViewModel
        {
            MaKH = kh.MaKH,
            TenKH = kh.TenKH,
            Email = kh.Email,
            SoDienThoai = kh.SoDienThoai,
        });
    }

    // POST /KhachHang/CapNhat
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CapNhat(CapNhatThongTinViewModel model, CancellationToken ct)
    {
        if (User.IsInRole(TaiKhoan.RoleAdmin))
        {
            return RedirectToAction("Sua", "QuanLyKhachHang", new { id = CurrentId });
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var kh = await TryGetCurrentAsync(ct);
        if (kh is null)
        {
            ModelState.AddModelError(string.Empty, "Chưa lấy được thông tin tài khoản từ API.");
            return View(model);
        }

        // Xác nhận mật khẩu hiện tại bằng chính endpoint Login
        var verified = await authApi.LoginAsync(kh.MaKH, model.MatKhauHienTai, ct);
        if (verified is null)
        {
            ModelState.AddModelError(nameof(model.MatKhauHienTai), "Mật khẩu hiện tại không đúng.");
            return View(model);
        }

        // Khách được sửa 3 trường; MaKH, LoaiTaiKhoan giữ nguyên
        kh.TenKH = model.TenKH.Trim();
        kh.Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim();
        kh.SoDienThoai = string.IsNullOrWhiteSpace(model.SoDienThoai) ? null : model.SoDienThoai.Trim();

        // Có mật khẩu mới thì đổi; không thì gửi lại mật khẩu hiện tại
        var doiMatKhau = !string.IsNullOrEmpty(model.MatKhauMoi);
        var matKhauGui = doiMatKhau ? model.MatKhauMoi! : model.MatKhauHienTai;

        try
        {
            await api.UpdateAsync(kh, matKhauGui, ct);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, ex is HttpRequestException ? "Không kết nối được API." : ex.Message);
            return View(model);
        }

        await RefreshDisplayNameAsync(kh.TenKH);
        TempData["Success"] = doiMatKhau ? "Đã cập nhật thông tin và đổi mật khẩu." : "Đã cập nhật thông tin.";
        return RedirectToAction("Index", "Home");
    }

    private async Task<KhachHang?> TryGetCurrentAsync(CancellationToken ct)
    {
        try
        {
            return await api.GetByIdAsync(CurrentId, ct);
        }
        catch (Exception ex) when (ex is ApiException or HttpRequestException)
        {
            return null;
        }
    }

    // Cập nhật lại tên hiển thị trong cookie sau khi khách đổi tên
    private async Task RefreshDisplayNameAsync(string tenKH)
    {
        var claims = User.Claims.Where(c => c.Type != ClaimTypes.GivenName).ToList();
        claims.Add(new Claim(ClaimTypes.GivenName, tenKH));
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var props = (await HttpContext.AuthenticateAsync()).Properties ?? new AuthenticationProperties();
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), props);
    }
}