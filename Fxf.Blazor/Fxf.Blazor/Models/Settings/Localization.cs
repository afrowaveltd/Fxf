namespace Fxf.Blazor.Models.Settings;

/// <summary>
/// Represents localization settings for the application.
/// </summary>
public class Localization
{
	/// <summary>
	/// Gets or sets the default language code (ISO 2-letter) used by the application.
	/// </summary>
	public string DefaultLanguage { get; set; } = "en";

	/// <summary>
	/// Gets or sets the list of language codes to ignore during localization operations.
	/// </summary>
	public List<string> IgnoredLanguages { get; set; } = [];

	/// <summary>
	/// Gets or sets the number of minutes between localization update cycles.
	/// </summary>
	public int MinutesBetweenCycles { get; set; } = 60;

	/// <summary>
	/// Gets or sets the number of days to retain log files before they are eligible for deletion.
	/// </summary>
	/// <remarks>Set this property to control how long old log files are kept. Log files older than the specified
	/// number of days may be deleted during maintenance or cleanup operations. Adjust this value based on your
	/// application's retention and compliance requirements.</remarks>
	public int OldLogsDeleteAfterDays { get; set; } = 30;
}