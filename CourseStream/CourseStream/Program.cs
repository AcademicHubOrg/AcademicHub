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

// Routes

app.MapGet("/healthz", EndpointHandlers.HealthCheck);
app.MapGet("/courseStreams/list", EndpointHandlers.ListOfCourseStreams);
app.MapGet("/courseStreams/{id}", EndpointHandlers.GetCourseStreamById);
app.MapGet("courseStreams/checkEnrollments/{courseId}", EndpointHandlers.CheckEnrollments);
app.MapGet("courseStreams/getEnrolledCourses/{studentId}", EndpointHandlers.GetEnrollmentsByStudent);
app.MapPost("/courseStreams/add", EndpointHandlers.AddCourse);
app.MapPost("courseStreams/enroll", EndpointHandlers.EnrollStudent);
app.MapDelete("/courseStreams/delete/{streamId}", EndpointHandlers.DeleteCourseStream);
app.MapDelete("/courseStreams/delete-all-by/{courseTemplateId}", EndpointHandlers.DeleteAllStreamsByTemplateId);
app.MapDelete("/courseStreams/unenroll/{studentId}/{courseStreamId}", EndpointHandlers.UnEnroll);
// Apply EF Core Migrations

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CourseStreamDbContext>();
    dbContext.Database.Migrate();
}

app.Run();