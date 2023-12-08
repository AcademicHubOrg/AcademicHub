using Microsoft.EntityFrameworkCore;

namespace CourseTemplate.Data;

public class CourseTemplateDbContext : DbContext
{
	public DbSet<CourseTemplate> CourseTemplates { get; set; } = null!;
	public CourseTemplateDbContext(DbContextOptions options)
		: base(options)
	{
	}
	
}



