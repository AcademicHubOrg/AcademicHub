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

var service = new CourseTemplateService();

app.MapGet("/", () => "Hello World!");
app.MapGet("/courseTemplate/list", async () => await service.ListAsync());
app.MapPost("/courseTemplate", async (CourseTemplateDto courseTemplate) =>
{
    await service.AddAsync(courseTemplate);
});

app.Run();