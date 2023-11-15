using Microsoft.EntityFrameworkCore;
namespace CourseStream.Data;

public class CourseStreamDBContext : DbContext
{
    public DbSet<CourseStream> CourseStreams { get; set; }
    public CourseStreamDBContext()
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=CourseStream;User Id=coursestreamuser;Password=coursestreamuser;Port=5437;");
        base.OnConfiguring(optionsBuilder);
    }
    
}