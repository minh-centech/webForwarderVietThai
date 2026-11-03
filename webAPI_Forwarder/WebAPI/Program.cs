using WebAPI.Code;

var builder = WebApplication.CreateBuilder(args);

// Đọc cấu hình
GlobalVariables.ConnectionString =
    builder.Configuration.GetConnectionString("Database") ?? "";

GlobalVariables.IDDanhMucDonVi =
    builder.Configuration["AppSettings:IDDanhMucDonVi"] ?? "";

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();