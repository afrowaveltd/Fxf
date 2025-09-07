using Fxf.Blazor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Fxf.Blazor.Api;

/// <summary>
/// Provides endpoints for managing localization, including retrieving available locales, supported languages, and
/// translation dictionaries, as well as performing text translations and saving locale files.
/// </summary>
/// <remarks>This controller exposes a set of APIs for localization-related operations, such as retrieving
/// metadata about supported languages, fetching translation dictionaries, and performing text translations. It also
/// allows saving custom locale files for specific languages.  The controller relies on dependency-injected services,
/// including <see cref="ILanguageService"/> for language-related operations, <see cref="IStringLocalizer{T}"/> for
/// localized strings, and <see cref="IHttpContextAccessor"/> for accessing HTTP context information.</remarks>
/// <param name="languageService">The language service</param>
/// <param name="t">Translation service</param>
/// <param name="accessor">Http Context accessor</param>
[Route("api/[controller]")]
[ApiController]
public class LocalizationController(ILanguageService languageService, IStringLocalizer<LocalizationController> t, IHttpContextAccessor accessor) : ControllerBase
{
	private readonly ILanguageService _languageService = languageService;
	private readonly IStringLocalizer<LocalizationController> _t = t;
	private readonly IHttpContextAccessor _accessor = accessor;

	/*
	 * public GET api/localization/get_locales returns all available client locales
	 * public GET api/localization/get_languages returns all supported languages with metadata
	 * public GET api/localization/get_language_by_code/{code}/{isServer = false}  selected language json by code
	 * public GET api/localization/get_all_dictionaries/{isServer = false} returns all available locale files as a TranslationTree
	 * public POST api/localization/localize/{query}/{target}/{source} translates text using LibreTranslate default target is detected client language, source is english if not specified
	 * public POST api/localization/save_locale/{code}/{isServer = false} saves a locale file for the specified language code
	 */

	/// <summary>
	/// Retrieves all available locale files for the specified client or server context.
	/// </summary>
	/// <remarks>This method calls the underlying language service to fetch locale files based on the specified
	/// context.</remarks>
	/// <param name="is_client">A boolean value indicating whether to retrieve client-specific locale files.  If <see langword="true"/>, client
	/// locale files are returned; otherwise, server locale files are retrieved.</param>
	/// <returns>An <see cref="IActionResult"/> containing a collection of locale files if found, or a 404 Not Found response if no
	/// locale files are available.</returns>
	[HttpGet("get_locales/{is_client}")]
	public async Task<IActionResult> GetLocalesAsync(bool? is_client = true)
	{
		var result = await _languageService.GetAllDictionariesAsync(is_client is not null && is_client == true ? true : false);
		if(result.Success && result.Data is not null)
		{
			return Ok(result.Data ?? new());
		}
		return NotFound(_t["No locale files found."]);
	}
}