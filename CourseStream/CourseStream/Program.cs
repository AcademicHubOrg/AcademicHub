using CourseStream.Core;
using CustomExceptions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CourseStreamService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
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
			errorMessage = exceptionHandlerPathFeature.Error.Message;
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
			errorMessage += " dev info: " + exceptionHandlerPathFeature?.Error.InnerException;
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
app.MapGet("/courseStream/list", ListOfCourseStreams);
app.MapPost("/courseStream/add", AddCourse);
app.MapPost("courseStream/enroll", EnrollStudent);
app.Run();

// Route implementations
string BaseUrl()
{
	return "Hello World!";
}

async Task<object> ListOfCourseStreams([FromServices] CourseStreamService service)
{
	var result = await service.ListAsync();
	return new { Data = result };
}

static async Task<object> AddCourse([FromServices] CourseStreamService service, CourseStreamAddDto courseStream)
{
	if (string.IsNullOrEmpty(courseStream.Name))
	{
		throw new ArgumentException("Invalid data provided. Course name is required.");
	}

	await service.AddAsync(courseStream);
	return new { Message = "Course added successfully." };
}

async Task<object> EnrollStudent([FromServices] CourseStreamService service, string studentId, string courseStreamId)
{
	if (string.IsNullOrWhiteSpace(studentId))
	{
		throw new ArgumentException("Invalid data provided. Student Id is required.");
	}
	if (string.IsNullOrWhiteSpace(courseStreamId))
	{
		throw new ArgumentException("Invalid data provided. Course Id is required.");
	}
	int.TryParse(studentId, out var argStudentId);
	int.TryParse(courseStreamId, out var argCourseStreamId);
	await service.EnrollStudentAsync(argStudentId, argCourseStreamId);
	return new { Message = "Enrolment successful." };
}



