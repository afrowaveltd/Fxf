using Fxf.Blazor.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Fxf.Blazor.Api;
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
}
