using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CourseTemplate.Core.CustomExceptions;

public class CustomExceptionFilter : IExceptionFilter
{
	private readonly ILogger<CustomExceptionFilter> _logger;

	public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		
		var exception = context.Exception;
		_logger.LogError(exception, "An unhandled exception occurred.");

		
		var response = new
		{
			ErrorMessage = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" 
				? exception.Message 
				: "An error occurred. Please try again later.",
		};

		int statusCode = exception switch
		{
			NotFoundException _ => StatusCodes.Status404NotFound,
			ConflictException _ => StatusCodes.Status409Conflict
		};

		context.Result = new ObjectResult(response)
		{
			StatusCode = statusCode
		};
	}
}