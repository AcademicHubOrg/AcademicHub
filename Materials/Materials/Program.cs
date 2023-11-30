using Materials;
using Materials.Core;
using CustomExceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MaterialService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder =>
		{
			builder.WithOrigins("http://localhost:3000") // Replace with the actual origin of your frontend
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
	return context.HttpContext.Response.WriteAsJsonAsync(new { ErrorMessage = "An unexpected error occurred." });
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

app.Run();



