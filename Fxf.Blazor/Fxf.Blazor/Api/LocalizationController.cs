using Microsoft.AspNetCore.Mvc;

namespace Fxf.Blazor.Api;

/// <summary>
/// HTTP API for localization: lists languages and locales, reads/saves locale dictionaries, and
/// performs text translation using LibreTranslate.
/// </summary>
/// <remarks>
/// Endpoints exposed by this controller:
/// - Get all locales (client/server)
/// - Get languages and language details
/// - Get/save locale dictionaries (single or bulk)
/// - Translate/localize text
///
/// Dependencies are provided via DI: <see cref="ILanguageService"/> for language metadata and
/// dictionary I/O, <see cref="IStringLocalizer{T}"/> for localized messages, <see
/// cref="IHttpContextAccessor"/> for HTTP context, and <see cref="ILibreTranslateService"/> for
/// translation operations.
/// </remarks>
/// <param name="languageService">Service for language metadata and locale dictionary operations.</param>
/// <param name="t">Localizer for user-facing and error messages.</param>
/// <param name="accessor">Accessor for the current HTTP context.</param>
/// <param name="cookieService">Service allowing to access and read cookies</param>
/// <param name="libre">Service that integrates with LibreTranslate.</param>
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
public class LocalizationController(ILanguageService languageService,
	 IStringLocalizer<LocalizationController> t,
	 IHttpContextAccessor accessor,
	 ICookieService cookieService,
	 ILibreTranslateService libre) : ControllerBase
{
	private readonly ILanguageService _languageService = languageService;
	private readonly IStringLocalizer<LocalizationController> _t = t;
	private readonly ICookieService _cookieService = cookieService;
	private readonly IHttpContextAccessor _accessor = accessor;
	private readonly ILibreTranslateService _libre = libre;

	/// <summary>
	/// Gets all available locale dictionaries for the specified context (client or server).
	/// </summary>
	/// <param name="is_client">
	/// True to return client (WebAssembly) locales; false for server locales. Defaults to true.
	/// </param>
	/// <returns>
	/// 200 OK with a <see cref="TranslationTree"/> when locales are available; otherwise 404 Not Found.
	/// </returns>
	/// <response code="200">Locales were found and returned.</response>
	/// <response code="404">No locale files are available.</response>
	[HttpGet("get_locales/{is_client?}")]
	public async Task<IActionResult> GetLocalesAsync(bool? is_client = true)

	{
		var result = await _languageService.GetAllDictionariesAsync(is_client is not null && is_client == true ? true : false);
		if(result.Success && result.Data is not null)
		{
			return Ok(result.Data ?? new());
		}
		return NotFound(new ErrorResponse(_t["No locale files found."].Value));
	}

	/// <summary>
	/// Gets the list of all supported languages.
	/// </summary>
	/// <returns>
	/// 200 OK with a list of <c>Language</c> items. Returns an empty list if none are configured.
	/// </returns>
	/// <response code="200">Languages were returned (possibly empty).</response>
	[HttpGet("get_languages")]
	public IActionResult GetLanguages()
	{
		var result = _languageService.GetAllLanguages();

		return Ok(result ?? new());
	}

	/// <summary>
	/// Gets language metadata by its code.
	/// </summary>
	/// <param name="code">The ISO language code (e.g., "en", "cs").</param>
	/// <returns>
	/// 200 OK with the language data; 400 Bad Request on invalid input; 404 Not Found if the
	/// language does not exist.
	/// </returns>
	/// <response code="200">Language information was found and returned.</response>
	/// <response code="400">The <paramref name="code"/> parameter is missing or invalid.</response>
	/// <response code="404">No language exists for the specified <paramref name="code"/>.</response>
	[HttpGet("get_language_info/{code?}")]
	public IActionResult GetLanguageInfo(string code)
	{
		if(string.IsNullOrWhiteSpace(code))
		{
			return BadRequest(_t["Language code is required."]);
		}
		var result = _languageService.GetLanguageByCode(code);
		if(result is null)
		{
			return NotFound(new ErrorResponse(_t["Language not found."].Value));
		}

		if(result.Success && result.Data is not null)
		{
			return Ok(result.Data);
		}

		if(result.Warning)
		{
			return Ok(result);
		}

		if(!result.Success)
		{
			return BadRequest(new ErrorResponse(result.Message));
		}
		return NotFound(new ErrorResponse(_t["Language not found."].Value));
	}

	/// <summary>
	/// Gets the translation dictionary for the specified language code.
	/// </summary>
	/// <param name="code">The ISO language code to retrieve (e.g., "en", "cs").</param>
	/// <param name="is_client">
	/// True to read the client (WebAssembly) dictionary; false for the server dictionary. Defaults
	/// to true.
	/// </param>
	/// <returns>
	/// 200 OK with the language dictionary or a warning payload; 400 Bad Request on failure; 404 Not
	/// Found when missing.
	/// </returns>
	/// <response code="200">Dictionary was returned.</response>
	/// <response code="400">The request failed (invalid input or processing error).</response>
	/// <response code="404">No dictionary exists for the specified language.</response>
	[HttpGet("get_language_by_code/{code}/{is_client?}")]
	public async Task<IActionResult> GetLanguageByCodeAsync(string code, bool? is_client = true)
	{
		if(string.IsNullOrWhiteSpace(code))
		{
			return BadRequest(_t["Language code is required."].Value);
		}
		var result = await _languageService.GetDictionaryAsync(code, is_client is not null && is_client == true);
		if(result.Success && result.Data is not null)
		{
			return Ok(result.Data);
		}
		if(result.Warning)
		{
			return Ok(result);
		}
		if(!result.Success)
		{
			return BadRequest(new ErrorResponse(result.Message));
		}

		return NotFound(new ErrorResponse(_t["Language not found."].Value));
	}

	/// <summary>
	/// Localizes a resource key or translates raw text.
	/// </summary>
	/// <remarks>
	/// If both <paramref name="target"/> and <paramref name="source"/> are null, the localized
	/// resource value for <paramref name="query"/> is returned. Otherwise, the text is translated
	/// using LibreTranslate.
	/// Defaults: <paramref name="source"/> = "en"; <paramref name="target"/> = current UI culture.
	/// </remarks>
	/// <param name="query">The text key or raw text to localize/translate.</param>
	/// <param name="target">Target language ISO code. If null, the current UI culture is used.</param>
	/// <param name="source">Source language ISO code. If null, "en" is used.</param>
	/// <returns>
	/// 200 OK with a localized string or <see cref="TranslateResult"/>; 400 Bad Request on invalid
	/// input or translation failure.
	/// </returns>
	/// <response code="200">Localization/translation succeeded.</response>
	/// <response code="400">The input was invalid or translation failed.</response>
	[HttpGet("localize/{query}/{target?}/{source?}")]
	public async Task<IActionResult> LocalizeAsync(string query, string? target = null, string? source = null)
	{
		if(string.IsNullOrWhiteSpace(query))
		{
			return BadRequest(_t["Query text is required."]);
		}
		if(target == null && source == null)
		{
			return Ok(_t[query].Value);
		}
		if(target != null)
		{
			_ = source ?? "en";
		}
		if(target == null)
		{
			_ = target ?? Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
		}

		var translationResult = await _libre.TranslateTextAsync(query, source ?? "en", target ?? Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);

		if(translationResult.Success)
		{
			return Ok(translationResult.Data);
		}
		if(translationResult.Warning)
		{
			return Ok(translationResult.Data);
		}
		if(translationResult.Success == false)
		{
			return BadRequest(new ErrorResponse(translationResult.Message));
		}
		else
		{
			return BadRequest(new ErrorResponse(_t["Unknown error"].Value));
		}
	}

	/// <summary>
	/// Saves a locale dictionary for the specified language code.
	/// </summary>
	/// <remarks>
	/// The request body must be a JSON object of string-to-string key/value pairs. Returns 400 when
	/// the body is empty, invalid JSON, or not a valid dictionary.
	/// </remarks>
	/// <param name="code">The ISO language code for which the data is being saved.</param>
	/// <param name="is_client">
	/// True to save to client (WebAssembly) locales; false to server locales. Defaults to true.
	/// </param>
	/// <returns>
	/// 200 OK when saved successfully (or with warnings); 400 Bad Request on invalid input; 404 Not
	/// Found when the language is missing.
	/// </returns>
	/// <response code="200">Dictionary saved or a warning result is returned.</response>
	/// <response code="400">The request body is empty, not valid JSON, or not a valid dictionary.</response>
	/// <response code="404">The specified language does not exist.</response>
	[HttpPost("save_locale/{code}/{is_client?}")]
	public async Task<IActionResult> SaveLocaleAsync(string code, bool? is_client = true)
	{
		if(string.IsNullOrWhiteSpace(code))
		{
			return BadRequest(_t["Language code is required."].Value);
		}
		using var reader = new StreamReader(Request.Body);
		var body = await reader.ReadToEndAsync();
		if(string.IsNullOrWhiteSpace(body))
		{
			return BadRequest(new ErrorResponse(_t["Request body is empty."].Value));
		}
		Dictionary<string, string>? dictionary = null;
		try
		{
			dictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(body);
			if(dictionary is null || !dictionary.Any())
			{
				return BadRequest(new ErrorResponse(_t["Invalid dictionary data."].Value));
			}
		}
		catch(Exception)
		{
			return BadRequest(new ErrorResponse(_t["Invalid JSON format."].Value));
		}
		if(dictionary is null || !dictionary.Any())
		{
			return BadRequest(new ErrorResponse(_t["Invalid dictionary data."].Value));
		}

		var result = await _languageService.SaveDictionaryAsync(code, dictionary, is_client is not null && is_client == true);
		if(result.Success)
		{
			return Ok(result.Data);
		}
		if(result.Warning)
		{
			return Ok(result);
		}
		if(!result.Success)
		{
			return BadRequest(new ErrorResponse(result.Message));
		}
		return NotFound(new ErrorResponse(_t["Language not found."].Value));
	}

	/// <summary>
	/// Saves a bulk set of translations provided in the request body.
	/// </summary>
	/// <remarks>
	/// The request body must contain a valid JSON representation of a <see cref="TranslationTree"/>.
	/// Returns 400 if the body is empty or invalid JSON.
	/// </remarks>
	/// <param name="is_client">
	/// True to save client (WebAssembly) translations; false for server. Defaults to true.
	/// </param>
	/// <returns>
	/// 200 OK with the per-language result map on success; 400 Bad Request on invalid input or failure.
	/// </returns>
	/// <response code="200">Translations were saved.</response>
	/// <response code="400">The input was empty or invalid JSON, or the operation failed.</response>
	[HttpPost("save_locale_bulk/{is_client?}")]
	public async Task<IActionResult> SaveBulkAsync(bool? is_client = true)
	{
		using var reader = new StreamReader(Request.Body);
		var body = await reader.ReadToEndAsync();
		TranslationTree jsonBody;
		try
		{
			jsonBody = JsonSerializer.Deserialize<TranslationTree>(body) ?? new();
		}
		catch(Exception ex)
		{
			return BadRequest(new ErrorResponse(_t["Error in the JSON file"].Value + " " + ex.Message));
		}
		if(string.IsNullOrWhiteSpace(body))
		{
			return BadRequest(new ErrorResponse(_t["Request body is empty."].Value));
		}

		var result = await _languageService.SaveTranslationsAsync(jsonBody, is_client != null && is_client == true);
		if(result.Success)
		{
			return Ok(result.Data);
		}
		if(result.Warning)
		{
			return Ok(result.Data);
		}
		if(result.Success == false)
		{
			return BadRequest(new ErrorResponse(result.Message));
		}
		return BadRequest(new ErrorResponse(_t["Unknown error"].Value));
	}

	/// <summary>
	/// Gets the last stored locale dictionary for the specified context.
	/// </summary>
	/// <param name="is_client">
	/// True for client (WebAssembly) context; false for server. Defaults to true.
	/// </param>
	/// <returns>200 OK with the last stored dictionary; otherwise 404 Not Found.</returns>
	/// <response code="200">The last stored dictionary was found and returned.</response>
	/// <response code="404">No stored dictionary is available.</response>
	[HttpGet("get_old/{is_client?}")]
	public async Task<IActionResult> GetOldAsync(bool? is_client = true)
	{
		var result = await _languageService.GetLastStored(is_client is not null && is_client == true);
		if(result.Success && result.Data is not null)
		{
			return Ok(result.Data ?? new());
		}
		return NotFound(new ErrorResponse(_t["No old locale file found."].Value));
	}

	/// <summary>
	/// Saves previously exported/temporary translation data provided in the request body.
	/// </summary>
	/// <remarks>
	/// The request body must be a JSON object of string-to-string key/value pairs. Returns 400 for
	/// empty body, invalid JSON, or invalid dictionary content.
	/// </remarks>
	/// <param name="is_client">
	/// True to save for client (WebAssembly) context; false for server. Defaults to true.
	/// </param>
	/// <returns>
	/// 200 OK when saved; 400 Bad Request on invalid input; 404 Not Found when the target language
	/// cannot be resolved.
	/// </returns>
	/// <response code="200">Dictionary saved.</response>
	/// <response code="400">The request body is empty, not valid JSON, or not a valid dictionary.</response>
	[HttpPost("save_old/{is_client?}")]
	public async Task<IActionResult> SaveOldAsync(bool? is_client = true)
	{
		using var reader = new StreamReader(Request.Body);
		var body = await reader.ReadToEndAsync();
		if(string.IsNullOrWhiteSpace(body))
		{
			return BadRequest(_t["Request body is empty."].Value);
		}
		Dictionary<string, string>? dictionary = null;
		try
		{
			dictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(body);
			if(dictionary is null || !dictionary.Any())
			{
				return BadRequest(new ErrorResponse(_t["Invalid dictionary data."].Value));
			}
		}
		catch(Exception)
		{
			return BadRequest(new ErrorResponse(_t["Invalid JSON format."].Value));
		}
		if(dictionary is null || !dictionary.Any())
		{
			return BadRequest(new ErrorResponse(_t["Invalid dictionary data."].Value));
		}
		var result = await _languageService.SaveOldTranslationAsync(dictionary, is_client is not null && is_client == true);
		if(result.Success)
		{
			return Ok(result.Data);
		}
		if(result.Warning)
		{
			return Ok(result);
		}
		if(!result.Success)
		{
			return BadRequest(new ErrorResponse(result.Message));
		}
		return BadRequest(new ErrorResponse(_t["Unknown error."].Value));
	}

	/// <summary>
	/// Retrieves the user's locale based on the current HTTP request.
	/// </summary>
	/// <remarks>
	/// This method extracts the user's locale from the HTTP request context. If the locale cannot be
	/// determined, a default locale of "en" is provided.
	/// </remarks>
	/// <returns>
	/// An <see cref="IActionResult"/> containing a <see cref="UiLocale"/> object with the user's
	/// locale. If no locale is determined, the default locale "en" is returned.
	/// </returns>
	[HttpGet("get_my_locales")]
	public IActionResult GetMyLocales()
	{
#pragma warning disable CS8604 // Může jít o argument s odkazem null.
		string? cult = GetCultureFromRequest(_accessor.HttpContext);
#pragma warning restore CS8604 // Může jít o argument s odkazem null.
		if(cult == null)
		{
			return Ok(new UiLocale() { Locale = "en" });
		}
		return Ok(new UiLocale() { Locale = cult });
	}

	/// <summary>
	/// Retrieves the full list of available Libre languages and their details.
	/// </summary>
	/// <remarks>
	/// This method fetches the list of available languages from an external source and processes it
	/// to return detailed information. If no languages are available or an error occurs during
	/// processing, an appropriate HTTP status code is returned.
	/// </remarks>
	/// <returns>
	/// An <see cref="IActionResult"/> containing the following:
	/// <list type="bullet">
	/// <item>
	/// <description>
	/// <see cref="OkObjectResult"/> with the list of languages if the operation is successful.
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// <see cref="NotFoundResult"/> if no languages are available or the processed data is null.
	/// </description>
	/// </item>
	/// <item>
	/// <description>
	/// <see cref="BadRequestObjectResult"/> with an error message if the operation fails.
	/// </description>
	/// </item>
	/// </list>
	/// </returns>
	[HttpGet("get_libre_languages_full_list")]
	public async Task<IActionResult> GetLibreLanguagesFullListAsync()
	{
		var response = await GetLibreAvailableLanguages();
		if(response.Count == 0)
		{
			return NotFound();
		}

		var languages = _languageService.GetSelectedLanguagesInfo(response);
		if(languages == null)
		{
			return NotFound();
		}
		if(languages.Success)
		{
			return Ok(languages.Data);
		}
		else
		{
			return BadRequest(languages.Message);
		}
	}

	/// <summary>
	/// Retrieves a list of available language codes from the Libre service.
	/// </summary>
	/// <remarks>
	/// This method asynchronously fetches the available language codes supported by the Libre
	/// service and returns them in the response. The response is formatted as an HTTP 200 OK result
	/// containing the list of language codes.
	/// </remarks>
	/// <returns>
	/// An <see cref="IActionResult"/> containing an HTTP 200 OK response with the list of available
	/// language codes, or an appropriate error response if the operation fails.
	/// </returns>

	[HttpGet("get_libre_languages_code_list")]
	public async Task<IActionResult> GetLibreLanguagesCodeListAsync()
	{
		var response = await GetLibreAvailableLanguages();
		return Ok(response);
	}

	private string? GetCultureFromRequest(HttpContext context)
	{
		string? culture = _cookieService.GetCookie("BlazorCulture");
		if(!string.IsNullOrWhiteSpace(culture))
		{
			return culture;
		}
		culture = context.Request.Query["culture"];
		if(!string.IsNullOrWhiteSpace(culture))
		{
			return culture;
		}
		culture = context.Request.Headers.AcceptLanguage.ToString().Split(',').FirstOrDefault();
		if(!string.IsNullOrWhiteSpace(culture))
		{
			return culture;
		}
		return "en";
	}

	private async Task<List<string>> GetLibreAvailableLanguages()
	{
		Response<string[]> languages = await _libre.GetAvailableLanguagesAsync();
		if(languages.Success)
		{
			return languages.Data.ToList() ?? new List<string>();
		}
		Console.WriteLine(languages.Message);
		return [];
	}
}