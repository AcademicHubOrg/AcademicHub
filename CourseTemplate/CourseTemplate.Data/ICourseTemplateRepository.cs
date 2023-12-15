namespace CourseTemplate.Data;

public interface ICourseTemplateRepository
{
    Task<List<CourseTemplate>> ListAsync(int start_index, int how_many);
    Task AddAsync(CourseTemplate courseTemplate);
    Task<CourseTemplate?> GetByIdAsync(int id);
}