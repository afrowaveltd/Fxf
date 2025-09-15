namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents statistics and errors related to language translations during a worker process.
/// </summary>
public class LanguagesTranslations
{
	/// <summary>
	/// Gets or sets the UTC end time for the translation process.
	/// </summary>
	public DateTime EndTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the collection of translation errors encountered during processing.
	/// </summary>
	public List<TranslationError> FailedTranslations { get; set; } = [];

	/// <summary>
	/// Gets or sets the UTC start time for the translation process.
	/// </summary>
	public DateTime StartTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the number of translations completed.
	/// </summary>
	public int TranslationsDone { get; set; } = 0;

	/// <summary>
	/// Gets or sets the number of translations needed.
	/// </summary>
	public int TranslationsNeeded { get; set; } = 0;
}