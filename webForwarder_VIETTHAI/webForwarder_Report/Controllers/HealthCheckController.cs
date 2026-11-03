
using System;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using System.Data.SqlClient;
using webForwarder_Report.Code;


namespace WebAPI.Controllers
{
    [RoutePrefix("api/healthcheck")]
    public class HealthCheckController : ApiController
    {
        // GET: api/healthcheck/database
        [HttpGet]
        [Route("database")]
        public IHttpActionResult Database()
        {
            string connectionString = Common.ConnectionString;

            var watch = Stopwatch.StartNew();

            string maskedServer = "Unknown";
            string dbName = "Unknown";

            try
            {
                var builder =
                    new SqlConnectionStringBuilder(connectionString);

                string fullServer = builder.DataSource;
                dbName = builder.InitialCatalog;

                // Ẩn port, chỉ trả về tên server hoặc IP
                maskedServer = fullServer.Split(',', ':')[0].Trim();

                // Giới hạn thời gian kết nối tối đa 5 giây
                if (builder.ConnectTimeout <= 0 ||
                    builder.ConnectTimeout > 5)
                {
                    builder.ConnectTimeout = 5;
                }

                using (var connection =
                    new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (var cmd =
                        new SqlCommand("SELECT 1", connection))
                    {
                        cmd.CommandTimeout = 5;
                        cmd.ExecuteScalar();
                    }
                }

                watch.Stop();

                return Ok(new
                {
                    status = "Healthy",
                    connectedTo = new
                    {
                        serverIp = maskedServer,
                        databaseName = dbName
                    },
                    responseTimeMs = watch.ElapsedMilliseconds,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (SqlException ex)
            {
                watch.Stop();

                return Content(
                    HttpStatusCode.ServiceUnavailable,
                    new
                    {
                        status = "Unhealthy",
                        message = "Không thể kết nối tới cơ sở dữ liệu.",
                        connectedTo = new
                        {
                            serverIp = maskedServer,
                            databaseName = dbName
                        },
                        responseTimeMs = watch.ElapsedMilliseconds,
                        timestamp = DateTime.UtcNow,
                        errorType = "SqlException",
                        errorCode = ex.Number
                    });
            }
            catch (TimeoutException)
            {
                watch.Stop();

                return Content(
                    HttpStatusCode.ServiceUnavailable,
                    new
                    {
                        status = "Unhealthy",
                        message = "Kết nối cơ sở dữ liệu bị timeout.",
                        connectedTo = new
                        {
                            serverIp = maskedServer,
                            databaseName = dbName
                        },
                        responseTimeMs = watch.ElapsedMilliseconds,
                        timestamp = DateTime.UtcNow,
                        errorType = "TimeoutException"
                    });
            }
            catch (Exception)
            {
                watch.Stop();

                return Content(
                    HttpStatusCode.InternalServerError,
                    new
                    {
                        status = "Unhealthy",
                        message = "Lỗi khi kiểm tra kết nối cơ sở dữ liệu.",
                        connectedTo = new
                        {
                            serverIp = maskedServer,
                            databaseName = dbName
                        },
                        responseTimeMs = watch.ElapsedMilliseconds,
                        timestamp = DateTime.UtcNow,
                        errorType = "Exception"
                    });
            }
        }

        // GET: api/healthcheck/ping
        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok(new
            {
                message = "Pong!",
                serverTime = DateTime.Now
            });
        }
    }
}
