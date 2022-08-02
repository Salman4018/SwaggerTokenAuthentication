using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SwaggerTokenAuthentication.Controllers;

public class AuthController : ControllerBase
{
    private readonly JwtConfiguration _jwtConfigurationOptions;
    public AuthController(IOptions<JwtConfiguration> jwtConfigurationOptions)
    {
        _jwtConfigurationOptions = jwtConfigurationOptions.Value;
    }


    [HttpPost, Route("login")]
    public IActionResult Login(LoginModel model)
    {
        if (model == null)
        {
            return BadRequest("Invalid client request");
        }

        if (model.UserName.Equals("admin", StringComparison.CurrentCultureIgnoreCase) && 
            model.Password.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurationOptions.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtConfigurationOptions.Issuer,
                audience: _jwtConfigurationOptions.Issuer,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(50),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { Token = tokenString });
        }
        else
        {
            return Unauthorized();
        }
    }
}