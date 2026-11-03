using cenDTO.HangXuatKhau;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace cenBUS.HangXuatKhau
{
    public class ctXacNhanHoanThanhDongHangXuatKhauBUS
    {
        public static DataTable List(
            string ConnectionString,
            long IDDanhMucDonVi,
            long IDDanhMucChungTuXacNhanDongHang,
            long IDDanhMucDaiLy,
            object TuNgay,
            object DenNgay)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(ctXacNhanHoanThanhDongHangXuatKhau.listProcedureNameByIDDanhMucDaiLy, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    object valueIDDanhMucDaiLy = IDDanhMucDaiLy == 0 ? (object)DBNull.Value : IDDanhMucDaiLy;

                    command.Parameters.Add("@IDDanhMucDaiLy", SqlDbType.BigInt).Value = valueIDDanhMucDaiLy;
                    command.Parameters.Add("@IDDanhMucDonVi", SqlDbType.BigInt).Value = IDDanhMucDonVi;
                    command.Parameters.Add("@IDDanhMucChungTu", SqlDbType.BigInt).Value = IDDanhMucChungTuXacNhanDongHang;
                    command.Parameters.Add("@TuNgay", SqlDbType.Date).Value = TuNgay ?? DBNull.Value;
                    command.Parameters.Add("@DenNgay", SqlDbType.Date).Value = DenNgay ?? DBNull.Value;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}