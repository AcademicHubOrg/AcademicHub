using System.Security.Claims;
using Identity.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Identity;

internal static class EndpointHandlers
{
    public static IResult HealthCheck()
    {
        return Results.Ok();
    }

    public static async Task<object> ListOfUsers([FromServices] UsersService service)
    {
        var result = await service.ListAsync();
        return new { Data = result };
    }

    public static async Task<object> AddUser([FromServices] UsersService service, UserDto user)
    {
        if (string.IsNullOrEmpty(user.Name))
        {
            throw new ArgumentException("Invalid data provided. User name is required.");
        }

        if (string.IsNullOrEmpty(user.Email))
        {
            throw new ArgumentException("Invalid data provided. User email is required.");
        }

        await service.AddAsync(user);
        return new { Message = "User added successfully." };
    }

    public static async Task<object> MakeAdmin([FromServices] UsersService service, HttpContext httpContext, string emailToMakeAdmin, string password)
    {
        var currentUserEmail = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        var targetStatus = GetUserExistAndAdmin(service, emailToMakeAdmin);
        var requesterStatus = GetUserExistAndAdmin(service, currentUserEmail);

        if (targetStatus.Result.Item1 == false)
        {
            throw new ArgumentException(emailToMakeAdmin);
        }
        if (requesterStatus.Result.Item1 == false)
        {
            throw new ArgumentException(currentUserEmail);
        }
        if (requesterStatus.Result.Item2 == false)
        {
            throw new ArgumentException(currentUserEmail);
        }
        if (targetStatus.Result.Item2 == true)
        {
            throw new ArgumentException(emailToMakeAdmin);
        }
        
        await service.MakeAdminAsync(emailToMakeAdmin, password);
        return new { Message = "Admin assigned successfully." };
    }

    public static async Task<IResult> Login(HttpContext httpContext)
    {
        await httpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = "/after-signin"
        });

        return Results.Empty;
    }

    public static async Task<IResult> AfterSignIn(HttpContext httpContext, CancellationToken ct)
    {
        var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
        {
            return Results.Unauthorized();
        }

        var email = authenticateResult.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)
            ?.Value;
        var name = authenticateResult.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
        {
            return Results.Unauthorized();
        }

        var userService = httpContext.RequestServices.GetRequiredService<UsersService>();
        var user = await userService.FindOrCreateUser(email, name);

        return Results.Ok(user);
    }
    
    public static async Task<bool> UserExist([FromServices] UsersService service, string email)
    {
        var tmp = GetUserExistAndAdmin(service, email);
        return tmp.Result.Item1;
    }
    
    public static async Task<bool> UserAdmin([FromServices] UsersService service, string email)
    {
        var tmp = GetUserExistAndAdmin(service, email);
        return tmp.Result.Item2;
    }

    private static async Task<(bool, bool)> GetUserExistAndAdmin([FromServices] UsersService service, string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Unable to identify the current user with email: " + email);
        }
        var UserExistAndAdmin = await service.GetUserExistAndAdmin(email);
        return UserExistAndAdmin;
    }
}