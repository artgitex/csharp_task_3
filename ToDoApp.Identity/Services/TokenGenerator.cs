using AuthenticationServer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationServer.Services;

public class TokenGenerator
{
    private readonly AuthenticationConfiguration _authenticationConfiguration;

    public TokenGenerator(AuthenticationConfiguration authenticationConfiguration)
    {
        _authenticationConfiguration = authenticationConfiguration;
    }

    public string GenerateToken(User user)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfiguration.AccessTokenSecret));        

        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new List<Claim>()
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };

        JwtSecurityToken token = new JwtSecurityToken(
            _authenticationConfiguration.Issuer,
            _authenticationConfiguration.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_authenticationConfiguration.AccessTokenExpirationMinutes)
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
