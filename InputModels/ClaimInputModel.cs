using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using learn.Models;

namespace learn.InputModels;

public class ClaimInputModel
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public static ClaimInputModel Save(UserModel user)
    {
        return new ClaimInputModel
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email
        };
    }

    public Claim[] Create()
    {
        return new Claim[] {
            new(JwtRegisteredClaimNames.UniqueName, Username),
            new("Id", Id.ToString()),
            new("Username", Username),
            new("Name", Name),
            new("Email", Email),
        };
    }
}