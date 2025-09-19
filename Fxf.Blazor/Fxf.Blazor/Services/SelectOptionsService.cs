namespace Fxf.Blazor.Services;

/// <summary>
/// Provides services for generating select list options for themes, including localization support
/// for display names.
/// </summary>
/// <remarks>
/// This service is typically used to populate UI elements such as dropdown lists with available
/// themes, ensuring that display names are localized according to the application's current culture.
/// </remarks>
/// <param name="localizer">
/// The string localizer used to provide localized display names for theme options.
/// </param>
/// <param name="themeService">The theme service used to retrieve available themes.</param>
/// <param name="languageService">The language service used to manage application languages.</param>
public class SelectOptionsService(IStringLocalizer<SelectOptionsService> localizer,
	IThemeService themeService,
	ILanguageService languageService) : ISelectOptionsService
{
	private static readonly string defaultTheme = "Afrowave Light";
	private readonly ILanguageService _languageService = languageService;
	private readonly IStringLocalizer<SelectOptionsService> _localizer = localizer;
	private readonly IThemeService _themeService = themeService;

	/// <summary>
	/// Generates a list of selectable theme items for use in UI elements, marking the current theme
	/// as selected.
	/// </summary>
	/// <remarks>
	/// This method is typically used to populate dropdown lists or other selection controls with
	/// available themes. The returned list is localized and includes all themes provided by the
	/// theme service.
	/// </remarks>
	/// <param name="actualThemeName">
	/// The name of the currently active theme. If null or empty, the default theme is used.
	/// </param>
	/// <returns>
	/// A list of <see cref="SelectListItem"/> objects representing available themes. The item
	/// matching the active theme is marked as selected.
	/// </returns>
	public List<SelectOption> GetThemes(string actualThemeName)
	{
		List<SelectOption> result = [];
		if(string.IsNullOrEmpty(actualThemeName))
		{
			actualThemeName = defaultTheme;
		}
		List<ThemeModel> themes = _themeService.GetAvailableThemes();
		foreach(var theme in themes)
		{
			result.Add(new SelectOption(actualThemeName)
			{
				Value = theme.Name,
				Text = _localizer[theme.Name],
				Selected = actualThemeName == theme.Name
			});
		}
		return [.. result.OrderBy(s => s.Text)];
	}
}