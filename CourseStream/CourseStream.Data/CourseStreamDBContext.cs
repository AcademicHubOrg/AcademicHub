using Microsoft.EntityFrameworkCore;
namespace CourseStream.Data;

public class CourseStreamDBContext : DbContext
{
    public DbSet<CourseStream> CourseStreams { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public CourseStreamDBContext()
    {
        
    }
    public CourseStreamDBContext(DbContextOptions<CourseStreamDBContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=CourseStream;User Id=coursestreamuser;Password=coursestreamuser;Port=5437;");
        base.OnConfiguring(optionsBuilder);
    }
    
}