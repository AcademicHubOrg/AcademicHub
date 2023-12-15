namespace CourseTemplate.Data;

public interface ICourseTemplateRepository
{
    Task<List<CourseTemplate>> ListAsync(int start_index =0, int how_many=10);
    Task AddAsync(CourseTemplate courseTemplate);
    Task<CourseTemplate?> GetByIdAsync(int id);
}