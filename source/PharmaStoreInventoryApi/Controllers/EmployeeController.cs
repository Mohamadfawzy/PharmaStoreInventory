using DataAccess.DomainModel;
using DataAccess.Dtos;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmaStoreAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeeController(EmployeeRepo repo) : ControllerBase
{
    private readonly EmployeeRepo repo = repo;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (string.IsNullOrEmpty(model.SecretKey) && model.SecretKey != "2830575c-293c-4261-9139-6d6053263102")
        {
            return BadRequest();
        }

        var res = await repo.EmpLogin(model);
        if (res == null)
        {
            return BadRequest();
        }
        if (res.IsSuccess)
        {
            var auth = await CreateAuth(model);
            var authResult = new AuthResult<EmployeeDto?>()
            {
                Result = res,
                Auth = auth,
            };
            return Ok(authResult);
        }
        return Ok(new AuthResult<EmployeeDto?>() { Result = res });
    }

    [HttpGet("test_connection")]
    public async Task<IActionResult> TestConnection()
    {
        return Ok(await repo.TestConnection());
    }

    public async Task<AuthModel> CreateAuth(LoginDto login)
    {
        var auth = new AuthModel();
        var jwtSecurityToken = await CreateJwtToken(login);
        auth.Roles = new List<string> { "client" };
        auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        auth.ExpiresOn = jwtSecurityToken.ValidTo;
        return auth;
    }

    private Task<JwtSecurityToken> CreateJwtToken(LoginDto admin)
    {
        var roleClaims = new List<Claim>();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, admin.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("uid", admin.Username),
            new Claim("roles", "client")
        };
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sz8eI7OdHBrjrIo8j9nTW/rQyO1OvY0pAQ2wDKQZw/0="));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: "PharmaInventoryMobileApp",
            audience: "Pharmacist",
            claims: claims,
            expires: DateTime.Now.AddMonths(12),
            signingCredentials: signingCredentials);
        return Task.FromResult(jwtSecurityToken);
    }
}
