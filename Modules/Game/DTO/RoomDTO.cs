namespace codenames.Modules.Game.DTO;

public class RoomDTO
{
    public int RoomId { get; set; }

    public string Name { get; set; }
    public string Code { get; set; }

    public string? Password { get; set; }

    public ICollection<UserInRoomDTO> Users { get; } = new List<UserInRoomDTO>();
    public bool IsInProgress { get; set; }
}