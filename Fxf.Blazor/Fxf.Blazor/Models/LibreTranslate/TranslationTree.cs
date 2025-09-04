namespace Fxf.Blazor.Models.LibreTranslate;

/// <summary>
/// Represents a hierarchical structure for managing translations across multiple languages and keys.
/// </summary>
/// <remarks>The <see cref="TranslationTree"/> class organizes translations in a nested dictionary structure,
/// where the outer dictionary maps language codes (e.g., "en", "fr") to inner dictionaries, and the inner dictionaries
/// map translation keys to their corresponding translated values.</remarks>
public class TranslationTree
{
	/// <summary>
	/// Gets or sets a collection of translations organized by language and key.
	/// </summary>
	/// <remarks>Use this property to manage and retrieve translations for multiple languages.  Ensure that both
	/// the language codes and translation keys are consistent across the application.</remarks>
	public Dictionary<string, Dictionary<string, string>> Translations { get; set; } = [];
}