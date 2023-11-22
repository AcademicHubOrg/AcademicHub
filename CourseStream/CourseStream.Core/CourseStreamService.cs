using System.Security.Cryptography;
using CustomExceptions;

namespace CourseStream.Core;

using CourseStream.Data;


public class CourseStreamShowDto
{
	public string Name { get; set; } = null!;
	public string Id { get; set; } = null!;
}

public class CourseStreamAddDto
{
	public string Name { get; set; } = null!;
}

public abstract class EnrolledStudentsDto
{
	public string Id { get; set; } = null!;
}

public class CourseStreamService
{
	private readonly CourseStreamRepository _repository;
	public CourseStreamService()
	{
		_repository = new CourseStreamRepository();
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
			CourseName = courseStream.Name
		});
	}
	
	public async Task EnrollStudentAsync(int studentId, int courseStreamId)
	{
		if (await _repository.IsStudentEnrolledAsync(studentId, courseStreamId))
		{
			throw new ConflictException("Student is already enrolled in this course stream.");
		}

		await _repository.EnrollStudentAsync(studentId, courseStreamId);
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
			});
		}
		return result;
	}
}