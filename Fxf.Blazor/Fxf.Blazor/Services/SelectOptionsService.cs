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
/// <param name="translateService">The transate service used to get list of supported languages</param>
public class SelectOptionsService(IStringLocalizer<SelectOptionsService> localizer,
	IThemeService themeService,
	ILanguageService languageService,
	ILibreTranslateService translateService) : ISelectOptionsService
{
	private static readonly string defaultTheme = "Afrowave Light";
	private readonly ILanguageService _languageService = languageService;
	private readonly IStringLocalizer<SelectOptionsService> _localizer = localizer;
	private readonly IThemeService _themeService = themeService;
	private readonly ILibreTranslateService _translateService = translateService;

	/// <summary>
	/// Asynchronously retrieves a list of available languages formatted as selection options,
	/// marking the specified language code as selected.
	/// </summary>
	/// <remarks>
	/// The returned list contains one <see cref="SelectOption"/> per available language, with the
	/// <see langword="Selected"/> property set to <see langword="true"/> for the option matching the
	/// specified language code. This method is typically used to populate language selection
	/// controls in user interfaces.
	/// </remarks>
	/// <param name="actualLanguageCode">
	/// The language code to be marked as selected in the returned list. If the value is null, empty,
	/// or not among the available languages, "en" is used as the default.
	/// </param>
	/// <returns>
	/// A list of <see cref="SelectOption"/> objects representing available languages. The list is
	/// empty if no languages are available.
	/// </returns>
	public async Task<List<SelectOption>> GetLanguagesAsync(string actualLanguageCode)
	{
		var serverResponse = await _translateService.GetAvailableLanguagesAsync();
		if(!serverResponse.Success)
		{
			return [];
		}
		List<string> availableLanguageCodes = serverResponse.Data?.ToList() ?? [];
		Response<List<Language>> languages = _languageService.GetSelectedLanguagesInfo(availableLanguageCodes);
		if(!languages.Success || languages.Data == null)
		{
			return [];
		}

		// Ensure the actual language code is valid; if not, default to "en"
		if(string.IsNullOrEmpty(actualLanguageCode) || !availableLanguageCodes.Contains(actualLanguageCode))
		{
			actualLanguageCode = "en";
		}
		List<SelectOption> result = [];
		foreach(var lang in languages.Data)
		{
			result.Add(new SelectOption(actualLanguageCode)
			{
				Value = lang.Code,
				Text = lang.Native,
				Selected = actualLanguageCode == lang.Code
			});
		}
		return result.OrderBy(s => s.Text).ToList();
	}

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