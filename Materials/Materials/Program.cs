using CustomExceptions;
using Materials.Core;
using Microsoft.AspNetCore.Mvc;

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
app.MapGet("/material/list", ListOfCourseTemplates);
app.MapGet("/material/id", GetMaterialById);
app.MapGet("/material/course", GetMaterialSByCourse);
app.MapPost("/material/add", AddMaterial);

app.Run();

// Route implementations
string BaseUrl()
{
	return "Hello World!";
}

async Task<object> ListOfCourseTemplates([FromServices] MaterialService service)
{
	var result = await service.ListAsync();
	return new { Data = result };
}

static async Task<object> AddMaterial([FromServices] MaterialService service, MaterialDataDtoAdd material)
{
	if (string.IsNullOrEmpty(material.Name))
	{
		throw new ArgumentException("Invalid data provided. Material name is required.");
	}
	if (string.IsNullOrEmpty(material.CourseId))
	{
		throw new ArgumentException("Invalid data provided. CourseId is required.");
	}

	try
	{ 
		Convert.ToInt32(material.CourseId);
	}
	catch (Exception e)
	{
		throw new ArgumentException("Invalid data provided: CourseId.");
	}

	await service.AddAsync(material);
	return new { Message = "Material added successfully." };
}

static async Task<object> GetMaterialById([FromServices] MaterialService service, int? materialId)
{
	if (materialId is null)
	{
		throw new ArgumentException("Invalid data provided. Material Id is required.");
	}
	return await service.ListByIdAsync(materialId.Value);
}

static async Task<object> GetMaterialSByCourse([FromServices] MaterialService service, int? courseId)
{
	if (courseId is null)
	{
		throw new ArgumentException("Invalid data provided. Course Id is required.");
	}

	return await service.ListByCourseIdAsync(courseId.Value);
}

