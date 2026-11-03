using cenBUS.DanhMuc;
using coreDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WebAPI.Code;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DanhMucDaiLyController : ControllerBase
    {
        [HttpGet]
        public webAPIresponse List()
        {
            webAPIresponse response = new webAPIresponse();

            try
            {
                DataTable dt = DanhMucDaiLyBUS.ListForAccount(GlobalVariables.ConnectionString);

                var data =
                    dt.AsEnumerable()
                      .Select(x => new
                      {
                          ID = x.Field<long>("ID"),
                          Ma = x.Field<string>("Ma"),
                          Ten = x.Field<string>("Ten")
                      })
                      .ToList();

                response.Status = 0;
                response.Data =
                    JsonConvert.SerializeObject(data);
                response.ErrorMsg =
                    string.Empty;
            }
            catch (Exception ex)
            {
                response.Status = 1;
                response.Data = string.Empty;
                response.ErrorMsg = ex.Message;
            }

            return response;
        }
    }
}

