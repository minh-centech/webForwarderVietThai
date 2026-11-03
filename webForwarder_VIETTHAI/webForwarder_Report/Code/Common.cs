using System.Configuration;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace webForwarder_Report.Code
{
    public class Common
    {
        // Đọc chuỗi kết nối có tên "Database" từ web.config
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["Database"]?.ConnectionString
            ?? string.Empty;

        public static void ApplyConnection(ReportDocument rpt)
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                return;
            }

            // Tự động phân tích các thành phần từ chuỗi kết nối
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);

            string serverName = builder.DataSource;
            string databaseName = builder.InitialCatalog;
            string userId = builder.UserID;
            string password = builder.Password;

            // Đăng nhập tổng quát cho Report
            rpt.SetDatabaseLogon(userId, password, serverName, databaseName);

            // Thiết lập ConnectionInfo đồng bộ với web.config
            ConnectionInfo crConnectionInfo = new ConnectionInfo
            {
                ServerName = serverName,
                DatabaseName = databaseName,
                UserID = userId,
                Password = password
            };

            // Áp dụng cho từng bảng trong Main Report
            foreach (Table crTable in rpt.Database.Tables)
            {
                TableLogOnInfo crTableLogOnInfo = crTable.LogOnInfo;
                crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogOnInfo);
            }

            // Áp dụng cho từng bảng trong tất cả Subreports (nếu có)
            foreach (ReportDocument srSubReport in rpt.Subreports)
            {
                foreach (Table crTable in srSubReport.Database.Tables)
                {
                    TableLogOnInfo crTableLogOnInfo = crTable.LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTable.ApplyLogOnInfo(crTableLogOnInfo);
                }
            }
        }
    }
}