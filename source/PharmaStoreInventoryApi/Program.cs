using DataAccess.Contexts;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using PharmaStoreInventoryApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


//var applicationUrl = builder.Configuration.GetSection("ApplicationSettings:applicationUrl").Value;


#if DEBUG
//builder.WebHost.UseUrls("http://192.168.1.103:6555");
var connectionString = "Server=.; Database=stock;User Id=sa;Password=Ph@store;Persist Security Info=True;Encrypt=True; TrustServerCertificate=True";
builder.Services.AddDbContext<AppDb>(opts => opts.UseSqlServer(connectionString));
#endif

#if RELEASE
var applicationUrl = builder.Configuration.GetConnectionString("applicationUrl");
if (applicationUrl != null)
    builder.WebHost.UseUrls(applicationUrl);
builder.Services.RegisterDbContext(builder.Configuration);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
#endif

builder.Services.AddScoped<AppDb>();
builder.Services.AddScoped<ProductAmountRepo>();
builder.Services.AddScoped<EmployeeRepo>();
builder.Services.AddScoped<CommonsRepo>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureJWT();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/api/connection", async Task<bool> () =>
{
    return await Task.FromResult(true);
});
app.MapControllers();

app.Run();
