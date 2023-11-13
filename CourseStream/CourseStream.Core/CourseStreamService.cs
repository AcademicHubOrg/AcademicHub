using System.Security.Cryptography;

namespace CourseStream.Core;

using CourseStream.Data;


public class CourseStreamDto
{
    public string Name { get; set; } = null!;
    public string Id { get; set; } = null!;
}

public class CourseStreamService
{
    private readonly CourseStreamRepository _repository;
    public CourseStreamService()
    {
        _repository = new CourseStreamRepository();
    }

    public async Task AddAsync(CourseStreamDto user)
    {
        await _repository.AddAsync(new CourseStream()
        {
            CourseName = user.Name
        });
    }

    public async Task<List<CourseStreamDto>> ListAsync()
    {
        var result = new List<CourseStreamDto>();
        var dbCourseStreams = await _repository.ListAsync();
        foreach (var courseStream in dbCourseStreams)
        {
            result.Add(new CourseStreamDto()
            {
                Name = courseStream.CourseName,
                Id = courseStream.Id.ToString(),
            });
        }
        return result;
    }
}