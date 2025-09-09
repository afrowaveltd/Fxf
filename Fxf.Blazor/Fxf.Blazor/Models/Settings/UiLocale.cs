namespace Fxf.Blazor.Models.Settings;

/// <summary>
/// Represents the user interface locale setting for the application.
/// </summary>
/// <remarks>This class provides a property to specify the locale used for user interface elements,  such as
/// language and regional formatting. The default value is "en" (English).</remarks>
public class UiLocale
{
	/// <summary>
	/// Gets or sets the locale used for user interface elements.
	/// </summary>
	/// <remarks>This property determines the language and regional settings for user interface elements. Set this
	/// property to a valid locale string, such as "en-US" for English (United States) or "fr-FR" for French
	/// (France).</remarks>
	public string Locale { get; set; } = "en";
}