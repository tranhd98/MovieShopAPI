using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MovieShopAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;

    private IConfiguration _configuration;
    // GET
    public AccountController(IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
    }


    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterRequestModel model)
    {
        // 2XX 
        if (!ModelState.IsValid)
        {
            return BadRequest(model);
        }

        var user = await _accountService.CreateUser(model);
        if (user == null) return BadRequest();
        return Ok(user);
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(UserLoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await _accountService.ValidateUser(model);
        if (user == null)
        {
            return Unauthorized(new {error = "please verify email/password"});
        }

        var token = GenerateToken(user);

        return Ok(new{token = token});
    }

    private string GenerateToken(LoginResponse user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName ),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth.ToShortDateString()),
            new("Language", "en")
        };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        // check if user has any roles
        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);
        
        // create the token with a secret signature
        // expiration time
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var expirationTime = DateTime.UtcNow.AddHours(_configuration.GetValue<int>("ExpirationHours"));
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // describe the contents of the token
        var token = new SecurityTokenDescriptor
        {
            Subject = identityClaims,
            Expires = expirationTime,
            SigningCredentials = credentials,
            Issuer = _configuration["Issuer"],
            Audience = _configuration["Audience"]
        };

        var encodedJWT = tokenHandler.CreateToken(token);
        return tokenHandler.WriteToken(encodedJWT);


    }
}