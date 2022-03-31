using CyberSec.Cryptocurrency.Service.Entities;
using CyberSec.Cryptocurrency.Service.Persistence;
using CyberSec.Cryptocurrency.Service.Interfaces;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CyberSec.Cryptocurrency.Service.Models.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace CyberSec.Cryptocurrency.Service.Services;

public class JwtUtils : IJwtUtils
{
    private readonly IDate _date;
    private readonly JwtSetting _jwtSetting;

    public JwtUtils(
        IOptions<JwtSetting> jwtSetting,
        IDate date)
    {
        _date = date;
        _jwtSetting = jwtSetting.Value;
    }

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = getUniqueToken(),
            Expires = _date.AddDays(7),
            Created = _date.Now,
            CreatedByIp = ipAddress
        };

        return refreshToken;

        static string getUniqueToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }

    public string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSetting.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(
                new[]
                {
                    new Claim(nameof(User.Id), user.Id.ToString())
                }),
            Expires = _date.AddMinutes(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string ValidateJwtToken(string token)
    {
        if (token is null) return string.Empty;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSetting.Secret);

        try
        {
            tokenHandler.ValidateToken(
                token,
                new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                },
                out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = jwtToken.Claims.First(x => x.Type == nameof(User.Id)).Value;

            return userId;
        }
        catch
        {
            return string.Empty;
        }
    }
}