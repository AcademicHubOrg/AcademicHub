using CourseTemplate.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var service = new CourseTemplateService();

app.MapGet("/", () => "Hello World!");

app.MapGet("/courseTemplate/list", async () => await service.ListAsync());
app.MapPost("/courseTemplate", async (CourseTemplateDto courseTemplate) =>
{
	await service.AddAsync(courseTemplate);
});

app.Run();