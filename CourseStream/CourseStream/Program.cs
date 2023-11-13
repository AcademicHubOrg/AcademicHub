using CourseStream.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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