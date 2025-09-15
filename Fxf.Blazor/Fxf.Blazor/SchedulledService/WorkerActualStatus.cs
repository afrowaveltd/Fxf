using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.SchedulledService;

/// <summary>
/// Provides static properties and methods to track and manage the actual status and progress of the
/// background worker process. Includes timing, status, and stage-specific data for the worker lifecycle.
/// </summary>
public static class WorkerActualStatus
{
	/// <summary>
	/// Gets or sets the current status of the worker process.
	/// </summary>
	public static WorkerStatus ActualStatus { get; set; } = WorkerStatus.Iddle;

	/// <summary>
	/// Gets the textual representation of the current worker status.
	/// </summary>
	public static string ActualStatusText => ActualStatus.StatusToText();

	/// <summary>
	/// Gets or sets a value indicating whether an old backend translation was found.
	/// </summary>
	public static bool BackendOldTranslationFound { get; set; } = false;

	/// <summary>
	/// Gets or sets the UTC end time of the backend translations stage.
	/// </summary>
	public static DateTime BackendTranslationsEnd { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC start time of the backend translations stage.
	/// </summary>
	public static DateTime BackendTranslationsStart { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets a value indicating whether the worker can continue to the frontend translations stage.
	/// </summary>
	public static bool CanContinueToFrontendTranslations { get; set; } = false;

	/// <summary>
	/// Gets or sets a value indicating whether the worker can continue to the language translations stage.
	/// </summary>
	public static bool CanContinueToLanguageTranslations { get; set; } = false;

	/// <summary>
	/// Gets or sets the cycle checks data for the worker process.
	/// </summary>
	public static CycleChecks CycleChecks { get; set; } = new();

	/// <summary>
	/// Gets or sets the UTC end time of the cycle checks stage.
	/// </summary>
	public static DateTime CycleChecksEnd { get; set; } = DateTime.UtcNow;

	// CycleChecks stage
	/// <summary>
	/// Gets or sets the UTC start time of the cycle checks stage.
	/// </summary>
	public static DateTime CycleChecksStart { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC end time of the worker process.
	/// </summary>
	public static DateTime EndTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets a value indicating whether an old frontend translation was found.
	/// </summary>
	public static bool FrontendOldTranslationFound { get; set; } = false;

	/// <summary>
	/// Gets or sets the UTC end time of the frontend translations stage.
	/// </summary>
	public static DateTime FrontendTranslationsEnd { get; set; } = DateTime.UtcNow;

	// FrontendAndBackendTranslations stage
	/// <summary>
	/// Gets or sets the UTC start time of the frontend translations stage.
	/// </summary>
	public static DateTime FrontendTranslationsStart { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the list of ignored languages during cycle checks.
	/// </summary>
	public static List<string> IgnoredLanguages { get; set; } = [];

	/// <summary>
	/// Gets or sets the dictionary of languages to translate and their status.
	/// </summary>
	public static Dictionary<string, string> LanguagesToTranslate { get; set; } = [];

	/// <summary>
	/// Gets or sets the list of translation errors encountered during language translations.
	/// </summary>
	public static List<TranslationError> LanguagesTranslationErrors { get; set; } = [];

	/// <summary>
	/// Gets or sets the UTC end time of the language translations stage.
	/// </summary>
	public static DateTime LanguagesTranslationsEnd { get; set; } = DateTime.UtcNow;

	// LanguagesTranslations stage
	/// <summary>
	/// Gets or sets the UTC start time of the language translations stage.
	/// </summary>
	public static DateTime LanguagesTranslationsStart { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the list of Libre languages detected during cycle checks.
	/// </summary>
	public static List<Language> LibreLanguages { get; set; } = [];

	/// <summary>
	/// Gets or sets the number of old database entries removed during the storing changes stage.
	/// </summary>
	public static int OldDatabaseEntriesRemoved { get; set; } = 0;

	/// <summary>
	/// Gets or sets a value indicating whether old translations were stored successfully.
	/// </summary>
	public static bool OldTranslationsStored { get; set; } = false;

	/// <summary>
	/// Gets or sets a value indicating whether settings were loaded during cycle checks.
	/// </summary>
	public static bool SettingsLoaded { get; set; } = false;

	/// <summary>
	/// Gets or sets the UTC start time of the worker process.
	/// </summary>
	public static DateTime StartTime { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC end time of the storing changes stage.
	/// </summary>
	public static DateTime StoringChangesEnd { get; set; } = DateTime.UtcNow;

	// StoringChanges stage
	/// <summary>
	/// Gets or sets the UTC start time of the storing changes stage.
	/// </summary>
	public static DateTime StoringChangesStart { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the queue of translation modifications to be processed.
	/// </summary>
	public static List<TranslationToModify> TranslationQueue { get; set; } = [];

	/// <summary>
	/// Gets or sets a value indicating whether translations were stored successfully.
	/// </summary>
	public static bool TranslationsStored { get; set; } = false;

	/// <summary>
	/// Resets all properties of the WorkerActualStatus class to their default values.
	/// </summary>
	public static void Reset()
	{
		ActualStatus = WorkerStatus.Iddle;
		StartTime = DateTime.UtcNow;
		EndTime = DateTime.UtcNow;
		CycleChecksStart = DateTime.UtcNow;
		CycleChecksEnd = DateTime.UtcNow;
		SettingsLoaded = false;
		CycleChecks = new();
		LibreLanguages = [];
		IgnoredLanguages = [];
		CanContinueToLanguageTranslations = false;
		LanguagesTranslationsStart = DateTime.UtcNow;
		LanguagesTranslationsEnd = DateTime.UtcNow;
		LanguagesToTranslate = [];
		LanguagesTranslationErrors = [];
		CanContinueToFrontendTranslations = false;
		FrontendTranslationsStart = DateTime.UtcNow;
		FrontendTranslationsEnd = DateTime.UtcNow;
		BackendTranslationsStart = DateTime.UtcNow;
		BackendTranslationsEnd = DateTime.UtcNow;
		FrontendOldTranslationFound = false;
		BackendOldTranslationFound = false;
		TranslationQueue = [];
		StoringChangesStart = DateTime.UtcNow;
		StoringChangesEnd = DateTime.UtcNow;
		TranslationsStored = false;
		OldTranslationsStored = false;
		OldDatabaseEntriesRemoved = 0;
	}

	/// <summary>
	/// Converts a WorkerStatus value to its corresponding display text.
	/// </summary>
	/// <param name="status">The worker status value.</param>
	/// <returns>The display text for the status.</returns>
	public static string StatusToText(this Enums.WorkerStatus status) => status switch
	{
		WorkerStatus.Iddle => "Idle",
		WorkerStatus.CheckServersAndFiles => "Checking servers and files",
		WorkerStatus.CheckLanguagesTranslations => "Translating language names",
		WorkerStatus.TranslatingFrontend => "Translating frontend",
		WorkerStatus.TranslatingBackend => "Translating backend",
		WorkerStatus.StoringChanges => "Storing changes",
		_ => "Unknown"
	};
}