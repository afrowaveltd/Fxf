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

	/// <summary>
	/// Specifies the available hub types used for communication or coordination within the system.
	/// </summary>
	/// <remarks>Each value represents a distinct hub with a specific role, such as worker processing, localization,
	/// or indexing. Use this enumeration to select or identify the appropriate hub for a given operation.</remarks>
	public enum ActiveHub
	{
		/// <summary>
		/// Specifies the Worker Hub role in the system.
		/// </summary>
		WorkerHub = 0,

		/// <summary>
		/// Represents the localization hub service type.
		/// </summary>
		LocalizationHub = 1,

		/// <summary>
		/// Specifies that the hub is used for indexing operations.
		/// </summary>
		IndexHub = 3
	}
}