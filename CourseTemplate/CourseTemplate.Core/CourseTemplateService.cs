using System.Security.Cryptography;

namespace CourseTemplate.Core;

using CourseTemplate.Data;


public class CourseTemplateDto
{
	public string Name { get; set; } = null!;
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
		await _repository.AddAsync(new CourseTemplate()
		{
			CourseName = course.Name
		});
	}

	public async Task<List<CourseTemplateDto>> ListAsync()
	{
		var result = new List<CourseTemplateDto>();
		var dbCourseTemplates = await _repository.ListAsync();
		foreach (var courseTemplate in dbCourseTemplates)
		{
			result.Add(new CourseTemplateDto()
			{
				Name = courseTemplate.CourseName,
				Id = courseTemplate.Id.ToString(),
			});
		}
		return result;
	}
}