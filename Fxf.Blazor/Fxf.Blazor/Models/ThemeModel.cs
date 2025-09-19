namespace Fxf.Blazor.Models;

/// <summary>
/// Represents a visual theme with a name and an associated resource URL.
/// </summary>
/// <remarks>Use the Theme class to encapsulate information about a UI theme, such as its display name and the
/// location of its resources. This class is typically used to manage or select themes in applications that support
/// multiple visual styles.</remarks>
public class ThemeModel
{
	/// <summary>
	/// Gets or sets the name associated with the object.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the URL associated with the resource.
	/// </summary>
	public string Url { get; set; } = string.Empty;
}