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

	public static async Task<object> AddCourse([FromServices] CourseStreamService service, [FromServices]IHttpClientFactory httpClientFactory , CourseStreamAddDto courseStream)
	{
		var client = httpClientFactory.CreateClient();
		var response = await client.GetAsync("https://localhost:5204/courseTemplates/" + courseStream.TemplateId);
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
		await service.EnrollStudentAsync(studentId, courseStreamId);
		return new { Message = "Enrolment successful." };
	}
}