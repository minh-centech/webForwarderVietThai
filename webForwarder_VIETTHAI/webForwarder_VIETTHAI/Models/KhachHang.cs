namespace webForwarder_VIETTHAI.Models;

public class KhachHang
{
    public long ID { get; set; }
    public string MaKH { get; set; } = "";
    public string TenKH { get; set; } = "";
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public int LoaiTaiKhoan { get; set; } = TaiKhoan.LoaiKhachHang;
    public long? IDDanhMucDaiLy { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? EditDate { get; set; }
}

public static class TaiKhoan
{
    public const int LoaiAdmin = 1;
    public const int LoaiKhachHang = 2;

    public const string RoleAdmin = "Admin";
    public const string RoleKhachHang = "KhachHang";

    public static string RoleFrom(int? loaiTaiKhoan) => loaiTaiKhoan == LoaiAdmin ? RoleAdmin : RoleKhachHang;

    public static string TenLoai(int loaiTaiKhoan) => loaiTaiKhoan == LoaiAdmin ? "Quản trị" : "Khách hàng";
}

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int TotalPages => (int)Math.Ceiling(Total / (double)Math.Max(PageSize, 1));
}