namespace Fxf.Blazor.Models.LibreTranslate;

/// <summary>
/// Represents a language supported by the LibreTranslate service, including its code, name, and target languages for
/// translation.
/// </summary>
/// <remarks>This class provides information about a specific language, including its ISO code, display name,
/// and the list of languages to which it can be translated. It is typically used to configure or query  supported
/// translation options in the LibreTranslate API.</remarks>
public class LibreLanguage
{
	/// <summary>
	/// Gets or sets the code associated with this instance.
	/// </summary>
	public string Code { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the name associated with this instance.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the list of target identifiers.
	/// </summary>
	public List<string> Targets { get; set; } = [];
}