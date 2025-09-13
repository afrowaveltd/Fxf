using System.ComponentModel.DataAnnotations;

namespace Fxf.Blazor.Data;

/// <summary>
/// Represents statistics and errors related to language translations during a worker process.
/// </summary>
public class LanguagesTranslations
{
	/// <summary>
	/// Gets or sets the unique identifier for the entity.
	/// </summary>
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the number of translations needed.
	/// </summary>
	public int TranslationsNeeded { get; set; } = 0;

	/// <summary>
	/// Gets or sets the number of translations completed.
	/// </summary>
	public int TranslationsDone { get; set; } = 0;

	/// <summary>
	/// Gets or sets the number of translation errors encountered.
	/// </summary>
	public int TranslationErrors { get; set; } = 0;

	/// <summary>
	/// Gets or sets the collection of translation errors encountered during processing.
	/// </summary>
	public List<TranslationError> FailedTranslations { get; set; } = [];

	/// <summary>
	/// Gets or sets the UTC start time for the translation process.
	/// </summary>
	public DateTime StartTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC end time for the translation process.
	/// </summary>
	public DateTime EndTime { get; set; } = DateTime.UtcNow;
}