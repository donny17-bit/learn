using System.IdentityModel.Tokens.Jwt;
using System.Text;
using learn.Configs;
using learn.InputModels;
using Microsoft.IdentityModel.Tokens;

namespace learn.Services;

public class TokenService
{
    public static string Jwt(ConfigJwt configsJwt, ClaimInputModel claim, int timerInMinutes)
    {
        // create credential
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configsJwt.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        // create claim information
        var claims = claim.Create();

        // create secure token
        var jwtToken = new JwtSecurityToken(
            issuer: configsJwt.Issuer,
            audience: configsJwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(timerInMinutes),
            signingCredentials: credentials
        );

        // create token from JWT 
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }

    public static string RefreshToken(int length)
    {
        Random random = new Random();
        string randomString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ12344567890";

        string RefreshToken = new string(Enumerable
            .Repeat(randomString, length)
            .Select(m => m[random.Next(m.Length)])
            .ToArray()
        );

        return RefreshToken;
    }
}