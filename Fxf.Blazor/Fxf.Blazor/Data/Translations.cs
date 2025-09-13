using System.ComponentModel.DataAnnotations;

namespace Fxf.Blazor.Data;

/// <summary>
/// Represents translation details for a specific set of resources, including timing and translation requests/results.
/// </summary>
public class Translations
{
	/// <summary>
	/// Gets or sets the UTC start time for the translation process.
	/// </summary>
	[Key]
	public DateTime StartTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC end time for the translation process.
	/// </summary>
	public DateTime EndTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets a value indicating whether an old translation file was found.
	/// </summary>
	public bool OldFileFound { get; set; } = false;

	/// <summary>
	/// Gets or sets the number of translations needed.
	/// </summary>
	public int TranslationsNeeded { get; set; } = 0;

	/// <summary>
	/// Gets or sets the server translation requests, keyed by language or resource.
	/// </summary>
	public List<TranslationRequests> ServerRequestedTranslations { get; set; } = [];

	/// <summary>
	/// Gets or sets the server translation results, keyed by language or resource.
	/// </summary>
	public List<TranslationResults> ServerResultOfTranslating { get; set; } = [];
}