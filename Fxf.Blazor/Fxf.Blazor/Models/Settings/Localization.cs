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
}