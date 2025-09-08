using Fxf.Blazor.Services;
using Fxf.Blazor.Services.LibreTranslate;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;

namespace Fxf.Blazor.Hubs;

public class LocalizationHub(ILanguageService languageService,
	IStringLocalizer<LocalizationHub> t,
	ILibreTranslateService libre) : Hub<ILocalizationClient>
{
	/* Hub for real-time localization and language updates. Implements
	 * all functions like the LocalizationController.
	 * - GetLocales(bool is_client) returns Response<TranslationTree>
	 * - GetLanguages() returns list of all supported languages
	 * - GetLanguageInfo(string code) returns Language information
	 * - GetDictionaryByCode(string code) returns Response<Dictionary<string, string>>
	 * - Localize(string code) void
	 * - Translate(string query, string from, string to) returns Response<TranslateResult>
	 * - SaveLocaleAsync(Dictionary<string, string> data,string code, bool isClient) returns Response<bool>
	 * - SaveBulkAsync(TranslationTree tree) returns Response<Dictionary<string, bool>>
	 * - GetOldAsync(bool isClient = true) returns Response<Dictionary<string, string>>
	 * - SaveOldAsync(Dictionary<string, string> data, bool isClient = true) returns Response<bool>
	 */

	private readonly IStringLocalizer<LocalizationHub> _t = t;
	private readonly ILanguageService _languageService = languageService;
	private readonly ILibreTranslateService _libreService = libre;
}