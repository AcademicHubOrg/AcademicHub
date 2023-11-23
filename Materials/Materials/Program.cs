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
			builder.WithOrigins("https://localhost:3000") // Replace with the actual origin of your frontend
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

app.MapGet("/", EndpointHandlers.BaseUrl);
app.MapGet("/material/list", EndpointHandlers.ListOfCourseTemplates);
app.MapGet("/material/id", EndpointHandlers.GetMaterialByMaterialId);
app.MapGet("/material/course", EndpointHandlers.GetMaterialSByCourseId);
app.MapPost("/material/add", EndpointHandlers.AddMaterial);

app.Run();



