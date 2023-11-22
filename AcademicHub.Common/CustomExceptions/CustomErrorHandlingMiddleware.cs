namespace CustomExceptions;

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class CustomErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomErrorHandlingMiddleware> _logger;

    public CustomErrorHandlingMiddleware(RequestDelegate next, ILogger<CustomErrorHandlingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred");

            var response = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? exception.Message
                : "An error occurred. Please try again later.";

            int statusCode = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response);
        }
    }
}