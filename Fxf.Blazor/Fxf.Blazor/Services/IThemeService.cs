namespace Fxf.Blazor.Services;

/// <summary>
/// Provides functionality for managing application themes, including retrieving available themes and accessing or
/// modifying the current theme.
/// </summary>
/// <remarks>Implementations of this interface allow applications to support theme switching and customization.
/// The service exposes the current theme name and its associated resource link, as well as a list of all available
/// themes. Thread safety and persistence of theme changes depend on the specific implementation.</remarks>
public interface IThemeService
{
	/// <summary>
	/// Gets or sets the name of the currently active theme.
	/// </summary>
	/// <remarks>The value should correspond to a valid theme recognized by the application. Changing this property
	/// updates the application's appearance to match the specified theme.</remarks>
	string CurrentTheme { get; set; }

	/// <summary>
	/// Gets the URL or path to the currently active theme resource.
	/// </summary>
	string CurrentThemeLink { get; }

	/// <summary>
	/// Retrieves a list of all available themes that can be applied.
	/// </summary>
	/// <returns>A list of <see cref="ThemeModel"/> objects representing the available themes. The list will be empty if no themes
	/// are available.</returns>
	List<ThemeModel> GetAvailableThemes();

	/// <summary>
	/// Retrieves the URL of the stylesheet associated with the specified theme name.
	/// </summary>
	/// <param name="themeName">The name of the theme for which to obtain the stylesheet link. Cannot be null or empty.</param>
	/// <returns>A string containing the URL of the theme's stylesheet. Returns null if the specified theme does not exist.</returns>
	string GetThemeLink(string themeName);
}