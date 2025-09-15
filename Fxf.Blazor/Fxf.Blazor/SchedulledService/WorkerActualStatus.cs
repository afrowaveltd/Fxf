using Fxf.Blazor.Data.Entities;
using Fxf.Blazor.Models;
using Fxf.Shared.Models;
using Microsoft.AspNetCore.StaticAssets;
using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.SchedulledService;

public static class WorkerActualStatus
{
	// properties

	// For all stages of the worker process
	public static WorkerStatus ActualStatus { get; set; } = WorkerStatus.Iddle;

	public static string ActualStatusText => ActualStatus.StatusToText();
	public static DateTime StartTime { get; set; } = DateTime.UtcNow;
	public static DateTime EndTime { get; set; } = DateTime.UtcNow;

	// For CycleChecks stage
	public static DateTime CycleChecksStart { get; set; } = DateTime.UtcNow;

	public static DateTime CycleChecksEnd { get; set; } = DateTime.UtcNow;
	public static bool SettingsLoaded { get; set; } = false;
	public static CycleChecks CycleChecks { get; set; } = new();
	public static List<Language> LibreLanguages { get; set; } = [];
	public static List<string> IgnoredLanguages { get; set; } = [];
	public static bool CanContinueToLanguageTranslations { get; set; } = false;

	// For LanguagesTranslations stage
	public static DateTime LanguagesTranslationsStart { get; set; } = DateTime.UtcNow;

	public static DateTime LanguagesTranslationsEnd { get; set; } = DateTime.UtcNow;
	public static Dictionary<string, string> LanguagesToTranslate { get; set; } = [];
	public static List<TranslationError> LanguagesTranslationErrors { get; set; } = [];
	public static bool CanContinueToFrontendTranslations { get; set; } = false;

	// For FrontendAndBackendTranslations stage
	public static DateTime FrontendTranslationsStart { get; set; } = DateTime.UtcNow;

	public static DateTime FrontendTranslationsEnd { get; set; } = DateTime.UtcNow;
	public static DateTime BackendTranslationsStart { get; set; } = DateTime.UtcNow;
	public static DateTime BackendTranslationsEnd { get; set; } = DateTime.UtcNow;
	public static bool FrontendOldTranslationFound { get; set; } = false;
	public static bool BackendOldTranslationFound { get; set; } = false;

	// methods
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