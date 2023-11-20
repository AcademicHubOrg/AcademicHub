using CustomExceptions;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authentication.Google;
using Identity.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UsersService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		builder =>
		{
			builder.WithOrigins("https://localhost:3000") // Replace with the actual origin of your frontend
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

builder.Services.AddMvc(options =>
{
	options.Filters.Add<CustomExceptionFilter>();
});

// Register authentication services
builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
	})
	.AddCookie(options =>
	{
		options.LoginPath = "/login"; // Specify the login path
	})
	.AddGoogle(googleOptions =>
	{
		googleOptions.ClientId = "989838419075-cmepplaro69du10sdlftsr5f8u58gj7p.apps.googleusercontent.com";
		googleOptions.ClientSecret = "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";
		googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; 

	});


// Register authorization services
builder.Services.AddAuthorization();

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


app.UseRouting(); // This must come before UseAuthentication and UseAuthorization
app.UseAuthentication();
app.UseAuthorization();


// Define your endpoints here
app.MapGet("/", BaseUrl);
app.MapGet("/user/list", ListOfUsers);
app.MapPost("/user/add", AddUser);
app.MapPost("/user/makeAdmin", MakeAdmin);
app.MapGet("/login", Login);
app.MapGet("/after-signin", AfterSignin);

app.Run();


// Route implementations
string BaseUrl()
{
	return "Hello World!";
}

async Task<object> ListOfUsers([FromServices] UsersService service)
{
	var result = await service.ListAsync();
	return new { Data = result };
}

async Task<object> AddUser([FromServices] UsersService service, UserDto user)
{
	
	if (string.IsNullOrEmpty(user.Name))
	{
		throw new ArgumentException("Invalid data provided. User name is required.");
	}
	if (string.IsNullOrEmpty(user.Email))
	{
		throw new ArgumentException("Invalid data provided. User email is required.");
	}

	await service.AddAsync(user);
	return new { Message = "User added successfully." };
}

async Task<object> MakeAdmin([FromServices] UsersService service, string email, string password)
{
	if (string.IsNullOrEmpty(email))
	{
		throw new ArgumentException("Invalid data provided. Email is required.");
	}

	if (string.IsNullOrEmpty(password))
	{
		throw new ArgumentException("Invalid data provided. Password is required.");
	}

	await service.MakeAdminAsync(email, password);
	return new {Message = "Admin assigned successfully."};
}

async Task<object> Login(HttpContext httpContext)
{
	// Challenge Google authentication, which will redirect to Google's login page
	return httpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
	{
		RedirectUri = "/after-signin" // Set the redirect URI to your callback endpoint
	});
};

async Task<object> AfterSignin(HttpContext httpContext)
{
	var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

	if (!authenticateResult.Succeeded)
		return Results.Redirect("/login"); // Redirect to login if not authenticated

	// Assuming you have a method to find or create the user
	var email = authenticateResult.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
	var name = authenticateResult.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;

	// Use your UsersService to find or create the user
	var userService = httpContext.RequestServices.GetRequiredService<UsersService>();
	var user = await userService.FindOrCreateUser(email!, name!);

	// Sign in the user with your own application logic if needed
	// ...

	// Redirect to the default page after login
	return Results.Redirect("/"); // Replace with your default page URL
};
