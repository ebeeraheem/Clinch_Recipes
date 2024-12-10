using Clinch_Recipes.UserEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Clinch_Recipes.HelperMethods;

public class TokenHelper(IConfiguration config, UserManager<ApplicationUser> userManager)
{
    public async Task<AuthToken> GenerateTokensAsync(ApplicationUser user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30);

        await userManager.UpdateAsync(user);

        return new AuthToken(
            accessToken,
            refreshToken
        );
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateAccessToken(ApplicationUser user)
    {
        var key = config.GetValue<string>("Jwt:Key");
        var issuer = config.GetValue<string>("Jwt:Issuer");
        var audience = config.GetValue<string>("Jwt:Audience");

        // Ensure configuration values are not null or empty
        if (string.IsNullOrEmpty(key) ||
            string.IsNullOrEmpty(issuer) ||
            string.IsNullOrEmpty(audience))
        {
            throw new InvalidOperationException("JWT configuration values are missing.");
        }

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
       {
           new (ClaimTypes.NameIdentifier, user.Id),
           new (ClaimTypes.Email, user.Email ?? string.Empty)
       };
        // Generate token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}

public record AuthToken(string AccessToken, string RefreshToken);
