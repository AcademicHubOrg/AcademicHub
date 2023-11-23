using CustomExceptions;
using Identity;
using Microsoft.AspNetCore.Authentication.Google;
using Identity.Core;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        googleOptions.ClientId = builder.Configuration["Auth:Google:ClientID"]!;
        googleOptions.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"]!;
        googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });

// Register authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigin");

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

app.UseMiddleware<CustomErrorHandlingMiddleware>();

app.MapGet("/", EndpointHandlers.BaseUrl);
app.MapGet("/user/list", EndpointHandlers.ListOfUsers);
app.MapPost("/user/add", EndpointHandlers.AddUser);
app.MapPost("/user/makeAdmin", EndpointHandlers.MakeAdmin);
app.MapGet("/login", EndpointHandlers.Login);
app.MapGet("/after-signin", EndpointHandlers.AfterSignIn);

app.Run();
