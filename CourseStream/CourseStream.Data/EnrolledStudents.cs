namespace CourseStream.Data;

public class EnrolledStudent
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseStreamId { get; set; }

    // Navigation Properties
    public CourseStream CourseStream { get; set; }
}