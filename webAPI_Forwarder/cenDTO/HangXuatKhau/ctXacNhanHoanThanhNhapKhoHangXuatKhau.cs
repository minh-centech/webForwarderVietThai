using System;

namespace cenDTO.HangXuatKhau
{
    public class ctXacNhanHoanThanhNhapKhoHangXuatKhau
    {
        public long? ID { get; set; }
        public long? IDDanhMucDonVi { get; set; }
        public long? IDDanhMucChungTu { get; set; }
        public long? IDDanhMucChungTuTrangThai { get; set; }
        public string So { get; set; }
        public DateTime? NgayLap { get; set; }
        public DateTime? NgayGioXacNhanHoanThanh { get; set; }
        public long? IDctLenhNhapKhoHangXuatKhau { get; set; }
        public string SoLenhNhapKhoHangXuatKhau { get; set; }
        public long? IDDanhMucDaiLy { get; set; }
        public string MaDanhMucDaiLy { get; set; }
        public string SoToKhai { get; set; }
        public string Booking { get; set; }
        public string TenNhaMay { get; set; }
        public string PO { get; set; }
        public string Item { get; set; }
        public string Code { get; set; }
        public string Acc { get; set; }
        public string SoXe { get; set; }
        public string MaDanhMucCuaLamHang { get; set; }
        public int? SoLuongDangKyNhap { get; set; }
        public int? SoLuongNhap { get; set; }
        public decimal? KhoiLuongDangKyNhap { get; set; }
        public decimal? KhoiLuongNhap { get; set; }
        public decimal? CBMDangKyNhap { get; set; }
        public decimal? CBMNhap { get; set; }
        public int? SoLuongPallet { get; set; }
        public string GhiChu { get; set; }
        public long? IDDanhMucNguoiSuDungCreate { get; set; }
        public string MaDanhMucNguoiSuDungCreate { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? IDDanhMucNguoiSuDungEdit { get; set; }
        public string MaDanhMucNguoiSuDungEdit { get; set; }
        public DateTime? EditDate { get; set; }

        public const string tableName = "ctXacNhanHoanThanhNhapKhoHangXuatKhau";
        public const string listDisplayProcedureName = "List_" + tableName + "_Display";
        public const string listProcedureNameByIDDanhMucDaiLy = "List_" + tableName + "_ByIDDanhMucDaiLy";
    }


    public class ctXacNhanHoanThanhNhapKhoHangXuatKhauListRequest
    {
        public object IDUser { get; set; }
        public long IDDanhMucDonVi { get; set; }
        public long IDDanhMucChungTu { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
    }
}