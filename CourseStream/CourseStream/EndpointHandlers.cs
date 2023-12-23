using CourseStream.Core;
using CustomExceptions;
using Microsoft.AspNetCore.Mvc;


namespace CourseStream;


internal static  class EndpointHandlers
{
	public static IResult HealthCheck()
	{
		return Results.Ok();
	}

	public static async Task<object> ListOfCourseStreams([FromServices] CourseStreamService service)
	{
		var result = await service.ListAsync();
		return new { Data = result };
	}

	public static async Task<object> CheckEnrollments([FromServices] CourseStreamService service, int courseId)
	{
		var result = await service.GetEnrollments(courseId);
		return new { Data = result };
	}

	public static async Task<object> AddCourse([FromServices] CourseStreamService service, [FromServices]IHttpClientFactory httpClientFactory , CourseStreamAddDto courseStream)
	{
		var client = httpClientFactory.CreateClient();
		// Use the service name and internal port here
		var response = await client.GetAsync("http://course-template:80/courseTemplates/" + courseStream.TemplateId);
		if (!response.IsSuccessStatusCode)
		{
			throw new ArgumentException("Invalid data provided. Template does not exist.");
		}
		if (string.IsNullOrEmpty(courseStream.Name))
		{
			throw new ArgumentException("Invalid data provided. Course name is required.");
		}
		await service.AddAsync(courseStream);
		return new { Message = "Course added successfully." };
	}
	
	public static async Task<object> EnrollStudent([FromServices] CourseStreamService service, int studentId, int courseStreamId)
	{
		try
		{
			await service.EnrollStudentAsync(studentId, courseStreamId);
			return new { Message = "Enrollment successful." };
		}
		catch (ConflictException ex)
		{
			return new { Message = ex.Message };
		}
	}


	public static async Task<object> GetCourseStreamById([FromServices] CourseStreamService service, int id)
	{
		var course = await service.GetByIdAsync(id);
		return course;
	}
}