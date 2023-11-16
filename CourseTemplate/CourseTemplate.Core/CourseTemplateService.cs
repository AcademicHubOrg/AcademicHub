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
		if (course.Name == null)
		{
			throw new Exception("Invalid data provided");
		}
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