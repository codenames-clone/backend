using Microsoft.EntityFrameworkCore;

namespace codenames.Modules.Game;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Code), IsUnique = true)]
public class Room
{
    public int RoomId { get; set; }

    public string Name { get; set; }
    public string Code { get; set; }

    public string? Password { get; set; }

    public ICollection<UserInRoom> Users { get; } = new List<UserInRoom>();
    public bool IsInProgress { get; set; }
}