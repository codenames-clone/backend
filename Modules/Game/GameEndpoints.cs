using System.Security.Claims;
using AutoMapper;
using codenames.Modules.Game.DTO;
using Microsoft.EntityFrameworkCore;

namespace codenames.Modules.Game;

public static class GameEndpoints
{
    private static readonly Random Random = new();

    public static void Map(WebApplication app)
    {
        app.MapPost("/games", async (CreateGameDTO createGameDto, HttpContext httpContext, CodenamesContext context) =>
        {
            var hostUserName = httpContext.User.Claims.First(claim => claim.Type.Equals(ClaimTypes.Name));
            var transaction = await context.Database.BeginTransactionAsync();

            var hostUser = await context.Users.FirstOrDefaultAsync(user => user.Name.Equals(hostUserName.Value));

            if (hostUser is null)
            {
                await transaction.RollbackAsync();
                return Results.BadRequest(new { Message = "User not found" });
            }

            var code = RandomString(5);

            var room = new Room
            {
                Name = createGameDto.Name,
                Password = createGameDto.Password,
                Code = code
            };


            var userInRoom = new UserInRoom
            {
                User = hostUser,
                Room = room,
                Role = Role.Spectator,
                Team = Team.Spectator,
                IsHost = true
            };

            context.Rooms.Add(room);
            context.UsersInRooms.Add(userInRoom);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Results.Ok(new { Code = code });
        }).RequireAuthorization();

        app.MapGet("/games/{code}", async (string code, CodenamesContext context, IMapper mapper) =>
        {
            var room = await context.Rooms.Include(room => room.Users)
                .FirstOrDefaultAsync(room => room.Code.Equals(code));

            if (room is null) return Results.BadRequest(new { Message = "No room for a given code" });

            return Results.Ok(mapper.Map<Room, RoomDTO>(room));
        }).RequireAuthorization();
    }

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}