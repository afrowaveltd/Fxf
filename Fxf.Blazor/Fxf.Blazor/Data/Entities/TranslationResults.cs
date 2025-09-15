using System.ComponentModel.DataAnnotations;

namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents the results of translation operations, including successful translations and errors.
/// </summary>
public class TranslationResults
{
	/// <summary>
	/// Gets or sets the language code for which the translation results are tracked.
	/// </summary>
	public string LanguageCode { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the mark of success
	/// </summary>
	public bool Successful { get; set; }

	/// <summary>
	/// Phrase that was translated
	/// </summary>
	public string Phrase { get; set; } = string.Empty;
}