using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authentication.Google;
using Identity.Core;
using Microsoft.AspNetCore.Authentication;

// View Layer

// Infrastructure As a Code

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
  {
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Google";
  })
  .AddCookie()
  .AddGoogle("Google", options =>
  {
    options.ClientId = "989838419075-cmepplaro69du10sdlftsr5f8u58gj7p.apps.googleusercontent.com";
    options.ClientSecret = "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";
  });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseAuthentication();
// app.UseAuthorization();

var service = new UsersService();

app.MapGet("/", () => "Hello World!");

app.MapGet("/users/list", async () => await service.ListAsync());
app.MapPost("/user", async (UserDto user) =>
{
  await service.AddAsync(user);
});

app.MapGet("/login", async context =>
{
  await context.ChallengeAsync("Google", new AuthenticationProperties { RedirectUri = "/" });
});

app.MapGet("/signin-google", async context =>
{
  var result = await context.AuthenticateAsync("Google");
  if (result.Principal != null)
  {
    context.Response.Redirect("/");
    // Create a user in your database if it doesn't exist
    // Redirect to the original protected resource or a default page
  }
  else
  {
    context.Response.Redirect("/login-failed");
  }
});


app.Run();
