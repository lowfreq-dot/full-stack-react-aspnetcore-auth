using System;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiJWT.Database;
using WebApiJWT.Models;

public interface IAuthService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}

public class AuthService : IAuthService
{
    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            // Add additional claims if needed
        };

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(AuthOptions.ACCESS_TOKEN_LIFETIME),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}