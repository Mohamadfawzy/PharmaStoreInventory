using DataAccess.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace PharmaStoreInventoryApi.Extensions;
public static class ServiceExtensions
{

    public static void ConfigureJWT(this IServiceCollection services)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = "PharmaInventoryMobileApp",
                ValidAudience = "Pharmacist",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sz8eI7OdHBrjrIo8j9nTW/rQyO1OvY0pAQ2wDKQZw/0="))
            };
        });
    }

    public static IServiceCollection RegisterDbContext(this IServiceCollection Services, IConfiguration Configuration)
    {
        // "server": "tcuGKOxBnoDKtQimVnrq/g==,1433;"
        var server = Configuration.GetConnectionString("server");
        string ip = "", port = "";


        if (server != null)
        {
            var dbServerDetails = server.Split(',');
            if (dbServerDetails.Length == 2)
            {
                ip = DecryptString(dbServerDetails[0], "bRyCqldrbgML3LXw", "y6OTIsYu8JE1wLkX");
                port = dbServerDetails[1];
            }
        }
        // ip = 192.168.1.1
        // port = 1433;
        var connectionString = $"Server={ip},{port}Database=stock;User Id=sa;Password=Ph@store;Persist Security Info=True;Encrypt=True;TrustServerCertificate=True;";

        Services.AddDbContext<AppDb>(options => options
        .UseSqlServer(connectionString));

        //Console.WriteLine(connectionString);
        return Services;
    }

    public static string DecryptString(string cipherText, string key, string iv)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        byte[] buffer = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = ivBytes;

        using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream(buffer))
        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        using (var reader = new StreamReader(cs))
        {
            return reader.ReadToEnd();
        }
    }

}
