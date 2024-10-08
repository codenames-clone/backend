using System.Text.Json.Serialization;
using codenames.Modules.Auth;

namespace codenames.Modules.Game;

public class UserInRoom
{
    public int UserId { get; set; }
    public int RoomId { get; set; }

    [JsonIgnore] public Room Room { get; set; } = null!;
    public User User { get; set; } = null!;

    public Role Role { get; set; }
    public Team Team { get; set; }

    public bool IsHost { get; set; }
}