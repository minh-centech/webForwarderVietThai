using coreDTO;
using System;

namespace cenDTO.DanhMuc
{
    public class DanhMucKhachHangForwarder : BaseDTO
    {
        public long ID { get; set; }
        public string MaKH { get; set; }
        public string MatKhau { get; set; }
        public string TenKhachHang { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public int LoaiTaiKhoan { get; set; } = 2;
        public long? IDDanhMucDaiLy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }

        public const string tableName = "DanhMucKhachHangForwarder";
        public const string listProcedureName = "List_" + tableName;
        public const string listLoginProcedureName = "List_" + tableName + "_Login";
        public const string insertProcedureName = "Insert_" + tableName;
        public const string updateProcedureName = "Update_" + tableName;
        public const string deleteProcedureName = "Delete_" + tableName;
        public const string resetPasswordProcedureName = tableName + "_ResetPassword";
    }

    public class DanhMucKhachHangForwarderLoginRequest
    {
        public string MaKH { get; set; }
        public string MatKhau { get; set; }
    }

    public class DanhMucKhachHangForwarderRequest
    {
        public long ID { get; set; }
        public string MaKH { get; set; }
        public string MatKhau { get; set; }
        public string TenKhachHang { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public int LoaiTaiKhoan { get; set; }
        public long? IDDanhMucDaiLy { get; set; }
    }
    public class DanhMucKhachHangForwarderUpdateRequest
    {
        public int ID { get; set; }
        public string MatKhau { get; set; }
        public string TenKhachHang { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }

    }
    public class DanhMucKhachHangForwarderResetPasswordRequest
    {
        public long IDAdmin { get; set; }
        public long ID { get; set; }
        public string MatKhauMoi { get; set; }
    }
}
