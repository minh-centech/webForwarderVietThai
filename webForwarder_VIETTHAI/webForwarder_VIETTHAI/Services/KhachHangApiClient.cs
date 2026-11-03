using webForwarder_VIETTHAI.Models;
using webForwarder_VIETTHAI.Models.Api;

namespace webForwarder_VIETTHAI.Services;
public sealed class KhachHangApiClient(HttpClient http, IHttpContextAccessor httpContextAccessor)
    : ApiClientBase(http), IKhachHangApi
{
    private const string Base = "api/DanhMucKhachHangForwarder/";

    // API dùng ID người đang đăng nhập với tên tham số khác nhau: List?ID=..., Update?IDUser=...
    private string CurrentIdQuery(string paramName)
    {
        var id = httpContextAccessor.HttpContext?.User.FindFirst("KhachHangId")?.Value;
        return string.IsNullOrEmpty(id) ? "" : $"?{paramName}={id}";
    }

    public async Task<PagedResult<KhachHang>> ListAsync(string? keyword, int page, int pageSize, CancellationToken ct = default)
    {
        var all = await GetAsync<List<KhachHang>>(Base + "List" + CurrentIdQuery("ID"), ct) ?? [];

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var k = keyword.Trim();
            all = all.Where(x => x.MaKH.Contains(k, StringComparison.OrdinalIgnoreCase)
                              || x.TenKH.Contains(k, StringComparison.OrdinalIgnoreCase)
                              || (x.Email ?? "").Contains(k, StringComparison.OrdinalIgnoreCase)
                              || (x.SoDienThoai ?? "").Contains(k))
                     .ToList();
        }

        return new PagedResult<KhachHang>
        {
            Items = all.OrderBy(x => x.MaKH).Skip((page - 1) * pageSize).Take(pageSize).ToList(),
            Total = all.Count,
            Page = page,
            PageSize = pageSize,
        };
    }

    public async Task<KhachHang?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        try
        {
            var d = await GetAsync<DetailData>(Base + "Detail?ID=" + id, ct);
            if (d is null || string.IsNullOrWhiteSpace(d.MaKH))
            {
                return null;
            }

            return new KhachHang
            {
                ID = d.ID > 0 ? d.ID : id,
                MaKH = d.MaKH,
                TenKH = d.TenKhachHang ?? d.TenKH ?? "",  
                Email = d.Email,
                SoDienThoai = d.SoDienThoai,
                LoaiTaiKhoan = d.LoaiTaiKhoan ?? TaiKhoan.LoaiKhachHang,
                IDDanhMucDaiLy = d.IDDanhMucDaiLy,
                CreateDate = d.CreateDate,
                EditDate = d.EditDate,
            };
        }
        catch (ApiException)
        {
            return null;
        }
    }
    private sealed class DetailData
    {
        public long ID { get; set; }
        public string? MaKH { get; set; }
        public string? TenKhachHang { get; set; }
        public string? TenKH { get; set; }
        public string? Email { get; set; }
        public string? SoDienThoai { get; set; }
        public int? LoaiTaiKhoan { get; set; }
        public long? IDDanhMucDaiLy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
    }

    public async Task<long> InsertAsync(KhachHang kh, string matKhau, CancellationToken ct = default)
    {
        var data = await PostAsync<IdData>(Base + "Insert", new
        {
            kh.MaKH,
            MatKhau = matKhau,
            TenKhachHang = kh.TenKH,
            kh.Email,
            kh.SoDienThoai,
            LoaiTaiKhoan = kh.LoaiTaiKhoan.ToString(),
            kh.IDDanhMucDaiLy,
        }, ct);

        return data?.ID ?? 0;
    }

    public Task UpdateAsync(KhachHang kh, string? matKhau, CancellationToken ct = default)
    {
        object body = matKhau is null
            ? new { kh.ID, TenKhachHang = kh.TenKH, kh.Email, kh.SoDienThoai }
            : new { kh.ID, MatKhau = matKhau, TenKhachHang = kh.TenKH, kh.Email, kh.SoDienThoai };

        return PutAsync<object>(Base + "Update" + CurrentIdQuery("IDUser"), body, ct);
    }

    public Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var query = CurrentIdQuery("IDAdmin");
        query = string.IsNullOrEmpty(query) ? $"?ID={id}" : $"{query}&ID={id}";
        return DeleteRequestAsync<object>(Base + "Delete" + query, ct);
    }

    // PUT api/DanhMucKhachHangForwarder/ResetPassword  { IDAdmin, ID, MatKhauMoi }
    public Task ResetPasswordAsync(long id, string matKhauMoi, CancellationToken ct = default)
    {
        var idAdmin = httpContextAccessor.HttpContext?.User.FindFirst("KhachHangId")?.Value;
        return PutAsync<object>(Base + "ResetPassword", new { IDAdmin = idAdmin, ID = id, MatKhauMoi = matKhauMoi }, ct);
    }


    private sealed class IdData { public long ID { get; set; } }
    private sealed class ResetData { public string? MatKhau { get; set; } }
}
