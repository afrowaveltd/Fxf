namespace Fxf.Shared.Models;

/// <summary>
/// Represents a language detected by a language identification process, along with its confidence score.
/// </summary>
/// <remarks>This class is typically used to store the results of language detection algorithms, where the
/// detected language and its associated confidence level are provided.</remarks>
public class Detections
{
	/// <summary>
	/// Gets or sets the confidence level of the operation or result.
	/// </summary>
	public int Confidence { get; set; } = 0;

	/// <summary>
	/// Gets or sets the language code representing the user's preferred language.
	/// </summary>
	public string Language { get; set; } = string.Empty;
}