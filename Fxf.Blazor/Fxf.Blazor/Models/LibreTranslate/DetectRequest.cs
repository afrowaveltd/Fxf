namespace Fxf.Blazor.Models.LibreTranslate;

/// <summary>
/// Represents a request model for detection operations.
/// </summary>
/// <remarks>This model is typically used to encapsulate the parameters required for a detection API
/// call.</remarks>
public class DetectRequestModel
{
	/// <summary>
	/// Gets or sets the value of the query string.
	/// </summary>
	public string Q { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the API key for authentication purposes.
	/// </summary>
	public string? Api_key { get; set; }
}