using CustomExceptions;
using Identity;
using Microsoft.AspNetCore.Authentication.Google;
using Identity.Core;
using Identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("http://localhost:3000") // Replace with the actual origin of your frontend
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
    return context.HttpContext.Response.WriteAsJsonAsync(new {ErrorMessage = "An unexpected error occurred."});
});

// Use Forwarded Headers Middleware to read headers forwarded by the reverse proxy
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// app.UseSession();
// Use Cookie Policy with Lax same-site policy
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CustomErrorHandlingMiddleware>();

app.MapGet("/healthz", EndpointHandlers.HealthCheck);
app.MapGet("/users/list", EndpointHandlers.ListOfUsers);
app.MapPost("/users/add", EndpointHandlers.AddUser);
app.MapPost("/users/makeAdmin", EndpointHandlers.MakeAdmin);
app.MapGet("/login", EndpointHandlers.Login);
app.MapGet("/after-signin", EndpointHandlers.AfterSignIn);

// Apply EF Core Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    dbContext.Database.Migrate();
}

app.Run();