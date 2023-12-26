using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CourseTemplate.Data;

public class CourseTemplateDbContext : DbContext
{
	public DbSet<CourseTemplate> CourseTemplates { get; set; } = null!;
	public CourseTemplateDbContext(DbContextOptions options)
		: base(options)
	{
	}
        // Design factory for migrations
        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CourseTemplateDbContext>
        {
            public CourseTemplateDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<CourseTemplateDbContext>();
                optionsBuilder.UseNpgsql("Host=postgres; Database=Identity; User Id=identityuser; Password=identityuser; Port=5432");
    
                return new CourseTemplateDbContext(optionsBuilder.Options);
            }
        }
	
}



