using CustomExceptions;
using Identity;
using Identity.Core;
using Identity.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Access configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure your DbContext
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

// Configure Forwarded Headers Middleware to handle reverse proxy server scenarios
// builder.Services.Configure<ForwardedHeadersOptions>(options =>
// {
//     options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
// });
//
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(Address.academichuburl) // Replace with the actual origin of your frontend
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
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
    return context.HttpContext.Response.WriteAsJsonAsync(new {ErrorMessage = "An unexpected error occurred."});
});

// Use Forwarded Headers Middleware to read headers forwarded by the reverse proxy
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<CustomErrorHandlingMiddleware>();

app.MapGet("/healthz", EndpointHandlers.HealthCheck);
app.MapGet("/users/list", EndpointHandlers.ListOfUsers);
app.MapPost("/users/add", EndpointHandlers.AddUser);
app.MapPost("/users/login", EndpointHandlers.Login);
app.MapPost("/users/makeAdmin", EndpointHandlers.MakeAdmin);

// Apply EF Core Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    dbContext.Database.Migrate();
}

app.Run();