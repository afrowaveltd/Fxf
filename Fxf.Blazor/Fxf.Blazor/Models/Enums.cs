namespace Fxf.Blazor.Models;

/// <summary>
/// Contains application-wide enumerations for worker status and related concepts.
/// </summary>
public class Enums
{
	/// <summary>
	/// Represents the status of a background worker or scheduled process.
	/// </summary>
	public enum WorkerStatus
	{
		/// <summary>
		/// The worker is idle and not performing any operation.
		/// </summary>
		Iddle = 0,
		/// <summary>
		/// The worker is checking servers and files for updates or changes.
		/// </summary>
		CheckServersAndFiles = 1,
		/// <summary>
		/// The worker is checking language translations for completeness or accuracy.
		/// </summary>
		CheckLanguagesTranslations = 2,
		/// <summary>
		/// The worker is translating frontend (client-side) resources.
		/// </summary>
		TranslatingFrontend = 3,
		/// <summary>
		/// The worker is translating backend (server-side) resources.
		/// </summary>
		TranslatingBackend = 4,
		/// <summary>
		/// The worker is storing changes or results to persistent storage.
		/// </summary>
		StoringChanges = 5
	}
}
