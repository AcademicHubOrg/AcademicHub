using System.Runtime.InteropServices.JavaScript;
/*using CourseStream.Core;
using CustomExceptions;
using Microsoft.AspNetCore.Mvc;*/

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
    
    

    public async Task DeleteAsync(CourseStream courseStream)
    {
        _context.CourseStreams.Remove(courseStream);
        await _context.SaveChangesAsync();
    }
    public async Task <List <int> > DeleteAllStreamsByTemplateId(int templateId )
    {
        var streamsToDelete = await _context.CourseStreams
            .Where(CourseStream => CourseStream.TemplateId == templateId)
            .ToListAsync();


        List<int> streamsId = new List<int>();
        
        for (var i = 0; i < streamsToDelete.Count; i++)
        {
            streamsId.Add(streamsToDelete[i].Id);
            
        }
        
        _context.CourseStreams.RemoveRange(streamsToDelete);
        await _context.SaveChangesAsync();
        return streamsId;
    }
    
    
}