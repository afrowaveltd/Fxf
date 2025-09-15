using System.ComponentModel.DataAnnotations;

namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents the results of various checks performed during a worker cycle, including translation and settings checks.
/// </summary>
public class CycleChecks
{
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
	/// Gets or sets a value indicating whether ignored languages were found.
	/// </summary>
	public bool IgnoredLanguagesFound { get; set; } = true;

	/// <summary>
	/// Gets or sets the count of Libre languages found.
	/// </summary>
	public int LibreLanguagesCount { get; set; } = 0;
}