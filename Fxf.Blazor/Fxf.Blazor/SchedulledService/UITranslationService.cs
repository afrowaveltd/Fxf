using Fxf.Blazor.Data;
using Fxf.Blazor.Models.Settings;
using Fxf.Blazor.Services;
using Fxf.Blazor.Services.LibreTranslate;

namespace Fxf.Blazor.SchedulledService;

public class UITranslationService(IConfiguration configuration,
	ILibreTranslateService libreTranslateService,
	ILanguageService languageService,
	ApplicationDbContext context) : IUITranslationService
{
	private readonly IConfiguration _configuration = configuration;
	private readonly ILibreTranslateService _libreTranslateService = libreTranslateService;
	private readonly ILanguageService _languageService = languageService;
	private readonly ApplicationDbContext _context = context;
	private string DefaultLanguage => _configuration.GetSection("Localization").Get<Localization>()?.DefaultLanguage ?? "en";
	private List<string> IgnoredLanguages => _configuration.GetSection("Localization").Get<Localization>()?.IgnoredLanguages ?? new List<string> { "en" };

	public async Task RunAsync()
	{
		/*
		 * 1. Check LibreTranslate servers
		 * 2. Check default translation
		 * 3. Add languages to all files - create missing files
		 * 4. Store files (here we include all languages
		 *
		 * -- Backend then Frontend do the same steps
		 *
		 * 5. Load Old Languages (to re-translate changed phrases)
		 * 6. Find records that need updates, deletes, add
		 * 7. Create translations queues
		 * 8. Run Translations - translation with buffering and address * buffers
		 * 9. On completed file returned store the file
		 * 10. Store old language
		 *
		 * -- Return results
		 * store to the DB's changed time
		 */
	}
}