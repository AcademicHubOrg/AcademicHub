﻿using Identity.Core;
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
        return new {Data = result};
    }

    public static async Task<object> AddUser([FromServices] UsersService service, UserAddDto user)
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
        return new {Message = "User added successfully."};
    }

    public static async Task<object> Login([FromServices] UsersService service, UserAddDto user)
    {
        if (string.IsNullOrEmpty(user.Name))
        {
            throw new ArgumentException("Invalid data provided. User name is required.");
        }

        if (string.IsNullOrEmpty(user.Email))
        {
            throw new ArgumentException("Invalid data provided. User email is required.");
        }

        await service.FindOrCreateUser(user);
        return new {Message = "User logined successfully."};
    }

    public static async Task<object> MakeAdmin([FromServices] UsersService service, string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Invalid data provided. Email is required.");
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Invalid data provided. Password is required.");
        }

        await service.MakeAdminAsync(email, password);
        return new {Message = "Admin assigned successfully."};
    }
    public static async Task<object> GetByEmail([FromServices] UsersService service, string email)
    {
        var user = await service.GetByEmailAsync(email);
        return user;
    }

}