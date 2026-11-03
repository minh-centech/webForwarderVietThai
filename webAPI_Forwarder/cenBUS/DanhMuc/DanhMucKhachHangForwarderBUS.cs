using cenDTO.DanhMuc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace cenBUS.DanhMuc
{
    public class DanhMucKhachHangForwarderBUS
    {
        public static DataTable ListLogin(string ConnectionString, object Ma)
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = DanhMucKhachHangForwarder.listLoginProcedureName;
                    cmd.Parameters.Add(new SqlParameter("@MaKH", Ma));

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static DataTable List(string ConnectionString, object ID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = DanhMucKhachHangForwarder.listProcedureName;
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID ?? DBNull.Value;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
        public static long Insert(string ConnectionString, string MaKH, string MatKhau, string TenKH, string Email, string SoDienThoai, int LoaiTaiKhoan, long? IDDanhMucDaiLy)
        {
            long ID = 0;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = DanhMucKhachHangForwarder.insertProcedureName;

                    SqlParameter pID = new SqlParameter("@ID", SqlDbType.BigInt);
                    pID.Direction = ParameterDirection.InputOutput;
                    pID.Value = 0;

                    SqlParameter pCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
                    pCreateDate.Direction = ParameterDirection.InputOutput;
                    pCreateDate.Value = DBNull.Value;

                    cmd.Parameters.Add(pID);
                    cmd.Parameters.Add(new SqlParameter("@MaKH", SqlDbType.NVarChar, 128) { Value = MaKH });
                    cmd.Parameters.Add(new SqlParameter("@MatKhau", SqlDbType.NVarChar, 500) { Value = MatKhau });
                    cmd.Parameters.Add(new SqlParameter("@TenKH", SqlDbType.NVarChar, 255) { Value = TenKH });
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = Email });
                    cmd.Parameters.Add(new SqlParameter("@SoDienThoai", SqlDbType.NVarChar, 50) { Value = SoDienThoai });
                    cmd.Parameters.Add(new SqlParameter("@LoaiTaiKhoan", SqlDbType.Int) { Value = LoaiTaiKhoan });
                    cmd.Parameters.Add(new SqlParameter("@IDDanhMucDaiLy", SqlDbType.BigInt) { Value = IDDanhMucDaiLy.HasValue ? (object)IDDanhMucDaiLy.Value : DBNull.Value });
                    cmd.Parameters.Add(pCreateDate);

                    cmd.ExecuteNonQuery();

                    if (pID.Value != DBNull.Value)
                    {
                        ID = Convert.ToInt64(pID.Value);
                    }
                }
            }

            return ID;
        }

        public static bool Update(string ConnectionString, long ID, string MatKhau, string TenKH, string Email, string SoDienThoai)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = DanhMucKhachHangForwarder.updateProcedureName;

                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
                    cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, -1).Value = (object)MatKhau ?? DBNull.Value;
                    cmd.Parameters.Add("@TenKH", SqlDbType.NVarChar, 150).Value = (object)TenKH ?? DBNull.Value;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = (object)Email ?? DBNull.Value;
                    cmd.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 10).Value = (object)SoDienThoai ?? DBNull.Value;
                    cmd.Parameters.Add("@EditDate", SqlDbType.DateTime).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
        }
        public static bool Delete(string ConnectionString, long ID)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = DanhMucKhachHangForwarder.deleteProcedureName;
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
        }
        public static void ResetPassword(string ConnectionString, long ID, string MatKhau)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = DanhMucKhachHangForwarder.resetPasswordProcedureName;
                    cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
                    cmd.Parameters.Add("@MatKhau", SqlDbType.NVarChar, -1).Value = MatKhau;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}