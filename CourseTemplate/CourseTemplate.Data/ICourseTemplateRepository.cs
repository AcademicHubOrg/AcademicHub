namespace CourseTemplate.Data;

public interface ICourseTemplateRepository
{
    Task<List<CourseTemplate>> ListAsync();
    Task AddAsync(CourseTemplate courseTemplate);
    Task<CourseTemplate?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}