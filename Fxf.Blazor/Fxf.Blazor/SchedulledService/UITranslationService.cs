namespace Fxf.Blazor.SchedulledService;

/// <summary>
/// Provides services for managing and automating UI translation workflows, including checking translation server
/// availability, updating language resources, and coordinating translation tasks.
/// </summary>
/// <remarks>This service coordinates the end-to-end process of UI translation, including validation of
/// translation servers, updating language files, and managing translation queues. It communicates progress and status
/// changes to clients in real time. Thread safety and completion of all workflow steps are ensured when methods are
/// awaited.</remarks>
/// <param name="configuration">The application configuration used to access localization settings and other relevant options.</param>
/// <param name="libreTranslateService">The service responsible for interacting with LibreTranslate servers to perform translation operations.</param>
/// <param name="languageService">The service used to manage supported languages and language-related operations.</param>
/// <param name="context">The database context for accessing and updating translation-related data.</param>
/// <param name="workerHub">The SignalR hub context used to communicate worker status updates to connected clients.</param>
public class UITranslationService(IConfiguration configuration,
	ILibreTranslateService libreTranslateService,
	ILanguageService languageService,
	ApplicationDbContext context,
	IHubContext<WorkerHub> workerHub) : IUITranslationService
{
	private readonly IConfiguration _configuration = configuration;
	private readonly ApplicationDbContext _context = context;
	private readonly ILanguageService _languageService = languageService;
	private readonly ILibreTranslateService _libreTranslateService = libreTranslateService;
	private readonly IHubContext<WorkerHub> _workerHub = workerHub;
	private string DefaultLanguage => _configuration.GetSection("Localization").Get<Localization>()?.DefaultLanguage ?? "en";
	private List<string> IgnoredLanguages => _configuration.GetSection("Localization").Get<Localization>()?.IgnoredLanguages ?? new List<string> { "en" };

	/// <summary>
	/// Executes the asynchronous workflow for checking translation servers, updating language files, and managing
	/// translation tasks.
	/// </summary>
	/// <remarks>This method performs a series of steps to validate translation server availability, update language
	/// resources, and process translation queues. It updates the worker status throughout the operation and communicates
	/// status changes to connected clients. Callers should await the returned task to ensure completion of all workflow
	/// steps.</remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
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
		WorkerActualStatus.Reset();
		WorkerActualStatus.StartTime = DateTime.UtcNow;
		WorkerActualStatus.CycleChecksStart = DateTime.UtcNow;
		WorkerActualStatus.ActualStatus = Enums.WorkerStatus.CheckServersAndFiles;
		await _workerHub.Clients.All.SendAsync("WorkerStatusChanged", WorkerActualStatus.ActualStatusText);
		var workerResult = new WorkerResults
		{
			StartTime = DateTime.UtcNow,
			CycleChecks = new CycleChecks
			{
				StartTime = DateTime.UtcNow,
			}
		};
	}
}