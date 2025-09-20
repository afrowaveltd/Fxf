namespace Fxf.Blazor.Services;

/// <summary>
/// Provides methods for retrieving selectable options such as available languages and themes for
/// use in user interface elements.
/// </summary>
/// <remarks>
/// This service is typically used to populate dropdown lists or selection controls with options
/// relevant to the application's current context, such as supported languages or available themes.
/// The returned options are intended for display and selection by end users.
/// </remarks>
public interface ISelectOptionsService
{
	/// <summary>
	/// Asynchronously retrieves a list of available language options for selection.
	/// </summary>
	/// <param name="actualLanguageCode">
	/// The language code representing the current or default language context. This value is used to
	/// determine which language should be preselected or highlighted in the returned options. Cannot
	/// be null or empty.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a list of <see
	/// cref="SelectOption"/> objects, each representing a selectable language. The list will be
	/// empty if no languages are available.
	/// </returns>
	Task<List<SelectOption>> GetLanguagesAsync(string actualLanguageCode);

	/// <summary>
	/// Retrieves a list of available theme options for selection, highlighting the current theme if specified.
	/// </summary>
	/// <param name="actualThemeName">
	/// The name of the currently active theme. If not null or empty, the corresponding option will
	/// be marked as selected in the returned list.
	/// </param>
	/// <returns>
	/// A list of <see cref="SelectOption"/> objects representing available themes. The list will be
	/// empty if no themes are available.
	/// </returns>
	List<SelectOption> GetThemes(string actualThemeName);
}