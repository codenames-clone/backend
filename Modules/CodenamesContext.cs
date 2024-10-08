using codenames.Modules.Auth;
using codenames.Modules.Game;
using Microsoft.EntityFrameworkCore;

namespace codenames.Modules;

public class CodenamesContext(DbContextOptions<CodenamesContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserInRoom> UsersInRooms { get; set; }
    public DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configure domain classes using modelBuilder here   

        modelBuilder.Entity<UserInRoom>()
            .HasKey(o => new { o.UserId, o.RoomId });

        modelBuilder.Entity<UserInRoom>()
            .Property(o => o.Role)
            .HasConversion<string>();

        modelBuilder.Entity<UserInRoom>()
            .Property(o => o.Team)
            .HasConversion<string>();
    }
}