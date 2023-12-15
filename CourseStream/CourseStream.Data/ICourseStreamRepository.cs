namespace CourseStream.Data;

public interface ICourseStreamRepository
{
    Task AddAsync(CourseStream courseStream);
    Task<List<CourseStream>> ListAsync(int start_index, int how_many);
    Task EnrollStudentAsync(int studentId, int courseStreamId, DateTime currentTime);
    Task<bool> IsStudentEnrolledAsync(int studentId, int courseStreamId);
    Task<CourseStream?> GetByIdAsync(int id);
}