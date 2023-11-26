using Microsoft.EntityFrameworkCore;

namespace CourseTemplate.Data;

public class CourseTemplateDbContext : DbContext
{
	public DbSet<CourseTemplate> CourseTemplates { get; set; }
	public CourseTemplateDbContext()
	{
		
	}
  
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql("Host=localhost;Database=CourseTemplate;User Id=coursetemplateuser;Password=coursetemplateuser;Port=5438;");
		base.OnConfiguring(optionsBuilder);
	}
}



