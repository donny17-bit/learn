using System.ComponentModel.DataAnnotations;

namespace learn.InputModels;

public class AuthInputModel
{
    public class Login
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}