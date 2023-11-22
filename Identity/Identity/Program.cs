using CustomExceptions;
using Identity;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authentication.Google;
using Identity.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UsersService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:3000") // Replace with the actual origin of your frontend
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddMvc(options => { options.Filters.Add<CustomExceptionFilter>(); });

// Register authentication services
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/login"; // Specify the login path
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = "989838419075-cmepplaro69du10sdlftsr5f8u58gj7p.apps.googleusercontent.com";
        googleOptions.ClientSecret = "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";
        googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });


// Register authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigin");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var env = context.RequestServices.GetService<IWebHostEnvironment>();
        var errorCode = StatusCodes.Status500InternalServerError;
        context.Response.StatusCode = errorCode;
        context.Response.ContentType = "application/json";

        var errorMessage = "An unexpected error occurred. Please try again later.";

        if (exceptionHandlerPathFeature?.Error is ArgumentException)
        {
            errorMessage = exceptionHandlerPathFeature.Error.Message;
            errorCode = StatusCodes.Status400BadRequest;
        }
        else if (exceptionHandlerPathFeature?.Error is NotFoundException)
        {
            errorMessage = exceptionHandlerPathFeature.Error.Message;
            errorCode = StatusCodes.Status404NotFound;
        }
        else if (exceptionHandlerPathFeature?.Error is ConflictException)
        {
            errorMessage = exceptionHandlerPathFeature.Error.Message;
            errorCode = StatusCodes.Status409Conflict;
        }

        if (env.IsDevelopment())
        {
            // In development, return detailed error information
            errorMessage += " dev info: " + exceptionHandlerPathFeature?.Error.InnerException;
        }

        await context.Response.WriteAsJsonAsync(new
        {
            ErrorMessage = errorMessage,
            ErrorCode = errorCode
        });
    });
});


// StatusCodePages Middleware
app.UseStatusCodePages(context =>
{
    context.HttpContext.Response.ContentType = "application/json";
    return context.HttpContext.Response.WriteAsJsonAsync(new { ErrorMessage = "An unexpected error occurred." });
});


app.UseRouting(); // This must come before UseAuthentication and UseAuthorization
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
// Then, define endpoints


app.MapGet("/", EndpointHandlers.BaseUrl);
app.MapGet("/user/list", EndpointHandlers.ListOfUsers);
app.MapPost("/user/add", EndpointHandlers.AddUser);
app.MapPost("/user/makeAdmin", EndpointHandlers.MakeAdmin);
app.MapGet("/login", EndpointHandlers.Login);
app.MapGet("/after-signin", EndpointHandlers.AfterSignIn);

app.Run();
