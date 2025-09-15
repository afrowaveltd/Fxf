namespace Fxf.Shared.Models;

/// <summary>
/// Represents an error response containing details about an error that occurred during an operation.
/// </summary>
/// <remarks>
/// This class is typically used to convey error information, such as a descriptive message, in
/// response to a failed operation or API call.
/// </remarks>
public class ErrorResponse(string? message)
{
	/// <summary>
	/// Gets or sets the error message providing details about the error that occurred.
	/// </summary>

	public string? Error { get; set; } = message;
}