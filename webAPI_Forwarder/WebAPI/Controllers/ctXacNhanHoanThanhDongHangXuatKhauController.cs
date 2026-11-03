using cenBUS.DanhMuc;
using cenBUS.HangXuatKhau;
using cenDTO.HangXuatKhau;
using coreDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using GlobalVariables = WebAPI.Code.GlobalVariables;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ctXacNhanHoanThanhDongHangXuatKhauController : ControllerBase
    {
        [HttpPost]
        public webAPIresponse List(ctXacNhanHoanThanhDongHangXuatKhauListRequest obj)
        {
            webAPIresponse response = new webAPIresponse();

            try
            {
                if (obj == null)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Dữ liệu không được để trống!";
                    return response;
                }

                DataTable dtUser = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, obj.IDUser);

                if (dtUser == null || dtUser.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Tài khoản không tồn tại!";

                    return response;
                }

                //int loaiTaiKhoan = Convert.ToInt32(dtUser.Rows[0]["LoaiTaiKhoan"]);

                //if (loaiTaiKhoan != 2)
                //{
                //    response.Status = 1;
                //    response.Data = "";
                //    response.ErrorMsg = "Chỉ tài khoản khách hàng được xem dữ liệu!";

                //    return response;
                //}

                long IDDanhMucDaiLy = Convert.ToInt64(dtUser.Rows[0]["IDDanhMucDaiLy"]);
                string tuNgay = obj.TuNgay;
                string denNgay = obj.DenNgay;

                if (string.IsNullOrWhiteSpace(tuNgay))
                {
                    tuNgay = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }

                if (string.IsNullOrWhiteSpace(denNgay))
                {
                    denNgay = DateTime.Now.ToString("yyyy-MM-dd");
                }

                DataTable dt = ctXacNhanHoanThanhDongHangXuatKhauBUS.List(GlobalVariables.ConnectionString, 1, 552, IDDanhMucDaiLy, tuNgay, denNgay);

                response.Status = 0;
                response.Data = JsonConvert.SerializeObject(dt);
                response.ErrorMsg = "";

                return response;
            }
            catch (Exception ex)
            {
                response.Status = 1;
                response.Data = "";
                response.ErrorMsg = ex.Message;

                return response;
            }
        }
    }
}