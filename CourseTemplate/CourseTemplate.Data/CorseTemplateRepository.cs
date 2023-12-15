namespace CourseTemplate.Data;

using Microsoft.EntityFrameworkCore;

public class CourseTemplateRepository : ICourseTemplateRepository
{
	private readonly CourseTemplateDbContext _context;

	public CourseTemplateRepository(CourseTemplateDbContext context)
	{
		_context = context;
	}

	public async Task AddAsync(CourseTemplate courseTemplate)
	{
		_context.Add(courseTemplate);
		await _context.SaveChangesAsync();
	}

	public async Task<List<CourseTemplate>> ListAsync(int start_index = 0, int how_many = 10)
	{
		return await _context.CourseTemplates
			.Skip(start_index)
			.Take(how_many)
			.ToListAsync();
	}

	public async Task<CourseTemplate?> GetByIdAsync(int id)
	{
		return await _context.CourseTemplates.FindAsync(id);
	}
}