namespace CourseStream.Data;

using Microsoft.EntityFrameworkCore;

public class CourseStreamRepository
{
    private readonly CourseStreamDBContext _context;

    public CourseStreamRepository()
    {
        _context = new CourseStreamDBContext();
    }

    public async Task AddAsync(CourseStream courseStream)
    {
        _context.Add(courseStream);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CourseStream>> ListAsync()
    {
        return await _context.CourseStreams.ToListAsync();
    }
    public async Task EnrollStudentAsync(int studentId, int courseStreamId)
    {
        var enrollment = new EnrolledStudent
        {
            StudentId = studentId,
            CourseStreamId = courseStreamId
        };

        _context.EnrolledStudents.Add(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseStreamId)
    {
        return await _context.EnrolledStudents
            .AnyAsync(e => e.StudentId == studentId && e.CourseStreamId == courseStreamId);
    }

}