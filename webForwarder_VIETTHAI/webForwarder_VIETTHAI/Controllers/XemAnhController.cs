using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Models.Api;
using webForwarder_VIETTHAI.Services;

namespace webForwarder_VIETTHAI.Controllers;

/// <summary>Xem ảnh chụp theo số lệnh nhập hàng / đóng hàng / rút hàng .</summary>
[Authorize]
public class XemAnhController(IAnhLenhApi api) : Controller
{
    // GET /XemAnh?loai=nhap&so=LNX26092800001   (loai = nhap | dong | rut)
    public async Task<IActionResult> Index(string? loai, string? so, CancellationToken ct)
    {
        var model = new XemAnhViewModel { Loai = LoaiLenhInfo.Parse(loai), So = so?.Trim() };

        if (string.IsNullOrWhiteSpace(model.So))
        {
            return View(model);
        }

        model.DaTim = true;

        try
        {
            model.Anh = await api.GetAnhAsync(model.Loai, model.So, ct);
        }
        catch (ApiException ex)
        {
            model.Loi = "Không lấy được ảnh: " + ex.Message;
        }
        catch (HttpRequestException)
        {
            model.Loi = "Không kết nối được API ảnh (AnhApi:BaseUrl trong appsettings.json).";
        }

        return View(model);
    }

    // GET /XemAnh/Anh?u=http://123.30.235.18/cenCFS-ITS/AnhLenhNhapKhoHangXuatKhau/LNX.../xxx.jpg
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client)]
    public async Task<IActionResult> Anh(string? u, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(u) || !Uri.TryCreate(u, UriKind.Absolute, out _))
        {
            return BadRequest();
        }

        (byte[] Bytes, string ContentType)? anh;
        try
        {
            anh = await api.TaiAnhAsync(u, ct);
        }
        catch (HttpRequestException)
        {
            return StatusCode(StatusCodes.Status502BadGateway);
        }

        return anh is null ? NotFound() : File(anh.Value.Bytes, anh.Value.ContentType);
    }
}
