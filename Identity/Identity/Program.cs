using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authentication.Google;
using Identity.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

// View Layer

// Infrastructure As a Code

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register authentication services
builder.Services.AddAuthentication(options =>
  {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
  })
  .AddCookie(options =>
  {
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

// Add the UsersService to the DI container
builder.Services.AddScoped<UsersService>(); // Use AddScoped or AddSingleton as appropriate

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting(); // This must come before UseAuthentication and UseAuthorization

app.UseAuthentication();
app.UseAuthorization();

var service = new UsersService();

// Define your endpoints here
app.MapGet("/", () => "Hello World!");

app.MapGet("/users/list", async () => await service.ListAsync());

app.MapPost("/user/add", async (UserDto user) =>
{
    await service.AddAsync(user);
});

app.MapPost("/user/makeAdmin", async (string email, string password) =>
{
  await service.MakeAdminAsync(email,password);
});




app.MapGet("/login", (HttpContext httpContext) =>
{
  // Challenge Google authentication, which will redirect to Google's login page
  return httpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
  {
    RedirectUri = "/after-signin" // Set the redirect URI to your callback endpoint
  });
});

app.MapGet("/after-signin", async (HttpContext httpContext) =>
{
  var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

  if (!authenticateResult.Succeeded)
    return Results.Redirect("/login"); // Redirect to login if not authenticated

  // Assuming you have a method to find or create the user
  var email = authenticateResult.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
  var name = authenticateResult.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;

  // Use your UsersService to find or create the user
  var userService = httpContext.RequestServices.GetRequiredService<UsersService>();
  var user = await userService.FindOrCreateUser(email!, name!);

  // Sign in the user with your own application logic if needed
  // ...

  // Redirect to the default page after login
  return Results.Redirect("/"); // Replace with your default page URL
});

app.Run();
