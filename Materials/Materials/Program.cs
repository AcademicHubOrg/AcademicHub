using Materials.Core;

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

var service = new MaterialService();

app.MapGet("/", () => "Hello World!");
app.MapGet("/material/list", async () => await service.ListAsync());
app.MapGet("/material/id", async (int materialId) => await service.ListByIdAsync(materialId));
app.MapGet("/material/course", async (int courseId) => await service.ListByCourseIdAsync(courseId));
app.MapPost("/material/add", async (MaterialDataDto materialData) => { await service.AddAsync(materialData);});

app.Run();
