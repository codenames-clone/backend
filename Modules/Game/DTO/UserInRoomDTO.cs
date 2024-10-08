namespace codenames.Modules.Game.DTO;

public class UserInRoomDTO
{
    public int UserId { get; set; }
    public int RoomId { get; set; }

    public string Role { get; set; }
    public string Team { get; set; }

    public bool IsHost { get; set; }
}