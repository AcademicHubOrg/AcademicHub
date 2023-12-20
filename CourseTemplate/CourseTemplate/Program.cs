using CourseTemplate;
using CourseTemplate.Core;
using CourseTemplate.Data;
using CustomExceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Access configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure your DbContext
builder.Services.AddDbContext<CourseTemplateDbContext>(options =>
	options.UseNpgsql(connectionString));
builder.Services.AddScoped<CourseTemplateService>();
builder.Services.AddScoped<ICourseTemplateRepository, CourseTemplateRepository>();

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
			corsPolicyBuilder.WithOrigins(Address.academichuburl) // Replace with the actual origin of your frontend
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
app.MapGet("/courseTemplates/list", EndpointHandlers.ListOfCourseTemplates);
app.MapPost("/courseTemplates/add", EndpointHandlers.AddCourse);
app.MapGet("/courseTemplates/{id}", EndpointHandlers.GetCourseById);
app.MapDelete("/courseDeleteTemplates/{id}", EndpointHandlers.DeleteCourseTemplate);
// Apply EF Core Migrations
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<CourseTemplateDbContext>();
	dbContext.Database.Migrate();
}

app.Run();
