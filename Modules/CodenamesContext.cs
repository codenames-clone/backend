using codenames.Modules.Auth;
using Microsoft.EntityFrameworkCore;

namespace codenames.Modules;

public class CodenamesContext(DbContextOptions<CodenamesContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}