using CourseStream.Core;
using Microsoft.AspNetCore.Mvc;

namespace CourseStream;

internal static  class EndpointHandlers
{
	public static string BaseUrl()
	{
		return "Hello World!";
	}

	public static async Task<object> ListOfCourseStreams([FromServices] CourseStreamService service)
	{
		var result = await service.ListAsync();
		return new { Data = result };
	}

	public static async Task<object> AddCourse([FromServices] CourseStreamService service, CourseStreamAddDto courseStream)
	{
		if (string.IsNullOrEmpty(courseStream.Name))
		{
			throw new ArgumentException("Invalid data provided. Course name is required.");
		}

		await service.AddAsync(courseStream);
		return new { Message = "Course added successfully." };
	}
}