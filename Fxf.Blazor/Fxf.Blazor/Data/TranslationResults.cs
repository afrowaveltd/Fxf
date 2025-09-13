using System.ComponentModel.DataAnnotations;

namespace Fxf.Blazor.Data;

/// <summary>
/// Represents the results of translation operations, including successful translations and errors.
/// </summary>
public class TranslationResults
{
	/// <summary>
	/// Gets or sets the unique identifier for the entity.
	/// </summary>
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the language code for which the translation results are tracked.
	/// </summary>
	public string LanguageCode { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the number of successful translations.
	/// </summary>
	public int SuccessfulTranslations { get; set; }

	/// <summary>
	/// Gets or sets a dictionary of translation errors, keyed by language or resource.
	/// </summary>
	public List<string> TranslationErrors { get; set; } = [];
}