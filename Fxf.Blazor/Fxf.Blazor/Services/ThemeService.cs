using Microsoft.IdentityModel.Tokens;
using System.IO;

namespace Fxf.Blazor.Services;

/// <summary>
/// Provides functionality for managing the user's selected theme, including persisting the theme choice across browser
/// sessions and generating the corresponding CSS file path.
/// </summary>
/// <remarks>The theme selection is stored in a browser cookie to ensure the user's preference is retained between
/// sessions. The service also provides the relative path to the CSS file for the current theme, enabling dynamic theme
/// switching in the application.</remarks>
/// <param name="localizer">The string localizer used for localizing theme-related strings.</param>
/// <param name="cookieService">The cookie service used to persist and retrieve the user's theme selection.</param>
public class ThemeService(IStringLocalizer<ThemeService> localizer, ICookieService cookieService) : IThemeService
{
	private readonly IStringLocalizer<ThemeService> _localizer = localizer;
	private readonly ICookieService _cookieService = cookieService;

	/// <summary>
	/// The full file system path to the theme CSS files directory (for file operations).
	/// </summary>
	private readonly string themesRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
[..AppDomain.CurrentDomain.BaseDirectory
		.IndexOf("bin")], "wwwroot", "themes");

	/// <summary>
	/// Gets or sets the name of the current theme selected by the user.
	/// </summary>
	/// <remarks>The theme value is persisted using cookies and will be retained across browser sessions. If no
	/// theme has been set, the default value "Afrowave Light" is used.</remarks>
	public string CurrentTheme
	{
		get
		{
			var theme = _cookieService.GetCookie("theme");
			_cookieService.SetCookie("theme", theme ?? "Afrowave Ligth", 365);
			return string.IsNullOrEmpty(theme) ? "Afrowave Light" : theme;
		}
		set
		{
			_cookieService.SetCookie("theme", value, 365);
		}
	}

	/// <summary>
	/// Gets the relative URL path to the CSS file for the current theme.
	/// </summary>
	public string CurrentThemeLink => $"/themes/{NameToLink(CurrentTheme)}";

	/// <summary>
	/// Returns the relative URL to the theme file corresponding to the specified theme name.
	/// </summary>
	/// <param name="themeName">The name of the theme for which to retrieve the link. If null or empty, the default theme "Afrowave Light" is used.</param>
	/// <returns>A relative URL string to the theme file if it exists; otherwise, the link to the default theme is returned.</returns>
	/// <exception cref="Exception">Thrown if the default theme "Afrowave Light" is requested but its file does not exist.</exception>
	public string GetThemeLink(string themeName)
	{
		if(string.IsNullOrEmpty(themeName))
		{
			return GetThemeLink("Afrowave Light");
		}
		string link = NameToLink(themeName);
		if(File.Exists(Path.Combine(themesRootPath, link)))
		{
			return $"/themes/{link}";
		}
		if(themeName == "Afrowave Light")
		{
			throw new Exception()
			{
			};
		}
		return GetThemeLink("Afrowave Light");
	}

	private static string NameToLink(string name) => (name.Replace(" ", "_")) + "__theme.css";

	private static string LinkToName(string link) => (link.Replace("/themes/", "").Replace("__theme.css", "").Replace("_", " "));

	/// <summary>
	/// Asynchronously retrieves a list of available themes from the themes directory.
	/// </summary>
	/// <remarks>This method scans the themes directory to identify available themes. The returned list may be empty
	/// if the directory does not contain any valid theme files. This method does not throw an exception if the directory
	/// is missing or empty.</remarks>
	/// <returns>A list of <see cref="ThemeModel"/> objects representing the available themes. The list will be empty if no themes
	/// are found.</returns>
	public List<ThemeModel> GetAvailableThemes()
	{
		List<ThemeModel> result = [];
		foreach(var file in Directory.EnumerateFiles(themesRootPath))
		{
			ThemeModel model = new()
			{
				Name = LinkToName(Path.GetFileName(file)),
				Url = NameToLink(LinkToName(Path.GetFileName(file)))
			};
			result.Add(model);
		}
		return result;
	}
}