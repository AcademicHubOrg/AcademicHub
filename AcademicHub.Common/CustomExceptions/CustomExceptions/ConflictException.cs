namespace CourseTemplate.Core.CustomExceptions; 
public class ConflictException : Exception
{
	public ConflictException() : base("A conflict occurred.")
	{
	}

	public ConflictException(string message) : base(message)
	{
	}
	public ConflictException(string message, Exception innerException) : base(message, innerException)
	{
	}
	public static ConflictException CreateNameConflictException(string resourceName)
	{
		var conflictException = new ConflictException($"The resource '{resourceName}' already exists.");
		conflictException.Data["HttpStatusCode"] = 409; // Set the HTTP status code
		return conflictException;
	}
}

