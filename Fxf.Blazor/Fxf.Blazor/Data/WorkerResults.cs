using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Data;

public class WorkerResults
{
	public DateTime StartTime { get; set; } = DateTime.UtcNow;
	public DateTime EndTime { get; set; } = DateTime.UtcNow;
	public bool Successful { get; set; } = true;
	public Dictionary<WorkerStatus, string> ErrorMessages { get; set; } = [];
	public WorkerStatus LastStatus { get; set; } = WorkerStatus.Iddle;
	public CycleChecks CycleChecks { get; set; } = new();
}

public class CycleChecks
{
	public bool SettingsLoaded { get; set; } = false;
	public DateTime StartTime { get; set; } = DateTime.UtcNow;
	public DateTime EndTime { get; set; } = DateTime.UtcNow;
	public bool DefaultTranslationFound { get; set; } = true;
	public bool IgnoredLanguagesFound { get; set; } = true;
	public int LibreLanguagesCount { get; set; } = 0;
	public Translations FrontendTranslations { get; set; }
	public Translations BackendTranslations { get; set; }
}

public class LanguagesTranslations
{
	public int TranslationsNeeded { get; set; } = 0;
	public int TranslationsDone { get; set; } = 0;
	public int TranslationErrors { get; set; } = 0;
	public Dictionary<string, List<string>> FailedTranslations { get; set; } = [];
	public DateTime StartTime { get; set; } = DateTime.UtcNow;
	public DateTime EndTime { get; set; } = DateTime.UtcNow;
}

public class Translations
{
	public DateTime StartTime { get; set; } = DateTime.UtcNow;
	public DateTime EndTime { get; set; } = DateTime.UtcNow;
	public bool OldFileFound { get; set; } = false;

	public int TranslationsNeeded { get; set; } = 0;
	public Dictionary<string, TranslationRequests> RequestedTranslations { get; set; } = [];
	public Dictionary<string, TranslationResults> ResultOfTranslating { get; set; } = [];
}

public class TranslationRequests
{
	public int ToAdd { get; set; } = 0;
	public int ToRemove { get; set; } = 0;
	public int ToUpdate { get; set; } = 0;
}

public class TranslationResults
{
	public int SuccessfulTranslations { get; set; }
	public Dictionary<string, string> TranslationErrors { get; set; }
}