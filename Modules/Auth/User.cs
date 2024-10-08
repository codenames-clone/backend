using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace codenames.Modules.Auth;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int UserId { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}