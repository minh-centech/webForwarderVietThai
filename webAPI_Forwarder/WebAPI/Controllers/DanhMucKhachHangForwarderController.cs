using cenBUS.DanhMuc;
using cenDTO.DanhMuc;
using coreDTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Web.Http.Cors;
using GlobalVariables = WebAPI.Code.GlobalVariables;

namespace WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
     [ApiController]
    [Route("api/[controller]/[action]")]
    public class DanhMucKhachHangForwarderController : ControllerBase
    {
        [HttpPost]
        public webAPIresponse Login(DanhMucKhachHangForwarderLoginRequest objLogin)
        {
            webAPIresponse response = new webAPIresponse();
            string ErrMsg = string.Empty;

            try
            {
                DataTable dtDanhMucKhachHangForwarder = DanhMucKhachHangForwarderBUS.ListLogin(GlobalVariables.ConnectionString, objLogin.MaKH.Trim());

                if (dtDanhMucKhachHangForwarder != null && dtDanhMucKhachHangForwarder.Rows.Count == 1)
                {
                    DataRow row = dtDanhMucKhachHangForwarder.Rows[0];
                    string rawPassword = objLogin.MatKhau;
                    string encryptedPassword = coreCommon.coreCommon.EncryptString(rawPassword);

                    string encryptedPasswordDB = row["MatKhau"]?.ToString() ?? "";

                    if (encryptedPassword == encryptedPasswordDB)
                    {
                        long? iDDanhMucDaiLy = null;
                        if (row["IDDanhMucDaiLy"] != DBNull.Value && row["IDDanhMucDaiLy"] != null)
                        {
                            iDDanhMucDaiLy = Convert.ToInt64(row["IDDanhMucDaiLy"]);
                        }
                        var x = new
                        {
                            ID = row["ID"].ToString(),
                            MaKH = row["MaKH"].ToString(),
                            TenKhachHang = row["TenKH"].ToString(),
                            Email = row["Email"].ToString(),
                            SoDienThoai = row["SoDienThoai"].ToString(),
                            LoaiTaiKhoan = row["LoaiTaiKhoan"].ToString(),
                            IDDanhMucDaiLy = iDDanhMucDaiLy
                        };

                        response.Status = 0;
                        response.Data = JsonConvert.SerializeObject(x);
                        response.ErrorMsg = String.Empty;
                    }
                    else
                    {
                        response.Status = 1;
                        response.Data = String.Empty;
                        response.ErrorMsg = "Mật khẩu không đúng!";
                    }
                }
                else
                {
                    response.Status = 1;
                    response.Data = String.Empty;
                    response.ErrorMsg = "Tài khoản không đúng!";
                }
            }
            catch (Exception ex)
            {
                response.Status = 1;
                response.Data = String.Empty;
                response.ErrorMsg = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public webAPIresponse Insert(DanhMucKhachHangForwarderRequest request)
        {
            webAPIresponse response = new webAPIresponse();

            try
            {
                if (request == null)
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Dữ liệu không được để trống!";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.MaKH))
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Mã khách hàng không được bỏ trống!";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.MatKhau))
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Mật khẩu không được bỏ trống!";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.TenKhachHang))
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Tên khách hàng không được bỏ trống!";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Email không được bỏ trống!";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.SoDienThoai))
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Số điện thoại không được bỏ trống!";
                    return response;
                }

                if (request.LoaiTaiKhoan != 1 && request.LoaiTaiKhoan != 2)
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Loại tài khoản không hợp lệ!";
                    return response;
                }

                if (request.LoaiTaiKhoan == 1)
                {
                    request.IDDanhMucDaiLy = null;
                }
                if (request.LoaiTaiKhoan == 2 && !request.IDDanhMucDaiLy.HasValue)
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg =
                        "Tài khoản khách hàng phải được liên kết với đại lý!";

                    return response;
                }

                string encryptedPassword = coreCommon.coreCommon.EncryptString(request.MatKhau);

                long ID = DanhMucKhachHangForwarderBUS.Insert(
                            GlobalVariables.ConnectionString,
                            request.MaKH,
                            encryptedPassword,
                            request.TenKhachHang,
                            request.Email,
                            request.SoDienThoai,
                            request.LoaiTaiKhoan,
                            request.IDDanhMucDaiLy
                        );

                if (ID > 0)
                {
                    response.Status = 0;

                    var data = new
                    {
                        ID = ID,
                        MaKH = request.MaKH,
                        TenKH = request.TenKhachHang,
                        Email = request.Email,
                        SoDienThoai = request.SoDienThoai,
                        LoaiTaiKhoan = request.LoaiTaiKhoan,
                        IDDanhMucDaiLy = request.IDDanhMucDaiLy
                    };

                    response.Data = coreCommon.coreCommon.stringParse(JsonConvert.SerializeObject(data));
                    response.ErrorMsg = "Thêm tài khoản thành công!";
                }
                else
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Không thể thêm tài khoản!";
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Status = 1;
                response.Data = string.Empty;
                response.ErrorMsg = ex.Message;

                return response;
            }
        }

        [HttpGet]
        public webAPIresponse List(long ID)
        {
            webAPIresponse response = new webAPIresponse();

            try
            {
                DataTable dtAdmin = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, ID);

                if (dtAdmin == null || dtAdmin.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Tài khoản không tồn tại!";
                    return response;
                }

                int loaiTaiKhoan = Convert.ToInt32(dtAdmin.Rows[0]["LoaiTaiKhoan"]);

                if (loaiTaiKhoan != 1)
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Chỉ tài khoản Admin mới được xem danh sách tài khoản!";
                    return response;
                }

                DataTable dt = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, null);

                response.Status = 0;
                response.Data = JsonConvert.SerializeObject(dt);
                response.ErrorMsg = string.Empty;
            }
            catch (Exception ex)
            {
                response.Status = 1;
                response.Data = string.Empty;
                response.ErrorMsg = ex.Message;
            }

            return response;
        }

        [HttpPut]
        public webAPIresponse Update(long IDUser, DanhMucKhachHangForwarderUpdateRequest request)
        {
            webAPIresponse response = new webAPIresponse();

            try
            {
                if (request == null)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Dữ liệu không hợp lệ!";
                    return response;
                }

                DataTable dtUser = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, IDUser);

                if (dtUser.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Tài khoản không tồn tại!";
                    return response;
                }

                int loaiTaiKhoan = Convert.ToInt32(dtUser.Rows[0]["LoaiTaiKhoan"]);

                if (loaiTaiKhoan == 2 && request.ID != IDUser)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Khách hàng chỉ được sửa thông tin của chính mình!";
                    return response;
                }

                if (loaiTaiKhoan != 1 && loaiTaiKhoan != 2)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Loại tài khoản không hợp lệ!";
                    return response;
                }

                DataTable dtTarget = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, request.ID);

                if (dtTarget.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Tài khoản cần sửa không tồn tại!";
                    return response;
                }

                string? encryptedPassword = string.IsNullOrEmpty(request.MatKhau) ? null : coreCommon.coreCommon.EncryptString(request.MatKhau);

                DanhMucKhachHangForwarderBUS.Update(
                    GlobalVariables.ConnectionString,
                    request.ID,
                    encryptedPassword,
                    request.TenKhachHang,
                    request.Email,
                    request.SoDienThoai);

                response.Status = 0;
                response.Data = "";
                response.ErrorMsg = "Cập nhật tài khoản thành công!";

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

        [HttpDelete]
        public webAPIresponse Delete(long IDAdmin, long ID)
        {
            webAPIresponse response = new webAPIresponse();

            try
            {
                DataTable dtAdmin = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, IDAdmin);

                if (dtAdmin.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Tài khoản người thực hiện không tồn tại!";
                    return response;
                }

                int loaiTaiKhoan = Convert.ToInt32(dtAdmin.Rows[0]["LoaiTaiKhoan"]);

                if (loaiTaiKhoan != 1)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Chỉ tài khoản Admin mới được xóa tài khoản!";
                    return response;
                }

                if (IDAdmin == ID)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Không thể xóa tài khoản Admin đang đăng nhập!";
                    return response;
                }

                DataTable dtTarget = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, ID);

                if (dtTarget.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Tài khoản cần xóa không tồn tại!";
                    return response;
                }

                string? maKH = dtTarget.Rows[0]["MaKH"].ToString();

                DanhMucKhachHangForwarderBUS.Delete(GlobalVariables.ConnectionString, ID);

                response.Status = 0;
                var data = new
                {
                    ID = ID,
                    MaKH = maKH
                };
                response.Data = coreCommon.coreCommon.stringParse(JsonConvert.SerializeObject(data));
                response.ErrorMsg = "Xóa tài khoản thành công!";

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

        [HttpGet]
        public webAPIresponse Detail(long ID)
        {
            webAPIresponse response = new webAPIresponse();

            try
            {
                DataTable dt = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, ID);

                if (dt == null || dt.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = string.Empty;
                    response.ErrorMsg = "Tài khoản không tồn tại!";
                    return response;
                }

                DataRow row = dt.Rows[0];

                var data = new DanhMucKhachHangForwarderRequest
                {
                    ID = (long)row["ID"],
                    MaKH = row["MaKH"]?.ToString(),
                    MatKhau = row["MatKhau"]?.ToString(),
                    TenKhachHang = row["TenKH"]?.ToString(),
                    Email = row["Email"]?.ToString(),
                    SoDienThoai = row["SoDienThoai"]?.ToString(),
                    LoaiTaiKhoan = Convert.ToInt32(row["LoaiTaiKhoan"])
                };

                response.Status = 0;
                response.Data = JsonConvert.SerializeObject(data);
                response.ErrorMsg = string.Empty;
            }
            catch (Exception ex)
            {
                response.Status = 1;
                response.Data = string.Empty;
                response.ErrorMsg = ex.Message;
            }

            return response;
        }

        [HttpPut]
        public webAPIresponse ResetPassword(DanhMucKhachHangForwarderResetPasswordRequest request)
        {
            webAPIresponse response = new webAPIresponse();

            try
            {
                if (request == null)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Dữ liệu không được để trống!";
                    
                    return response;
                }

                if (request.IDAdmin <= 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "ID Admin không hợp lệ!";

                    return response;
                }

                if (request.ID <= 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "ID tài khoản không hợp lệ!";

                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.MatKhauMoi))
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Mật khẩu mới không được bỏ trống!";

                    return response;
                }

                DataTable dtAdmin = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, request.IDAdmin);

                if (dtAdmin == null || dtAdmin.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Tài khoản Admin không tồn tại!";

                    return response;
                }

                int loaiTaiKhoan = Convert.ToInt32(dtAdmin.Rows[0]["LoaiTaiKhoan"]);

                if (loaiTaiKhoan != 1)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Chỉ tài khoản Admin mới được đổi mật khẩu cho tài khoản khác!";

                    return response;
                }

                if (request.IDAdmin == request.ID)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Không sử dụng chức năng này để đổi mật khẩu Admin đang đăng nhập!";

                    return response;
                }

                DataTable dtTarget = DanhMucKhachHangForwarderBUS.List(GlobalVariables.ConnectionString, request.ID);

                if (dtTarget == null || dtTarget.Rows.Count == 0)
                {
                    response.Status = 1;
                    response.Data = "";
                    response.ErrorMsg = "Tài khoản cần đổi mật khẩu không tồn tại!";

                    return response;
                }

                string encryptedPassword = coreCommon.coreCommon.EncryptString(request.MatKhauMoi);

                DanhMucKhachHangForwarderBUS.ResetPassword(GlobalVariables.ConnectionString, request.ID, encryptedPassword);

                string? maKH = dtTarget.Rows[0]["MaKH"].ToString();

                var data = new
                {
                    ID = request.ID,
                    MaKH = maKH
                };

                response.Status = 0;
                response.Data = coreCommon.coreCommon.stringParse(JsonConvert.SerializeObject(data));
                response.ErrorMsg = "Đổi mật khẩu thành công!";

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