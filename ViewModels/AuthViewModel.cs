namespace learn.ViewModels;

public class AuthViewModel
{
    public string Status { get; set; } = null!;
    public string? Jwt { get; set; }
    public string? RefreshToken { get; set; }

    private AuthViewModel() { }

    private AuthViewModel(string status, string? jwt = null, string? refreshToken = null)
    {
        Status = status;
        Jwt = jwt;
        RefreshToken = refreshToken;
    }

    public static AuthViewModel Success(string jwt, string refreshToken)
    {
        return new AuthViewModel("Success", jwt, refreshToken);
    }

    public static AuthViewModel Failed()
    {
        return new AuthViewModel("Failed");
    }
}