using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cenBUS.DanhMuc
{
    public static class DanhMucDaiLyBUS
    {
        public static DataTable ListForAccount(
            string ConnectionString)
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "List_DanhMucDaiLy_ForAccount";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}
