using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace codenames.Modules.Auth;

public class LoginUserDTO
{
    [Required(ErrorMessage = "The email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The password is required.")]
    [StringLength(100, MinimumLength = 8)]
    [PasswordPropertyText]
    public string Password { get; set; }

    public override string ToString()
    {
        return $"LoginUserDTO(Email: {Email})";
    }
}