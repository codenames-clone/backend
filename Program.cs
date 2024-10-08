using codenames;
using codenames.Modules;
using codenames.Modules.Auth;
using codenames.Modules.Game;
using codenames.Modules.Game.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<UserInRoom, UserInRoomDTO>();
    config.CreateMap<Room, RoomDTO>();
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<CodenamesContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("CodenamesPostgres")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
});
builder.Services.AddAuthorization();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict
};
app.UseCookiePolicy(cookiePolicyOptions);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CodenamesContext>();
    context.Database.EnsureCreated();
    await context.Database.MigrateAsync();
    // DbInitializer.Initialize(context);
}

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        =>
    {
        await Results.Problem()
            .ExecuteAsync(context);
    }));

AuthEndpoints.Map(app);
GameEndpoints.Map(app);

app.Run();