using System.ComponentModel.DataAnnotations;

namespace webForwarder_VIETTHAI.Models;

public class DatLaiMatKhauViewModel
{
    public long ID { get; set; }
    public string MaKH { get; set; } = "";
    public string TenKH { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 ký tự")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu mới")]
    public string MatKhauMoi { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu mới")]
    [Compare(nameof(MatKhauMoi), ErrorMessage = "Mật khẩu nhập lại không khớp")]
    [DataType(DataType.Password)]
    [Display(Name = "Nhập lại mật khẩu mới")]
    public string XacNhan { get; set; } = "";
}
