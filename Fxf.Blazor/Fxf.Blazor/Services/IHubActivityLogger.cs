namespace Fxf.Blazor.Services;

/// <summary>
/// Defines a contract for logging activity events related to a hub connection asynchronously.
/// </summary>
/// <remarks>
/// Implementations of this interface can be used to record hub activity such as connections,
/// disconnections, and other events for auditing or monitoring purposes. The logger may store
/// events in various backends, such as databases or external logging services. This interface is
/// typically used in real-time communication scenarios where tracking user and connection activity
/// is required.
/// </remarks>
public interface IHubActivityLogger
{
	/// <summary>
	/// Asynchronously records a hub activity event for a specified connection, optionally including
	/// user and network details.
	/// </summary>
	/// <remarks>
	/// This method is thread-safe and can be called concurrently from multiple threads. Logging is
	/// performed asynchronously and does not block the calling thread.
	/// </remarks>
	/// <param name="hub">
	/// The type of hub where the activity occurred. Determines the context for the event being logged.
	/// </param>
	/// <param name="ev">
	/// The activity event to log. Specifies the nature of the action or occurrence within the hub.
	/// </param>
	/// <param name="connectionId">
	/// The unique identifier for the connection associated with the event. Cannot be null or empty.
	/// </param>
	/// <param name="userId">
	/// The identifier of the user associated with the event, if available. May be null if the event
	/// is not user-specific.
	/// </param>
	/// <param name="remoteIpAddress">
	/// The remote IP address from which the connection originated, if available. May be null if not applicable.
	/// </param>
	/// <param name="remarks">
	/// Optional remarks or additional information to include with the event log. May be null if no
	/// extra details are needed.
	/// </param>
	/// <returns>A task that represents the asynchronous logging operation.</returns>
	Task LogAsync(Enums.HubType hub,
		Enums.HubActivityEvent ev,
		string connectionId,
		string? userId = null,
		string? remoteIpAddress = null,
		string? remarks = null);
}