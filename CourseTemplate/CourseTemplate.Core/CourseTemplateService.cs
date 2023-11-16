using System.Security.Cryptography;

namespace CourseTemplate.Core;

using CourseTemplate.Data;


public class CourseTemplateDto
{
	public string Name { get; set; } = null!;
}
public class CourseTemplateViewDto : CourseTemplateDto
{
	public string Id { get; set; } = null!;
}

public class CourseTemplateService
{
	private readonly CourseTemplateRepository _repository;
	public CourseTemplateService()
	{
		_repository = new CourseTemplateRepository();
	}
  
	public async Task AddAsync(CourseTemplateDto course)
	{
		var dbCourseTemplates = await _repository.ListAsync();

		// Check if a course with the same name already exists
		if (dbCourseTemplates.Any(c => c.CourseName == course.Name))
		{
			throw new ArgumentException($"A course with the name '{course.Name}' already exists.");
		}

		// If not, add the new course
		await _repository.AddAsync(new CourseTemplate()
		{
			CourseName = course.Name
		});
	}


	public async Task<List<CourseTemplateViewDto>> ListAsync()
	{
		var result = new List<CourseTemplateViewDto>();
		var dbCourseTemplates = await _repository.ListAsync();
		foreach (var courseTemplate in dbCourseTemplates)
		{
			result.Add(new CourseTemplateViewDto()
			{
				Name = courseTemplate.CourseName,
				Id = courseTemplate.Id.ToString(),
			});
		}
		return result;
	}
}