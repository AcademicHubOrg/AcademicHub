using Microsoft.AspNetCore.OpenApi;
using Identity.Core;
// View Layer


// Infrastructure As a Code


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var service = new UsersService();

app.MapGet("/", () => "Hello World!");

app.MapGet("/users/list", async () => await service.ListAsync());
app.MapPost("/user", async (UserDto user) =>
{
  await service.AddAsync(user);
});

app.Run();
