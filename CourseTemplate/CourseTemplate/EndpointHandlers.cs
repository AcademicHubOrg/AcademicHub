using CourseTemplate.Core;

namespace CourseTemplate;
using Microsoft.AspNetCore.Mvc;

internal static class EndpointHandlers
{
	public static string BaseUrl()
	{
		return "Hello World!";
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
}