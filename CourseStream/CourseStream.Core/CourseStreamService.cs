using System.Security.Cryptography;
using CustomExceptions;

namespace CourseStream.Core;

using CourseStream.Data;


public class CourseStreamShowDto
{
	public string Name { get; set; } = null!;
	public string Id { get; set; } = null!;
	public string TemplateId { get; set; } = null!;
}

public class CourseStreamAddDto
{
	public string Name { get; set; } = null!;
	public int TemplateId { get; set; }
}


public class CourseStreamService
{
	private readonly ICourseStreamRepository _repository;
	
	public CourseStreamService(ICourseStreamRepository repository)
	{
		_repository = repository;
		
	}

	public async Task AddAsync(CourseStreamAddDto courseStream)
	{
		var dbCourseStreams = await _repository.ListAsync();
		if (dbCourseStreams.Any(m => m.CourseName == courseStream.Name))
		{
			throw new ConflictException($"Course with name '{courseStream.Name}' already exists");
		}
		await _repository.AddAsync(new CourseStream()
		{
			CourseName = courseStream.Name,
			TemplateId = courseStream.TemplateId
		});
	}

	
	public async Task EnrollStudentAsync(int studentId, int courseStreamId)
	{
		if (await _repository.IsStudentEnrolledAsync(studentId, courseStreamId))
		{
			throw new ConflictException("Student is already enrolled in this course stream.");
		}

		var currentTime = DateTime.UtcNow;
		await _repository.EnrollStudentAsync(studentId, courseStreamId, currentTime);
	}


	public async Task<List<CourseStreamShowDto>> ListAsync()
	{
		var result = new List<CourseStreamShowDto>();
		var dbCourseStreams = await _repository.ListAsync();
		foreach (var courseStream in dbCourseStreams)
		{
			result.Add(new CourseStreamShowDto()
			{
				Name = courseStream.CourseName,
				Id = courseStream.Id.ToString(),
				TemplateId = courseStream.TemplateId.ToString()
			});
		}
		return result;
	}

	public async Task<CourseStreamShowDto> GetByIdAsync(int id)
	{
		var courseStream = await _repository.GetByIdAsync(id);
		if (courseStream == null)
		{
			throw new NotFoundException($"Course with ID '{id}'");
		}
		return new CourseStreamShowDto()
		{
			Name = courseStream.CourseName,
			Id = courseStream.Id.ToString(),
			TemplateId = courseStream.TemplateId.ToString()
		};
	}
	
	public async Task DeleteCourseStreamAsync(int streamId)
	{
		var courseStream = await _repository.GetByIdAsync(streamId);

		if (courseStream == null)
		{
			throw new NotFoundException($"Course stream with ID '{streamId}' not found.");
		}
		
		await _repository.DeleteAsync(courseStream);
	}
	
}