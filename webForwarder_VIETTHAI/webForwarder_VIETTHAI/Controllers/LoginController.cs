using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Services;

namespace webForwarder_VIETTHAI.Controllers;

[AllowAnonymous]
public class LoginController(
    IAuthApi authApi,
    ILogger<LoginController> logger,
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory) : Controller
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
        // 1. Lấy token Turnstile từ form submit
        var turnstileToken = Request.Form["cf-turnstile-response"].ToString();

        // 2. Xác thực bảo mật với Cloudflare trước
        var isTurnstileValid = await VerifyTurnstileAsync(turnstileToken, ct);
        if (!isTurnstileValid)
        {
            ModelState.AddModelError(string.Empty, "Xác thực bảo mật chống bot không hợp lệ. Vui lòng thử lại.");
            return View(model);
        }

        // 3. Kiểm tra tính hợp lệ của Model
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // 4. Kiểm tra tài khoản qua authApi
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

    // Hàm phụ trợ xác thực Cloudflare Turnstile
    private async Task<bool> VerifyTurnstileAsync(string? token, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            logger.LogWarning("Turnstile token trống khi gửi request đăng nhập.");
            return false;
        }

        var secretKey = configuration["CloudflareTurnstile:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            logger.LogError("Chưa cấu hình 'CloudflareTurnstile:SecretKey' trong appsettings.json.");
            return false;
        }

        var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();

        try
        {
            var client = httpClientFactory.CreateClient();
            var postData = new Dictionary<string, string>
            {
                { "secret", secretKey },
                { "response", token },
                { "remoteip", remoteIp ?? string.Empty }
            };

            var response = await client.PostAsync(
                "https://challenges.cloudflare.com/turnstile/v0/siteverify",
                new FormUrlEncodedContent(postData),
                ct);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Gửi request xác thực Turnstile thất bại với mã lỗi HTTP {StatusCode}", response.StatusCode);
                return false;
            }

            var json = await response.Content.ReadAsStringAsync(ct);
            var result = JsonSerializer.Deserialize<TurnstileResponse>(json);

            return result?.Success == true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Lỗi xảy ra trong quá trình xác thực Turnstile với Cloudflare.");
            return false;
        }
    }

    private class TurnstileResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error-codes")]
        public string[]? ErrorCodes { get; set; }
    }
}