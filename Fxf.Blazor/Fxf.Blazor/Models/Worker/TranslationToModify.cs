using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Models.Worker;

/// <summary>
/// Represents a translation entry to be added, updated, or removed in a localization system.
/// </summary>
/// <remarks>
/// Use this class to specify the details of a translation modification, including the target
/// language, the translation key, the new value, and the type of change to apply. The default
/// language is "en" and the default change type is <see cref="PhraseChange.Add"/>.
/// </remarks>
public class TranslationToModify
{
	/// <summary>
	/// Gets or sets the type of change to apply to the translation entry (add, remove, or update).
	/// </summary>
	public PhraseChange Change { get; set; } = PhraseChange.Add;

	/// <summary>
	/// Gets or sets the key identifying the translation entry.
	/// </summary>
	public string Key { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the language code for the translation (e.g., "en", "fr").
	/// </summary>
	public string Language { get; set; } = "en";

	/// <summary>
	/// Gets or sets a value indicating whether the translation is for the frontend (true) or backend (false).
	/// </summary>
	public TranslationTarget Target { get; set; } = TranslationTarget.Frontend;

	/// <summary>
	/// Gets or sets the value of the translation to be applied.
	/// </summary>
	public string Value { get; set; } = string.Empty;
}