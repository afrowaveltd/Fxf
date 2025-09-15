namespace Fxf.Blazor.Models;

/// <summary>
/// Contains application-wide enumerations for worker status and related concepts.
/// </summary>
public class Enums
{
	/// <summary>
	/// Specifies the types of activity events that can occur on a hub connection.
	/// </summary>
	/// <remarks>
	/// Use this enumeration to identify the current state or activity event of a hub connection,
	/// such as when a connection is established or terminated. The value <see
	/// cref="HubActivityEvent.Unknown"/> indicates an undefined or unrecognized event.
	/// </remarks>
	public enum HubActivityEvent
	{
		/// <summary>
		/// Indicates that the value or state is unknown or has not been specified.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Indicates that the connection state is established and active.
		/// </summary>
		Connected = 1,

		/// <summary>
		/// Indicates that the connection state is disconnected.
		/// </summary>
		Disconnected = 2
	}

	/// <summary>
	/// Specifies the available hub types used for communication or coordination within the system.
	/// </summary>
	/// <remarks>
	/// Each value represents a distinct hub with a specific role, such as worker processing,
	/// localization, or indexing. Use this enumeration to select or identify the appropriate hub for
	/// a given operation.
	/// </remarks>
	public enum HubType
	{
		/// <summary>
		/// Specifies the Worker Hub role in the system.
		/// </summary>
		Worker = 0,

		/// <summary>
		/// Represents the localization hub service type.
		/// </summary>
		Localization = 1,

		/// <summary>
		/// Specifies that the hub is used for indexing operations.
		/// </summary>
		Index = 3,

		/// <summary>
		/// Represents a chat message type within the messaging system.
		/// </summary>
		Chat = 4
	}

	/// <summary>
	/// Specifies the type of change applied to a phrase in a collection or data set.
	/// </summary>
	/// <remarks>
	/// Use this enumeration to indicate whether a phrase is being added, removed, or updated. This
	/// is typically used in scenarios where tracking or responding to modifications in a set of
	/// phrases is required, such as in event arguments or change notifications.
	/// </remarks>
	public enum PhraseChange
	{
		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		Add,

		/// <summary>
		/// Removes the specified element from the collection.
		/// </summary>
		Remove,

		/// <summary>
		/// Updates the current object or state with new information or changes.
		/// </summary>
		Update
	}

	/// <summary>
	/// Specifies the target area for translation operations.
	/// </summary>
	/// <remarks>
	/// Use this enumeration to indicate whether a translation applies to language resources,
	/// frontend components, or backend components. The value selected determines which part of the
	/// application will be affected by translation processes.
	/// </remarks>
	public enum TranslationTarget
	{
		/// <summary>
		/// Gets the collection of supported languages.
		/// </summary>
		Languages,

		/// <summary>
		/// Represents the user interface component responsible for interacting with users and
		/// displaying application data.
		/// </summary>
		/// <remarks>
		/// The Frontend typically handles input from users and presents information received from
		/// backend services or business logic layers. It may include controls, views, or other UI
		/// elements depending on the application's architecture.
		/// </remarks>
		Frontend,

		/// <summary>
		/// Represents the backend system or component responsible for processing data or handling
		/// core application logic.
		/// </summary>
		Backend
	}

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