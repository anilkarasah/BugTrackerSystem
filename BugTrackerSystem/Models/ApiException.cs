namespace BugTrackerAPI.Models;

public class ApiException : Exception
{
	public int StatusCode { get; set; }

	public ApiException(int statusCode = 500, string message = "Internal server error") : base(message)
	{
		StatusCode = statusCode;
	}
}
