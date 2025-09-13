using System.ComponentModel.DataAnnotations;
using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Data;

/// <summary>
/// Represents the results and status information of a background worker process, including timing, success state, errors, and translation details.
/// </summary>
public class WorkerResults
{
	/// <summary>
	/// Gets or sets the unique identifier for the worker results entry.
	/// </summary>
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the UTC start time of the worker process.
	/// </summary>
	public DateTime StartTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC end time of the worker process.
	/// </summary>
	public DateTime EndTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets a value indicating whether the worker process completed successfully.
	/// </summary>
	public bool Successful { get; set; } = true;

	/// <summary>
	/// Gets or sets the last status of the worker process.
	/// </summary>
	public WorkerStatus LastStatus { get; set; } = WorkerStatus.Iddle;

	/// <summary>
	/// Gets or sets the unique identifier for the cycle checks record.
	/// </summary>
	public int CycleChecksId { get; set; }

	/// <summary>
	/// Gets or sets the unique identifier for the language translation entry.
	/// </summary>
	public int LanguagesTranslationsId { get; set; }

	/// <summary>
	/// Gets or sets the unique identifier for the translation request.
	/// </summary>
	public int TranslationRequestsId { get; set; }

	/// <summary>
	/// Gets or sets the unique identifier for the translation results.
	/// </summary>
	public int TranslationResultsId { get; set; }

	/// <summary>
	/// Gets or sets the unique identifier for the worker error entry.
	/// </summary>

	public int WorkerErrorId { get; set; }

	/// <summary>
	/// Gets or sets the cycle checks performed during the worker process.
	/// </summary>
	public CycleChecks CycleChecks { get; set; } = null!;

	/// <summary>
	/// Gets or sets the language translation statistics for the worker process.
	/// </summary>
	public LanguagesTranslations LanguagesTranslations { get; set; } = null!;

	/// <summary>
	/// Gets or sets the translation requests made during the worker process.
	/// </summary>
	public TranslationRequests TranslationRequests { get; set; } = null!;

	/// <summary>
	/// Gets or sets the translation results produced by the worker process.
	/// </summary>
	public TranslationResults TranslationResults { get; set; } = null!;

	/// <summary>
	/// Gets or sets a list of error messages, keyed by worker status.
	/// </summary>
	public List<WorkerError> ErrorMessages { get; set; } = [];
}