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

        if (await isUserExist(service, emailToMakeAdmin) == false)
        {
            throw new ArgumentException("email target");
        }
        if (await isUserExist(service, currentUserEmail) == false)
        {
            throw new ArgumentException("email current");
        }

        
        if (await isUserAdmin(service, currentUserEmail) == false)
        {
            return Results.Forbid();
        }
        
        if (await isUserAdmin(service, emailToMakeAdmin) == true)
        {
            return new { Message = "This user is already an admin." };
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
    
    public static async Task<bool> isUserExist([FromServices] UsersService service, string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Unable to identify the current user with email: " + email);
        }

        var isUserExist= await service.UserExist(email);
        if (isUserExist)
        {
            return true;
        }

        return false;
        
        //throw new ArgumentException("Unable to identify the current user with email: " + email);
    }
    
    public static async Task<bool> isUserAdmin([FromServices] UsersService service, string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Unable to identify the current user with email: " + email);
        }

        var isUserExist= await service.UserExist(email);
        if (!isUserExist)
        {
            return false;
        }

        var isCurrentUserAdmin = await service.IsUserAdminAsync(email);
        if (!isCurrentUserAdmin)
        {
            return false;
        }

        return true;

        //throw new ArgumentException("Unable to identify the current user with email: " + email);
    }
}