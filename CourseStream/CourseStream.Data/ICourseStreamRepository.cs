namespace CourseStream.Data;

public interface ICourseStreamRepository
{
    Task AddAsync(CourseStream courseStream);
    Task<List<CourseStream>> ListAsync(int start_index =0, int how_many=10);
    Task EnrollStudentAsync(int studentId, int courseStreamId, DateTime currentTime);
    Task<bool> IsStudentEnrolledAsync(int studentId, int courseStreamId);
    Task<CourseStream?> GetByIdAsync(int id);
}