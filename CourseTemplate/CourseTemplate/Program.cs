using CourseTemplate.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CourseTemplateService>();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services with a specific policy
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

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Use the CORS policy
app.UseCors("AllowSpecificOrigin");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        context.Response.StatusCode = 500;

        // Check if the exception is of type ArgumentException
        if (exceptionHandlerPathFeature?.Error is ArgumentException)
        {
            context.Response.StatusCode = 400; // Bad Request
            await context.Response.WriteAsJsonAsync(new { ErrorMessage = exceptionHandlerPathFeature.Error.Message });
        }
        else
        {
            await context.Response.WriteAsJsonAsync(new { ErrorMessage = "An unexpected error occurred. Please try again later." });
        }
    });
});

app.UseStatusCodePages(async context =>
{
    context.HttpContext.Response.ContentType = "application/json";

    if (context.HttpContext.Response.StatusCode == 404)
    {
        await context.HttpContext.Response.WriteAsJsonAsync(new { ErrorMessage = "Resource not found." });
    }
    else
    {
        await context.HttpContext.Response.WriteAsJsonAsync(new { ErrorMessage = "An unexpected error occurred." });
    }
});

app.MapGet("/", BaseUrl);
app.MapGet("/courseTemplate/list", ListOfCourseTemplates);
app.MapPost("/courseTemplate/add", AddCourse);

app.Run();


string BaseUrl()
{
    return "Hello World!";
}

async Task<object> ListOfCourseTemplates([FromServices] CourseTemplateService service)
{
    var result = await service.ListAsync();
    return new { Data = result };
}

static async Task<object> AddCourse([FromServices] CourseTemplateService service, CourseTemplateDto courseTemplate)
{
    if (courseTemplate == null || string.IsNullOrEmpty(courseTemplate.Name))
    {
        throw new ArgumentException("Invalid data provided. Course name is required.");
    }

    await service.AddAsync(courseTemplate);
    return new { Message = "Course added successfully." };
    
}
