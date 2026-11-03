using System.ComponentModel.DataAnnotations;

namespace webForwarder_VIETTHAI.Models;

/// <summary>Khách hàng tự cập nhật thông tin, kèm đổi mật khẩu</summary>
public class CapNhatThongTinViewModel
{
    public string MaKH { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập tên")]
    [StringLength(150, ErrorMessage = "Tối đa 150 ký tự")]
    [Display(Name = "Tên khách hàng")]
    public string TenKH { get; set; } = "";

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(20)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    // ----- Xác nhận & đổi mật khẩu -----
    [Required(ErrorMessage = "Nhập mật khẩu hiện tại để xác nhận thay đổi")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu hiện tại")]
    public string MatKhauHienTai { get; set; } = "";

    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu mới từ 6 ký tự")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu mới")]
    public string? MatKhauMoi { get; set; }

    [Compare(nameof(MatKhauMoi), ErrorMessage = "Mật khẩu nhập lại không khớp")]
    [DataType(DataType.Password)]
    [Display(Name = "Nhập lại mật khẩu mới")]
    public string? XacNhanMatKhauMoi { get; set; }
}

