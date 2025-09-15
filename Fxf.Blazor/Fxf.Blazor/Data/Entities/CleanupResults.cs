using System.Data.SqlTypes;

namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents the results of a cleanup operation, including timing, status, and statistics.
/// </summary>
public class CleanupResults
{
	/// <summary>
	/// Gets or sets the UTC start time of the cleanup operation.
	/// </summary>
	public DateTime StartTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC end time of the cleanup operation.
	/// </summary>
	public DateTime EndTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets a value indicating whether the old client language was successfully stored.
	/// </summary>
	public bool ClientOldLanguageStored { get; set; } = true;

	/// <summary>
	/// Gets or sets a value indicating whether the old server language was successfully stored.
	/// </summary>
	public bool ServerOldLanguageStored { get; set; } = true;

	/// <summary>
	/// Gets or sets the number of old translation results deleted during the cleanup.
	/// </summary>
	public int OldTranslationResultsDeleted { get; set; } = 0;
}