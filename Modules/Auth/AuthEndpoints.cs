using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace codenames.Modules.Auth;

public static class AuthEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/register",
            async (HttpContext httpContext, CodenamesContext context, IPasswordHasher hasher,
                RegisterUserDTO? registerUserDto) =>
            {
                if (registerUserDto is null) return Results.BadRequest("Incorrect data format.");

                if (!ValidationHelper.IsValid(registerUserDto, out var results)) return Results.BadRequest(results);

                var hashedPassword = hasher.HashPassword(registerUserDto.Password);

                var user = new User
                {
                    Email = registerUserDto.Email, Name = registerUserDto.Name, Password = hashedPassword
                };
                try
                {
                    context.Users.Add(user);
                    await context.SaveChangesAsync();

                    return Results.Ok(new { Id = user.UserId });
                }
                catch (DbUpdateException exception)
                {
                    return Results.BadRequest(new { Message = "User with that email or name already exists!" });
                }
            });

        app.MapPost("/login",
            async (HttpContext httpContext, CodenamesContext context, IPasswordHasher hasher,
                LoginUserDTO? loginUserDto) =>
            {
                if (loginUserDto is null) return Results.BadRequest("Incorrect data format.");

                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);

                if (user is null) return Results.BadRequest(new { Message = "Incorrect login details" });

                var result = hasher.VerifyPassword(loginUserDto.Password, user.Password);

                if (!result) return Results.BadRequest(new { Message = "Incorrect login details" });

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Name),
                    new(ClaimTypes.Email, user.Password)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Results.Ok(new { Message = "Logged in successfully", Id = user.UserId });
            });

        app.MapPost("/logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Results.Ok(new { Message = "Logged out successfully" });
        });
    }
}