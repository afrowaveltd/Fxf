using Microsoft.IdentityModel.Tokens;

namespace Fxf.Blazor.Hubs;

/// <summary>
/// SignalR hub for real-time localization and language updates. Provides methods for clients to
/// request and update locale data, language metadata, dictionaries, and translations.
/// </summary>
public class LocalizationHub(
	 ILanguageService languageService,
	 IStringLocalizer<LocalizationHub> t,
	 ILibreTranslateService libre,
	 IHttpContextAccessor accessor,
	 ICookieService cookieService) : Hub<ILocalizationInterface>

{
	private readonly IStringLocalizer<LocalizationHub> _t = t;
	private readonly ILanguageService _languageService = languageService;
	private readonly ILibreTranslateService _libreService = libre;
	private readonly ICookieService _cookieService = cookieService;
	private readonly IHttpContextAccessor _accessor = accessor;

	/// <summary>
	/// Handles a request from the client to retrieve all available locale dictionaries.
	/// </summary>
	/// <param name="isClient">True to retrieve client (WebAssembly) locales; false for server locales.</param>
	public async Task RequestLocales(bool isClient = true)
	{
		var request = await _languageService.GetAllDictionariesAsync(isClient);
		await Clients.Caller.OnLocalesRequested(request, isClient);
	}

	/// <summary>
	/// Handles a request from the client to retrieve the list of all supported languages.
	/// </summary>
	public async Task RequestLanguages()
	{
		var languages = _languageService.GetAllLanguages();
		await Clients.Caller.OnLanguagesRequested(languages);
	}

	/// <summary>
	/// Handles a request from the client to retrieve metadata for a specific language code.
	/// </summary>
	/// <param name="code">The ISO language code to query.</param>
	public async Task RequestLanguageInfo(string code)
	{
		if(code.IsNullOrEmpty())
		{
			await Clients.Caller.OnError(_t["Code cant be null"], _t["Data error"]);
			return;
		}
		var response = _languageService.GetLanguageByCode(code);
		if(response is null)
		{
			await Clients.Caller.OnError(_t["Error response from the server"], _t["Data error"]);
			return;
		}
		if(response.Success != true)
		{
			await Clients.Caller.OnError(response.Message ?? _t["Error response from the server"], _t["Data error"]);
			return;
		}
		if(response.Data is null)
		{
			// Language not found
			await Clients.Caller.OnError(_t["Server responded with empty data"], _t["Data error"]);
			return;
		}
		await Clients.Caller.OnLanguageInfoRequested(response.Data);
	}

	/// <summary>
	/// Handles a request from the client to retrieve a translation dictionary for a specific
	/// language code and context.
	/// </summary>
	/// <param name="code">The ISO language code.</param>
	/// <param name="isClient">True to retrieve client (WebAssembly) dictionary; false for server.</param>
	public async Task RequestDictionaryByCode(string code, bool isClient = true)
	{
		var dictionary = await _languageService.GetDictionaryAsync(code, isClient);
		await Clients.Caller.OnDictionaryByCodeRequested(code, dictionary, isClient);
	}

	/// <summary>
	/// Handles a request from the client to localize a query string using the current UI culture.
	/// </summary>
	/// <param name="query">The text to localize.</param>
	public async Task RequestLocalization(string query)
	{
		if(string.IsNullOrWhiteSpace(query))
		{
			var errorResponse = new Response<TranslateResult>
			{
				Success = false,
				Message = _t["The query cannot be null or empty."]
			};
			await Clients.Caller.OnLocalizationRequested(query, errorResponse);
			return;
		}
		var target = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
		var result = await _libreService.TranslateTextAsync(query, "en", target);
		await Clients.Caller.OnLocalizationRequested(query, result);
	}

	/// <summary>
	/// Handles a request from the client to translate a query string from one language to another.
	/// </summary>
	/// <param name="query">The text to translate.</param>
	/// <param name="from">The source language ISO code.</param>
	/// <param name="to">The target language ISO code.</param>
	public async Task Translate(string query, string from, string to)
	{
		if(query == null)
		{
			await Clients.Caller.OnError(_t["Empty text can't be translated"], _t["Data error"]);
			return;
		}
		if(from.IsNullOrEmpty())
		{
			from = "en";
		}
		if(to.IsNullOrEmpty())
		{
			to = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
		}
		if(from == to)
		{
			var res = new TranslateResult()
			{
				TranslatedText = query
			};
			await Clients.Caller.OnTranslationRequested(query, from, to, Response<TranslateResult>.Successful(res));
			return;
		}
		var result = await _libreService.TranslateTextAsync(query, from, to);
		await Clients.Caller.OnTranslationRequested(query, from, to, result);
	}

	/// <summary>
	/// Handles a request from the client to save a locale dictionary for a specific language and context.
	/// </summary>
	/// <param name="data">The dictionary of translations to save.</param>
	/// <param name="code">The ISO language code.</param>
	/// <param name="isClient">True to save to client (WebAssembly) locales; false to server locales.</param>
	public async Task SaveLocaleAsync(Dictionary<string, string> data, string code, bool isClient)
	{
		if(data == null || data.Count == 0)
		{
			await Clients.Caller.OnError(_t["No data to save"], _t["Data error"]);
			return;
		}
		if(code.IsNullOrEmpty())
		{
			await Clients.Caller.OnError(_t["Code cant be null"], _t["Data error"]);
			return;
		}
		var response = await _languageService.SaveDictionaryAsync(code, data, isClient);
		if(response == null)
		{
			await Clients.Caller.OnError(_t["Error response from the server"], _t["Data error"]);
			return;
		}
		if(response.Success != true)
		{
			await Clients.Caller.OnError(response.Message ?? _t["Error response from the server"], _t["Data error"]);
			return;
		}
		await Clients.Caller.OnLocaleSaveRequested(code, true, isClient);
	}

	/// <summary>
	/// Handles a request from the client to save all locale dictionaries in bulk.
	/// </summary>
	/// <param name="tree">The translation tree containing all language dictionaries to save.</param>
	public async Task SaveBulkAsync(TranslationTree tree)
	{
		if(tree == null || tree.Translations == null || tree.Translations.Count == 0)
		{
			await Clients.Caller.OnError(_t["No data to save"], _t["Data error"]);
			return;
		}
		var response = await _languageService.SaveTranslationsAsync(tree);
		if(response == null)
		{
			await Clients.Caller.OnError(_t["Error response from the server"], _t["Data error"]);
			return;
		}
		if(response.Success != true)
		{
			await Clients.Caller.OnError(response.Message ?? _t["Error response from the server"], _t["Data error"]);
			return;
		}
		await Clients.Caller.OnBulkSaveRequested(response.Data ?? new Dictionary<string, bool>());
	}

	/// <summary>
	/// Handles a request from the client to retrieve the last stored translation dictionary for the
	/// given context.
	/// </summary>
	/// <param name="isClient">True to retrieve client (WebAssembly) data; false for server data.</param>
	public async Task GetOldAsync(bool isClient = true)
	{
		var response = await _languageService.GetLastStored(isClient);
		await Clients.Caller.OnOldLocaleRequested(response.Data ?? new(), isClient);
	}

	/// <summary>
	/// Handles a request from the client to save the last known translation dictionary for the given context.
	/// </summary>
	/// <param name="data">The dictionary of translations to save.</param>
	/// <param name="isClient">
	/// True to save to client (WebAssembly) temp location; false for server temp location.
	/// </param>
	public async Task SaveOldAsync(Dictionary<string, string> data, bool isClient = true)
	{
		var response = await _languageService.SaveOldTranslationAsync(data, isClient);
		if(response == null)
		{
			await Clients.Caller.OnError(_t["The dictionary is empty"], _t["Data error"]);
			return;
		}
		if(response.Success != true)
		{
			await Clients.Caller.OnError(response.Message ?? _t["Error saving old translations"], _t["Data error"]);
			return;
		}
		await Clients.Caller.OnOldLocaleSaveRequested(response.Success, isClient);
	}

	/// <summary>
	/// Detects the locale of the current HTTP request and notifies the caller with the detected locale.
	/// </summary>
	/// <remarks>
	/// This method retrieves the culture information from the current HTTP request and defaults to
	/// "en" if no culture is detected. The detected locale is then sent to the caller using the interface.
	/// </remarks>
	/// <returns></returns>
	public async Task DetectLocales()
	{
#pragma warning disable CS8604 // Může jít o argument s odkazem null.
		string detected = GetCultureFromRequest(_accessor.HttpContext) ?? "en";
#pragma warning restore CS8604 // Může jít o argument s odkazem null.
		await Clients.Caller.OnDetectLocales(new UiLocale() { Locale = detected });
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
}