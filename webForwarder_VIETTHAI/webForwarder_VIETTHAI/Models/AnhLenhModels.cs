namespace webForwarder_VIETTHAI.Models;

/// <summary>Loại lệnh có ảnh chụp: nhập kho / đóng hàng / rút hàng.</summary>
public enum LoaiLenh
{
    LenhNhapHangXuatKhau,
    LenhDongHangXuatKhau,
    LenhRutHangXuatKhau,
    LenhNhapHangNhapKhau,

}
public static class LoaiLenhInfo
{
    public static readonly LoaiLenh[] TatCa = [LoaiLenh.LenhNhapHangXuatKhau, LoaiLenh.LenhDongHangXuatKhau, LoaiLenh.LenhRutHangXuatKhau, LoaiLenh.LenhNhapHangNhapKhau];

    public static string Ten(LoaiLenh loai) => loai switch
    {
        LoaiLenh.LenhNhapHangXuatKhau => "Lệnh nhập hàng xuất khẩu",
        LoaiLenh.LenhDongHangXuatKhau => "Lệnh đóng hàng xuất khẩu",
        LoaiLenh.LenhRutHangXuatKhau => "Lệnh rút hàng xuất khẩu",
        LoaiLenh.LenhNhapHangNhapKhau => "Lệnh nhập hàng nhập khẩu",
        _ => loai.ToString(),
    };

    /// <summary>Giá trị dùng trên URL: ?loai=nhap|dong|rut</summary>
    public static string Ma(LoaiLenh loai) => loai switch
    {
        LoaiLenh.LenhNhapHangXuatKhau => "nhap",
        LoaiLenh.LenhDongHangXuatKhau => "dong",
        LoaiLenh.LenhRutHangXuatKhau => "rut",
        LoaiLenh.LenhNhapHangNhapKhau => "nhapnk",
        _ => "nhap",
    };

    public static LoaiLenh Parse(string? ma) => (ma ?? "").Trim().ToLowerInvariant() switch
    {
        "dong" => LoaiLenh.LenhDongHangXuatKhau,
        "rut" => LoaiLenh.LenhRutHangXuatKhau,
        "nhapnk" => LoaiLenh.LenhNhapHangNhapKhau,
        _ => LoaiLenh.LenhNhapHangXuatKhau,
    };

    public static string ApiPath(LoaiLenh loai) => loai switch
    {
        LoaiLenh.LenhNhapHangXuatKhau => "api/ctChupAnhLenhNhapHangXuatKhau/ListAnhTheoLenh",
        LoaiLenh.LenhDongHangXuatKhau => "api/ctChupAnhLenhDongHangXuatKhau/ListAnhTheoLenh",
        LoaiLenh.LenhRutHangXuatKhau => "api/ctChupAnhLenhRutHangXuatKhau/ListAnhTheoLenh",
        LoaiLenh.LenhNhapHangNhapKhau => "api/ctChupAnhLenhNhapHangNhapKhau/ListAnhTheoLenh",
        
        _ => throw new ArgumentOutOfRangeException(nameof(loai)),
    };

    /// <summary>Tên tham số form gửi lên API.</summary>
    public static string TenThamSo(LoaiLenh loai) => loai switch
    {
        LoaiLenh.LenhNhapHangXuatKhau => "SoLenhNhap",
        LoaiLenh.LenhDongHangXuatKhau => "SoLenhDong",
        LoaiLenh.LenhRutHangXuatKhau => "SoLenhRut",
        LoaiLenh.LenhNhapHangNhapKhau => "SoLenhNhap",
        _ => throw new ArgumentOutOfRangeException(nameof(loai)),
    };
}

/// <summary>"Data" của API ảnh.</summary>
public sealed class AnhLenhResult
{
    public List<string> FilePaths { get; set; } = [];
}

public sealed class XemAnhViewModel
{
    public LoaiLenh Loai { get; set; } = LoaiLenh.LenhNhapHangXuatKhau;
    public string? So { get; set; }

    /// <summary>Đã bấm tìm (để phân biệt "chưa tìm" với "không có ảnh").</summary>
    public bool DaTim { get; set; }
    public string? Loi { get; set; }

    /// <summary>URL ảnh gốc (trên server cenCFS-ITS).</summary>
    public IReadOnlyList<string> Anh { get; set; } = [];
}
