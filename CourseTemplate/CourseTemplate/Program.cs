using CourseTemplate.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services with a specific policy
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder =>
		{
			builder.WithOrigins("http://localhost:3000")
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

var service = new CourseTemplateService();

app.MapGet("/", BaseUrl);
app.MapGet("/courseTemplate/list", ListOfCourseTemplates);
app.MapPost("/courseTemplate/add", AddCourse);

app.Run();

string BaseUrl()
{
	return "Hello World!";
}

async Task<object> ListOfCourseTemplates()
{
	try
	{
		var result = await service.ListAsync();
		return new { Success = true, Data = result };
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Failed to retrieve course templates. Error: {ex.Message}");
		return new { Success = false, ErrorMessage = $"Failed to retrieve course templates. Error: {ex.Message}" };
	}
}

async Task<object> AddCourse(CourseTemplateDto courseTemplate)
{
	try
	{
		await service.AddAsync(courseTemplate);
		return new { Success = true, Message = "Course added successfully." };
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Failed to add course. Error: {ex.Message}");
		return new { Success = false, ErrorMessage = $"Failed to add course. Error: {ex.Message}" };
	}
}