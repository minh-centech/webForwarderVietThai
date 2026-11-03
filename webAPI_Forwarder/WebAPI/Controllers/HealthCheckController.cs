using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using GlobalVariables = WebAPI.Code.GlobalVariables;
namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        // GET: api/healthcheck/database
        [HttpGet("database")]
        public IActionResult Database()
        {
            string connectionString =GlobalVariables.ConnectionString;
            var watch = Stopwatch.StartNew();

            string maskedServer = "Unknown";
            string dbName = "Unknown";

            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);

                string fullServer = builder.DataSource;
                dbName = builder.InitialCatalog;

                // Hide port, only return server name/IP
                maskedServer = fullServer.Split(',', ':')[0].Trim();

                // Force connection timeout to 5 seconds
                if (builder.ConnectTimeout <= 0 || builder.ConnectTimeout > 5)
                {
                    builder.ConnectTimeout = 5;
                }

                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    // Test database query
                    using (var cmd = new SqlCommand("SELECT 1", connection))
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

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
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

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
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

                return StatusCode(StatusCodes.Status500InternalServerError, new
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
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new
            {
                message = "Pong!",
                serverTime = DateTime.Now
            });
        }
    }
}
