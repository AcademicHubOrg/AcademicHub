namespace CourseTemplate.Data;

using Microsoft.EntityFrameworkCore;

public class CourseTemplateRepository
{
	private readonly CourseTemplateDbContext _context;

	public CourseTemplateRepository()
	{
		_context = new CourseTemplateDbContext();
	}
  
	public async Task AddAsync(CourseTemplate courseTemplate)
	{
		_context.Add(courseTemplate);
		await _context.SaveChangesAsync();
	}

	public async Task<List<CourseTemplate>> ListAsync()
	{
		return await _context.CourseTemplates.ToListAsync();
	}
	
	public async Task<CourseTemplate?> GetByIdAsync(int id)
	{
		return await _context.CourseTemplates.FindAsync(id);
	}
}