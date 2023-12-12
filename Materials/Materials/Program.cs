using Materials;
using Materials.Core;
using CustomExceptions;
using Materials.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Access configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Configure your DbContext
builder.Services.AddDbContext<MaterialsDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<IMaterialsRepository, MaterialsRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("http://165.22.66.19:3000") // Replace with the actual origin of your frontend
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

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

app.UseRouting(); // This must come before UseAuthentication and UseAuthorization
app.UseHttpsRedirection();

app.UseMiddleware<CustomErrorHandlingMiddleware>();

app.MapGet("/healthz", EndpointHandlers.HealthCheck);
app.MapGet("/materials/list", EndpointHandlers.ListOfMaterials);
app.MapGet("/materials/essentials-list", EndpointHandlers.ListOfEssentialMaterials);
app.MapGet("/materials/by-id/{id}", EndpointHandlers.GetMaterialById);
app.MapGet("/materials/by-essential-id/{id}", EndpointHandlers.GetEssentialMaterialById);
app.MapGet("/materials/by-course/{courseId}", EndpointHandlers.GetMaterialsByCourseId);
app.MapGet("/materials/by-template/{templateId}", EndpointHandlers.GetMaterialsByTemplateId);
app.MapPost("/materials/add", EndpointHandlers.AddMaterial);
app.MapPost("/materials/add-essential", EndpointHandlers.AddEssentialMaterial);

// Apply EF Core Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MaterialsDbContext>();
    dbContext.Database.Migrate();
}

app.Run();