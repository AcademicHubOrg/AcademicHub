using System.Runtime.InteropServices.JavaScript;

namespace CourseStream.Data;

using Microsoft.EntityFrameworkCore;

public class CourseStreamRepository : ICourseStreamRepository
{
    private readonly CourseStreamDbContext _context;

    public CourseStreamRepository(CourseStreamDbContext context)
    {
        _context = context;
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
    public async Task EnrollStudentAsync(int studentId, int courseStreamId, DateTime currentTime)
    {
        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseStreamId = courseStreamId,
            EnrollmentTimestamp = currentTime
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseStreamId)
    {
        return await _context.Enrollments
            .AnyAsync(e =>  e.CourseStreamId == courseStreamId && e.StudentId == studentId );
    }

    public async Task<CourseStream?> GetByIdAsync(int id)
    {
        return await _context.CourseStreams.FindAsync(id);
    }

}