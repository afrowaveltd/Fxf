using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Models.Worker;

/// <summary>
/// Represents the result of a translation operation, including the translated value, language,
/// status, and any associated error information.
/// </summary>
public class TranslationOutput
{
	/// <summary>
	/// Gets or sets the error message associated with the current operation or state.
	/// </summary>
	public string? ErrorMessage { get; set; } = null;

	/// <summary>
	/// Gets or sets the type of change applied to the phrase.
	/// </summary>
	public PhraseChange Change { get; set; } = PhraseChange.Add;

	/// <summary>
	/// Gets or sets the unique identifier associated with the object.
	/// </summary>
	public string Key { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the language code used for localization or content display.
	/// </summary>
	public string Language { get; set; } = "en";

	/// <summary>
	/// Gets or sets a value indicating whether the operation completed successfully.
	/// </summary>
	public bool Successful { get; set; } = true;

	public TranslationTarget Target { get; set; } = TranslationTarget.Frontend;

	/// <summary>
	/// Gets or sets the string value associated with this instance.
	/// </summary>
	public string Value { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the target area for the translation operation (e.g., frontend or backend).
	/// </summary>
}