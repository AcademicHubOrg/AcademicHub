namespace CustomExceptions;

public class NotFoundException : Exception
{
	public NotFoundException(string resourceName) : base($"The resource '{resourceName}' was not found.")
	{
		Data["HttpStatusCode"] = 404; // Set the HTTP status code
	}
}

