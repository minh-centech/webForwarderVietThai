using Microsoft.AspNetCore.Authentication.Cookies;   // [THÊM]
using webForwarder_VIETTHAI.Services;  // [THÊM]

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

var apiBase = new Uri(builder.Configuration["Api:BaseUrl"]
    ?? throw new InvalidOperationException("Thiếu Api:BaseUrl trong appsettings.json"));

var reportApiBase = new Uri(
    builder.Configuration["ReportApi:BaseUrl"]
    ?? throw new InvalidOperationException(
        "Thiếu ReportApi:BaseUrl trong appsettings.json"));

builder.Services.AddHttpClient<IKhachHangApi, KhachHangApiClient>(c => c.BaseAddress = apiBase).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Bỏ qua xác thực SSL cho môi trường test/nội bộ
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
}); ;
builder.Services.AddHttpClient<IAuthApi, AuthApiClient>(c => c.BaseAddress = apiBase).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Bỏ qua xác thực SSL cho môi trường test/nội bộ
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});
builder.Services.AddHttpClient<IHangXuatKhauApi, HangXuatKhauApiClient>(c => c.BaseAddress = apiBase).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Bỏ qua xác thực SSL cho môi trường test/nội bộ
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
}); 
builder.Services.AddHttpClient<IHangNhapKhauApi, HangNhapKhauApiClient>(c => c.BaseAddress = apiBase).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Bỏ qua xác thực SSL cho môi trường test/nội bộ
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
}); 
builder.Services.AddHttpClient<IReportApi, ReportApi>(c => c.BaseAddress = reportApiBase).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Bỏ qua xác thực SSL cho môi trường test/nội bộ
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
}); 
builder.Services.AddHttpClient<IDaiLyApi, DaiLyApiClient>(c => c.BaseAddress = apiBase).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Bỏ qua xác thực SSL cho môi trường test/nội bộ
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
}); 

// API ảnh chụp theo lệnh (cenCFS-ITS) 
var anhApiBase = new Uri(builder.Configuration["AnhApi:BaseUrl"]
    ?? throw new InvalidOperationException("Thiếu AnhApi:BaseUrl trong appsettings.json"));
builder.Services.AddHttpClient<IAnhLenhApi, AnhLenhApiClient>(c =>
{
    c.BaseAddress = anhApiBase;
    c.Timeout = TimeSpan.FromSeconds(60);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Bỏ qua xác thực SSL cho môi trường test/nội bộ
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

builder.Services.AddHttpClient<IReportApi, ReportApi>(c =>
{
    c.BaseAddress = reportApiBase;
    c.Timeout = TimeSpan.FromSeconds(120);   

    // Khoá bảo vệ dịch vụ in (đặt cùng giá trị ở ReportApi:ApiKey và Web.config <add key="ApiKey"> bên webForwarder_Report)
    var apiKey = builder.Configuration["ReportApi:ApiKey"];
    if (!string.IsNullOrWhiteSpace(apiKey))
    {
        c.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
    }
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Bỏ qua xác thực SSL cho môi trường test/nội bộ
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o => { o.LoginPath = "/Login"; o.LogoutPath = "/Login/Logout"; o.AccessDeniedPath = "/Login/KhongCoQuyen"; });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();   // [THÊM]
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
