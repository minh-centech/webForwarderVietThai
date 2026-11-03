namespace webForwarder_VIETTHAI.Models;

public class XacNhanNhapKhoRow
{
    public long ID { get; set; }
    public string? So { get; set; }
    public DateTime? NgayLap { get; set; }
    public DateTime? NgayGioXacNhanHoanThanh { get; set; }
    public string? SoLenhNhapKhoHangXuatKhau { get; set; }
    public string? Booking { get; set; }
    public string? SoXe { get; set; }
    public string? MaDanhMucDaiLy { get; set; }
    public decimal SoLuongDangKyNhap { get; set; }
    public decimal KhoiLuongDangKyNhap { get; set; }
    public decimal CBMDangKyNhap { get; set; }
    public decimal SoLuongNhap { get; set; }
    public decimal KhoiLuongNhap { get; set; }
    public decimal CBMNhap { get; set; }
    public decimal SoLuongPallet { get; set; }
}

public class XacNhanDongHangRow
{
    public long ID { get; set; }
    public string? So { get; set; }
    public DateTime? NgayLap { get; set; }
    public DateTime? NgayGioXacNhanHoanThanh { get; set; }
    public DateTime? NgayGioBatDauDongHang { get; set; }
    public DateTime? NgayGioKetThucDongHang { get; set; }
    public string? CarrierBooking { get; set; }
    public string? SoContainer { get; set; }
    public string? MaDanhMucLoaiContainer { get; set; }
    public string? MaDanhMucDaiLy { get; set; }
    public string? Booking { get; set; }
    public string? TenDanhMucHangTau { get; set; }
    public string? MaDanhMucCangDi { get; set; }
    public string? MaDanhMucCuaLamHang { get; set; }
    public decimal SoLuongDangKyDongHang { get; set; }
    public decimal SoLuongDongHang { get; set; }
    public decimal KhoiLuongDangKyDongHang { get; set; }
    public decimal KhoiLuongDongHang { get; set; }
    public decimal CBMDangKyDongHang { get; set; }
    public decimal CBMDongHang { get; set; }
    public string? GhiChu { get; set; }
}

public sealed record DateFilterModel(DateTime TuNgay, DateTime DenNgay, int Count);

public class HangXuatKhauListViewModel<T>
{
    public DateTime TuNgay { get; set; }
    public DateTime DenNgay { get; set; }
    public IReadOnlyList<T> Items { get; set; } = [];
}
