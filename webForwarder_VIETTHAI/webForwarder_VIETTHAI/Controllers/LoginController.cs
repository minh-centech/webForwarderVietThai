using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Services;

namespace webForwarder_VIETTHAI.Controllers;

[AllowAnonymous]
public class LoginController(IAuthApi authApi, ILogger<LoginController> logger) : Controller
{
    // GET /Login
    [HttpGet]
    public IActionResult Index(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    // POST /Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LoginViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var kh = await authApi.LoginAsync(model.MaKH.Trim(), model.MatKhau, ct);
        if (kh is null)
        {
            ModelState.AddModelError(string.Empty, "Mã khách hàng hoặc mật khẩu không đúng.");
            return View(model);
        }

        // Thông tin dùng ở các trang sau được ghi vào cookie dưới dạng claim
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, kh.MaKH),
            new(ClaimTypes.GivenName, kh.TenKH),
            new(ClaimTypes.Role, kh.Role),
            new("KhachHangId", kh.Id.ToString()),
            new("LoginAt", DateTime.Now.ToString("O")),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = model.RememberMe });

        logger.LogInformation("Khách hàng {MaKH} đăng nhập.", kh.MaKH);

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    // POST /Login/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }

    // GET /Login/QuenMatKhau
    [HttpGet]
    public IActionResult QuenMatKhau() => View();

    [HttpGet]
    public IActionResult KhongCoQuyen() => View();
}
