using System.ComponentModel.DataAnnotations;

namespace webForwarder_VIETTHAI.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập mã khách hàng")]
    [Display(Name = "Mã khách hàng")]
    public string MaKH { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = "";

    public bool RememberMe { get; set; } = false;

    public string? ReturnUrl { get; set; }

    // Token phản hồi từ Cloudflare Turnstile
    public string? CfTurnstileResponse { get; set; }
}

