using System.ComponentModel.DataAnnotations;

namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents an error that occurred during a translation operation, including details about the error, the target
/// language, and the original text.
/// </summary>
/// <remarks>Use this class to capture and convey information about translation failures, such as logging errors
/// or displaying error details to users. Each instance contains the time the error occurred, the language targeted for
/// translation, the error message, and the original text that caused the error.</remarks>
public class TranslationError
{
	/// <summary>
	/// Gets or sets the date and time value associated with this instance.
	/// </summary>
	public DateTime Time { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the target language code for translation operations.
	/// </summary>
	public string TargetLanguage { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the error message associated with the current operation.
	/// </summary>
	public string ErrorMessage { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the original, unmodified text associated with this instance.
	/// </summary>
	public string OriginalText { get; set; } = string.Empty;
}