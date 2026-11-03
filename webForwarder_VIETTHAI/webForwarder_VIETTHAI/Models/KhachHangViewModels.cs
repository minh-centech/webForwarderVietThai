using System.ComponentModel.DataAnnotations;

namespace webForwarder_VIETTHAI.Models;

/// <summary>Trang danh sách (admin).</summary>
public class KhachHangListViewModel
{
    public string? Keyword { get; set; }
    public PagedResult<KhachHang> Result { get; set; } = new();

    /// <summary>ID đại lý -> đại lý, để cột "Đại lý" hiện mã + tên thay vì số.</summary>
    public IReadOnlyDictionary<long, DaiLy> DaiLy { get; set; } = new Dictionary<long, DaiLy>();

    public string TenDaiLy(long? id) => id is null ? "-" : DaiLy.TryGetValue(id.Value, out var dl) ? dl.HienThi : id.Value.ToString();
}

/// <summary>Form Thêm / Sửa (admin).</summary>
public class KhachHangFormViewModel
{
    public long ID { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mã khách hàng")]
    [StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
    [RegularExpression(@"^[A-Za-z0-9_\-\.]+$", ErrorMessage = "Chỉ dùng chữ, số, gạch ngang, gạch dưới, dấu chấm")]
    [Display(Name = "Mã khách hàng")]
    public string MaKH { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
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

    [Display(Name = "Loại tài khoản")]
    public int LoaiTaiKhoan { get; set; } = TaiKhoan.LoaiKhachHang;

    /// <summary>DanhMucDaiLy.ID gắn với tài khoản khách hàng (API bắt buộc khi LoaiTaiKhoan = 2; admin để trống).</summary>
    [Display(Name = "Đại lý")]
    public long? IDDanhMucDaiLy { get; set; }

    /// <summary>Danh sách đại lý cho dropdown (controller nạp từ API, không bind từ form).</summary>
    public IReadOnlyList<DaiLy> DanhSachDaiLy { get; set; } = [];

    /// <summary>Không lấy được danh sách đại lý từ API -> cho nhập ID tay.</summary>
    public string? LoiDaiLy { get; set; }

    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 ký tự")]
    [Display(Name = "Mật khẩu")]
    public string? MatKhau { get; set; }

    public bool IsNew => ID == 0;
}
