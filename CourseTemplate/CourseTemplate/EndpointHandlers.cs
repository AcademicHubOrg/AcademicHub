using CourseTemplate.Core;
using CustomExceptions;
namespace CourseTemplate;
using Microsoft.AspNetCore.Mvc;

internal static class EndpointHandlers
{
	public static IResult HealthCheck()
	{
		return Results.Ok();
	}
	
	public static async Task<object> ListOfCourseTemplates([FromServices] CourseTemplateService service)
	{
		var result = await service.ListAsync();
		return new { Data = result };
	}

	public static async Task<object> AddCourse([FromServices] CourseTemplateService service, CreateCourseTemplateDto courseTemplate)
	{
		if (string.IsNullOrEmpty(courseTemplate.Name))
		{
			throw new ArgumentException("Invalid data provided. Course name is required.");
		}

		await service.AddAsync(courseTemplate);
		return new { Message = "Course added successfully." };
	}

	public static async Task<object> GetCourseById([FromServices] CourseTemplateService service, int id)
	{
		var course = await service.GetByIdAsync(id);
		return course;
	}
	
	public static async Task<object> DeleteCourseTemplate([FromServices] CourseTemplateService service, int id)
	{
		try
		{
			await service.DeleteCourseAsync(id);
			return new { Message = "Course template deleted successfully." };
		}
		catch (NotFoundException ex)
		{
			return new { Error = ex.Message };
		}
		catch (Exception ex)
		{
			return new { Error = "An error occurred while deleting the course template." };
		}
	}
}