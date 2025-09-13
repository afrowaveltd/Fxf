using System.ComponentModel.DataAnnotations;

namespace Fxf.Blazor.Data;

/// <summary>
/// Represents the results of various checks performed during a worker cycle, including translation and settings checks.
/// </summary>
public class CycleChecks
{
	/// <summary>
	/// Gets or sets the unique identifier for the cycle checks entry.
	/// </summary>
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether settings were loaded successfully.
	/// </summary>
	public bool SettingsLoaded { get; set; } = false;

	/// <summary>
	/// Gets or sets the UTC start time of the cycle check.
	/// </summary>
	public DateTime StartTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC end time of the cycle check.
	/// </summary>
	public DateTime EndTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets a value indicating whether the default translation was found.
	/// </summary>
	public bool DefaultTranslationFound { get; set; } = true;

	/// <summary>
	/// Gets or sets a value indicating whether ignored languages were found.
	/// </summary>
	public bool IgnoredLanguagesFound { get; set; } = true;

	/// <summary>
	/// Gets or sets the count of Libre languages found.
	/// </summary>
	public int LibreLanguagesCount { get; set; } = 0;

	/// <summary>
	/// Gets or sets the unique identifier for the frontend translations resource.
	/// </summary>
	public int FrontendTranslationsId { get; set; }

	/// <summary>
	/// Gets or sets the unique identifier for the backend translations entry.
	/// </summary>
	public int BackendTranslationsId { get; set; }

	/// <summary>
	/// Gets or sets the frontend translations check results.
	/// </summary>
	public Translations FrontendTranslations { get; set; } = new();

	/// <summary>
	/// Gets or sets the backend translations check results.
	/// </summary>
	public Translations BackendTranslations { get; set; } = new();

	/// <summary>
	/// Gets or sets the results produced by the worker operation.
	/// </summary>
	public WorkerResults WorkerResults { get; set; } = null!;
}