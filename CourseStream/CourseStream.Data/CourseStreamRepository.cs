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
}