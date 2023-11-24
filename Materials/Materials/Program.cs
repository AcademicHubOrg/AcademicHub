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
app.MapGet("/materials/list", EndpointHandlers.ListOfCourseTemplates);
app.MapGet("/materials/{id}", EndpointHandlers.GetMaterialByMaterialId);
app.MapGet("/materials/{courseId}", EndpointHandlers.GetMaterialSByCourseId);
app.MapPost("/materials/add", EndpointHandlers.AddMaterial);

app.Run();



