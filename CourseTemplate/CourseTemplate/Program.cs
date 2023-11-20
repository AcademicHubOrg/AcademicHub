using CourseTemplate.Core;
using CustomExceptions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CourseTemplateService>();
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

builder.Services.AddMvc(options =>
{
	options.Filters.Add<CustomExceptionFilter>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigin");

app.UseExceptionHandler(errorApp =>
{
	errorApp.Run(async context =>
	{
		var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
		var env = context.RequestServices.GetService<IWebHostEnvironment>();
		var errorCode = StatusCodes.Status500InternalServerError;
		context.Response.StatusCode = errorCode;
		context.Response.ContentType = "application/json";

		var errorMessage = "An unexpected error occurred. Please try again later.";
		
		if (exceptionHandlerPathFeature?.Error is ArgumentException)
		{
			errorMessage = "Invalid input provided.";
			errorCode = StatusCodes.Status400BadRequest;
		}
		else if (exceptionHandlerPathFeature?.Error is NotFoundException)
		{
			errorMessage = exceptionHandlerPathFeature.Error.Message;
			errorCode = StatusCodes.Status404NotFound;
		}
		else if (exceptionHandlerPathFeature?.Error is ConflictException)
		{
			errorMessage = exceptionHandlerPathFeature.Error.Message;
			errorCode = StatusCodes.Status409Conflict;
		}

		if (env.IsDevelopment())
		{
			// In development, return detailed error information
			errorMessage += " dev info: " + exceptionHandlerPathFeature?.Error.Message;
		}
		await context.Response.WriteAsJsonAsync(new 
		{ 
			ErrorMessage = errorMessage, 
			ErrorCode = errorCode 
		});
	});
});


// StatusCodePages Middleware
app.UseStatusCodePages(context =>
{
	context.HttpContext.Response.ContentType = "application/json";
	return context.HttpContext.Response.WriteAsJsonAsync(new { ErrorMessage = "An unexpected error occurred." });
});

// Routes
app.MapGet("/", BaseUrl);
app.MapGet("/courseTemplate/list", ListOfCourseTemplates);
app.MapPost("/courseTemplate/add", AddCourse);
app.MapGet("/courseTemplate/{id}", GetCourseById);

app.Run();

// Route implementations
string BaseUrl()
{
	return "Hello World!";
}

async Task<object> ListOfCourseTemplates([FromServices] CourseTemplateService service)
{
	var result = await service.ListAsync();
	return new { Data = result };
}

static async Task<object> AddCourse([FromServices] CourseTemplateService service, CreateCourseTemplateDto courseTemplate)
{
	if (string.IsNullOrEmpty(courseTemplate.Name))
	{
		throw new ArgumentException("Invalid data provided. Course name is required.");
	}

	await service.AddAsync(courseTemplate);
	return new { Message = "Course added successfully." };
}

async Task<object> GetCourseById([FromServices] CourseTemplateService service, int id)
{
	var course = await service.GetByIdAsync(id);
	return course;
}