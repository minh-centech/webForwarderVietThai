using System;

namespace cenDTO.HangXuatKhau
{
    public class ctXacNhanHoanThanhDongHangXuatKhau
    {
        public long? ID { get; set; }
        public object IDDanhMucDonVi { get; set; }
        public object IDDanhMucChungTu { get; set; }
        public object IDDanhMucChungTuTrangThai { get; set; }
        public string So { get; set; }
        public DateTime? NgayLap { get; set; }
        public DateTime? NgayGioXacNhanHoanThanh { get; set; }
        public DateTime? NgayGioBatDauDongHang { get; set; }
        public DateTime? NgayGioKetThucDongHang { get; set; }
        public object IDctContainerHangXuatKhauVaoKho { get; set; }
        public object IDctDongHangXuatKhau { get; set; }
        public string CarrierBooking { get; set; }
        public string SoContainer { get; set; }
        public string MaDanhMucLoaiContainer { get; set; }
        public string MaDanhMucDaiLy { get; set; }
        public string Booking { get; set; }
        public string TenDanhMucHangTau { get; set; }
        public string MaDanhMucCangDi { get; set; }
        public object IDDanhMucCuaLamHang { get; set; }
        public string MaDanhMucCuaLamHang { get; set; }
        public object SoLuongDangKyDongHang { get; set; }
        public object SoLuongDongHang { get; set; }
        public object KhoiLuongDangKyDongHang { get; set; }
        public object KhoiLuongDongHang { get; set; }
        public object CBMDangKyDongHang { get; set; }
        public object CBMDongHang { get; set; }
        public string GhiChu { get; set; }
        public object IDDanhMucNguoiSuDungCreate { get; set; }
        public string MaDanhMucNguoiSuDungCreate { get; set; }
        public DateTime? CreateDate { get; set; }
        public object IDDanhMucNguoiSuDungEdit { get; set; }
        public string MaDanhMucNguoiSuDungEdit { get; set; }
        public DateTime? EditDate { get; set; }

        public const string tableName = "ctXacNhanHoanThanhDongHangXuatKhau";
        public const string listDisplayProcedureName = "List_" + tableName + "_Display";
        public const string listProcedureNameByIDDanhMucDaiLy = "List_" + tableName + "_ByIDDanhMucDaiLy";
    }

    public class ctXacNhanHoanThanhDongHangXuatKhauListRequest
    {
        public string IDUser { get; set; }
        public long IDDanhMucDonVi { get; set; }
        public long IDDanhMucChungTu { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
    }
}