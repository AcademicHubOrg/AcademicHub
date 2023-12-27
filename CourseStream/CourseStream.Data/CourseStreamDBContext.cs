using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CourseStream.Data;

public class CourseStreamDbContext : DbContext
{
    public DbSet<CourseStream> CourseStreams { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    
    public CourseStreamDbContext(DbContextOptions options)
        : base(options)
    {
    }
    // Design factory for migrations
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CourseStreamDbContext>
    {
        public CourseStreamDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CourseStreamDbContext>();
            optionsBuilder.UseNpgsql("Host=postgres; Database=Identity; User Id=identityuser; Password=identityuser; Port=5432");

            return new CourseStreamDbContext(optionsBuilder.Options);
        }
    }
}