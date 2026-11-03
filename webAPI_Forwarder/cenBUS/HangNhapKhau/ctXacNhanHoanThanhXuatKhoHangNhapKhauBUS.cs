using cenDTO.HangNhapKhau;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace cenBUS.HangNhapKhau
{
    public class ctXacNhanHoanThanhXuatKhoHangNhapKhauBUS
    {
        public static DataTable List(
            string ConnectionString,
            long IDDanhMucDonVi,
            long IDDanhMucChungTu,
            long IDDanhMucDaiLy,
            DateTime TuNgay,
            DateTime DenNgay)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(ctXacNhanHoanThanhXuatKhoHangNhapKhau.listProcedureNameByIDDanhMucDaiLy, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@IDDanhMucDonVi", SqlDbType.BigInt).Value = IDDanhMucDonVi;
                    command.Parameters.Add("@IDDanhMucChungTu", SqlDbType.BigInt).Value = IDDanhMucChungTu;
                    command.Parameters.Add("@IDDanhMucDaiLy", SqlDbType.BigInt).Value = IDDanhMucDaiLy;
                    command.Parameters.Add("@TuNgay", SqlDbType.Date).Value = TuNgay.Date;
                    command.Parameters.Add("@DenNgay", SqlDbType.Date).Value = DenNgay.Date;

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

