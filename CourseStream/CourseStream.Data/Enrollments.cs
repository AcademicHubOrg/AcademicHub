namespace CourseStream.Data;

public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseStreamId { get; set; }
    public DateTime EnrollmentTimestamp { get; set; }
    // Navigation Properties
    public CourseStream CourseStream { get; set; }
}