using System.ComponentModel.DataAnnotations;

namespace codenames.Modules.Game.DTO;

public class CreateGameDTO
{
    [Required] public string Name { get; set; }
    public string? Password { get; set; }
}