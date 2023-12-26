
using CourseStream.Core;
using CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using CustomExceptions;

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
		await service.EnrollStudentAsync(studentId, courseStreamId);
		return new { Message = "Enrolment successful." };
	}

	public static async Task<object> GetCourseStreamById([FromServices] CourseStreamService service, int id)
	{
		var course = await service.GetByIdAsync(id);
		return course;
	}
	
	public static async Task<object> DeleteCourseStream([FromServices] CourseStreamService service,[FromServices]IHttpClientFactory httpClientFactory,  int streamId)
	{
		var client = httpClientFactory.CreateClient();
		var response = await client.DeleteAsync("http://materials:80/materials/delete-by-course-id/" + streamId);
		if (!response.IsSuccessStatusCode)
		{
			throw new ArgumentException("Invalid data provided. Course Stream does not exist.");
		}
		
		try
		{
			Console.WriteLine("pudge");
			await service.DeleteCourseStreamAsync(streamId);
			return new { Message = "Course stream and associated materials deleted successfully." };
		}
		
		
		catch (NotFoundException ex)
		{
			return new { Error = ex.Message };
		}
		catch (Exception ex)
		{
			return new { Error = "An error occurred while processing the request." };
		}
	}
    
	public static async Task<object> DeleteAllStreamsByTemplateId([FromServices] CourseStreamService service,[FromServices]IHttpClientFactory httpClientFactory, int id)
	{
		var client = httpClientFactory.CreateClient();
		/*var response = await client.DeleteAsync("http://materials:80/materials/delete-by-course-id/" + id);#1#*/
		/*if (!response.IsSuccessStatusCode)
		{
			throw new ArgumentException("Invalid data provided. Course Template does not exist.");
		}*/
	
		try
		{
			List<int> CourseStreams = await service.DeleteAllStreamsByTemplateId(id );
			foreach (var i in CourseStreams)
			{
				var response = await client.DeleteAsync("http://materials:80/materials/delete-by-course-id/" + i);
			}
			return new { Message = "Essential material deleted successfully." };
			
			
		}
		catch (NotFoundException ex)
		{
			return new { Error = ex.Message };
		}
	}
	
}