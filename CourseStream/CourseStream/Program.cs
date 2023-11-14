using CourseStream.Core;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Adjust this as per your React app's URL
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use CORS policy
app.UseCors("MyAllowSpecificOrigins");

app.UseSwagger();
app.UseSwaggerUI();

var service = new CourseStreamService();

app.MapGet("/", () => "Hello World!");

app.MapGet("/courseStream/list", async () => await service.ListAsync());
app.MapPost("/courseStream", async (CourseStreamDto courseStream) =>
{
    await service.AddAsync(courseStream);
});

app.Run();