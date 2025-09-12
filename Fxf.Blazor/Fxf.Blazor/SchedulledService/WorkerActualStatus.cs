namespace Fxf.Blazor.SchedulledService;

public static class WorkerActualStatus
{
}

public enum WorkerStatus
{
	Iddle = 0,
	CheckServersAndFiles = 1,
	CheckLanguagesTranslations = 2,
	TranslatingFrontend = 3,
	TranslatingBackend = 4,
	StoringChanges = 5
}