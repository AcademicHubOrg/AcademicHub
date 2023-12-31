namespace CourseStream.Data;

public interface ICourseStreamRepository
{
    Task AddAsync(CourseStream courseStream);
    Task<List<CourseStream>> ListAsync();
    Task EnrollStudentAsync(int studentId, int courseStreamId, DateTime currentTime);
    Task<bool> IsStudentEnrolledAsync(int studentId, int courseStreamId);
    Task<CourseStream?> GetByIdAsync(int id);
    Task<List<Enrollment>> GetEnrollments(int courseId);
    Task<List<Enrollment>> GetEnrollmentsByStudent(int studentId);
    Task DeleteAsync(CourseStream courseStream);
    
    Task <List<int>> DeleteAllStreamsByTemplateId(int templateId);
    Task UnEnrollStudent(int studentId, int courseStreamId);
    
}