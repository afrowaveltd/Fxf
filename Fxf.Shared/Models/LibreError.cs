namespace Fxf.Shared.Models;

/// <summary>
/// Represents an error with a descriptive message.
/// </summary>
/// <remarks>
/// This class is typically used to encapsulate error information in APIs or applications. The <see
/// cref="Error"/> property contains the error message, which can be displayed to users or logged
/// for debugging purposes.
/// </remarks>
public class LibreError
{
	/// <summary>
	/// Gets or sets the error message associated with the current operation.
	/// </summary>
	public string Error { get; set; } = string.Empty;
}