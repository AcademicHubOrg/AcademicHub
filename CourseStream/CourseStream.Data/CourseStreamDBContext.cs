using Microsoft.EntityFrameworkCore;
namespace CourseStream.Data;

public class CourseStreamDbContext : DbContext
{
    public DbSet<CourseStream> CourseStreams { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    
    public CourseStreamDbContext(DbContextOptions options)
        : base(options)
    {
    }
}