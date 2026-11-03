using cenBUS.DanhMuc;
using cenBUS.HangNhapKhau;
using cenDTO.HangNhapKhau;
using coreDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using GlobalVariables = WebAPI.Code.GlobalVariables;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ctXacNhanHoanThanhXuatKhoHangNhapKhauController : ControllerBase
    {
        private const long IDDanhMucDonVi = 1;
        private const long IDDanhMucChungTuXacNhanXuatKho = 625188;

        [HttpPost]
        public webAPIresponse List(ctXacNhanHoanThanhXuatKhoHangNhapKhauListRequest obj)
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

                object idDaiLy = dtUser.Columns.Contains("IDDanhMucDaiLy") ? dtUser.Rows[0]["IDDanhMucDaiLy"] : DBNull.Value;

                if (idDaiLy == null || idDaiLy == DBNull.Value)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Tài khoản chưa được gắn với đại lý nên không có dữ liệu xuất kho!";

                    return response;
                }

                long IDDanhMucDaiLy = Convert.ToInt64(idDaiLy);

                DateTime denNgay = DocNgay(obj.DenNgay) ?? DateTime.Today;
                DateTime tuNgay = DocNgay(obj.TuNgay) ?? denNgay.AddMonths(-1);

                if (tuNgay > denNgay)
                {
                    (tuNgay, denNgay) = (denNgay, tuNgay);
                }

                DataTable dt = ctXacNhanHoanThanhXuatKhoHangNhapKhauBUS.List(
                    GlobalVariables.ConnectionString,
                    IDDanhMucDonVi,
                    IDDanhMucChungTuXacNhanXuatKho,
                    IDDanhMucDaiLy,
                    tuNgay,
                    denNgay);

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
        private static DateTime? DocNgay(string? value)
        {
            return DateTime.TryParseExact(value?.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngay)
                ? ngay
                : null;
        }
    }
}

