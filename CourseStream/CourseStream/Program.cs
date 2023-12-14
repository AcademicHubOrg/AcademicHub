using CourseStream;
using CourseStream.Core;
using CourseStream.Data;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Access configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure your DbContext
builder.Services.AddDbContext<CourseStreamDbContext>(options =>
	options.UseNpgsql(connectionString));
builder.Services.AddScoped<CourseStreamService>();
builder.Services.AddScoped<ICourseStreamRepository, CourseStreamRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		corsPolicyBuilder =>
		{
			corsPolicyBuilder.WithOrigins("http://academichub.net/") // Replace with the actual origin of your frontend
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

// Routes

app.MapGet("/healthz", EndpointHandlers.HealthCheck);
app.MapGet("/courseStreams/list", EndpointHandlers.ListOfCourseStreams);
app.MapGet("/courseStreams/{id}", EndpointHandlers.GetCourseStreamById);
app.MapPost("/courseStreams/add", EndpointHandlers.AddCourse);
app.MapPost("courseStreams/enroll", EndpointHandlers.EnrollStudent);

// Apply EF Core Migrations

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<CourseStreamDbContext>();
	dbContext.Database.Migrate();
}

app.Run();
