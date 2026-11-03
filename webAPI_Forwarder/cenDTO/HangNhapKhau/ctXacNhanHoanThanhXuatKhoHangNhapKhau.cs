using System;

namespace cenDTO.HangNhapKhau
{
	public class ctXacNhanHoanThanhXuatKhoHangNhapKhau
	{
		public long? ID { get; set; }
		public long? IDDanhMucDonVi { get; set; }
		public long? IDDanhMucChungTu { get; set; }
		public long? IDDanhMucChungTuTrangThai { get; set; }
		public string So { get; set; }
		public DateTime? NgayLap { get; set; }
		public DateTime? NgayGioXacNhanXuatKho { get; set; }
		public long? IDctLenhXuatKhoHangNhapKhau { get; set; }
		public string SoVanDon { get; set; }
		public string MoTaHangHoa { get; set; }
		public decimal? SoLuongKienXuat { get; set; }
		public long? IDDanhMucDonViTinh { get; set; }
		public string MaDanhMucDonViTinh { get; set; }
		public decimal? KhoiLuongXuat { get; set; }
		public decimal? CBMXuat { get; set; }
		public string TenChuHang { get; set; }
		public string SoContainer { get; set; }
		public string SoSeal { get; set; }
		public string MaDanhMucLoaiContainer { get; set; }
		public string TenTau { get; set; }
		public string SoChuyen { get; set; }
		public DateTime? NgayTauDen { get; set; }
		public string MaDanhMucDaiLy { get; set; }
		public string MaDanhMucHangTau { get; set; }
		public string GhiChu { get; set; }
		public long? IDDanhMucNguoiSuDungCreate { get; set; }
		public string MaDanhMucNguoiSuDungCreate { get; set; }
		public DateTime? CreateDate { get; set; }
		public long? IDDanhMucNguoiSuDungEdit { get; set; }
		public string MaDanhMucNguoiSuDungEdit { get; set; }
		public DateTime? EditDate { get; set; }

		public const string tableName = "ctXacNhanHoanThanhXuatKhoHangNhapKhau";
		public const string listProcedureNameByIDDanhMucDaiLy = "List_" + tableName + "_ByIDDanhMucDaiLy";
	}

	public class ctXacNhanHoanThanhXuatKhoHangNhapKhauListRequest
	{
		public object IDUser { get; set; }

		public string TuNgay { get; set; }

		public string DenNgay { get; set; }
	}
}
