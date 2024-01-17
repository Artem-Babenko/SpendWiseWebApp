
using SpendWiseWebApp.Models;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder();
// Add Database service.
string connectionString = builder.Configuration.GetConnectionString("DevConnection")!;
builder.Services.AddSqlServer<ApplicationDbContext>(connectionString);
// Add controllers and views.
builder.Services.AddControllersWithViews();
// Add authentication and authorization services.
builder.Services.AddAuthentication("Cookies").AddCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = "AuthenticationCookie";
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/login";
});
builder.Services.AddAuthorization();
// Add compression for response.
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});
var app = builder.Build();

app.UseStaticFiles();
app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();