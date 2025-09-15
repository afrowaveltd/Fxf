using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Services;

/// <summary>
/// Provides functionality to log activity events for SignalR hubs to the application's database.
/// </summary>
/// <remarks>
/// This logger records connection, disconnection, and other hub-related events for auditing and
/// monitoring purposes. It is typically used within SignalR middleware or hub implementations to
/// track user and connection activity. Thread safety is ensured by relying on the provided database
/// context's concurrency model.
/// </remarks>
/// <param name="dbContext">
/// The database context used to persist hub activity logs. Must not be null.
/// </param>
public class HubActivityLogger(ApplicationDbContext dbContext) : IHubActivityLogger
{
	private readonly ApplicationDbContext _dbContext = dbContext;

	/// <summary>
	/// Asynchronously records a hub activity event to the activity log database.
	/// </summary>
	/// <remarks>
	/// If an error occurs while saving the activity log entry, the exception is handled internally
	/// and the failure is reported to the error output stream. The method does not throw on logging failure.
	/// </remarks>
	/// <param name="hub">
	/// The type of hub where the activity occurred. Specifies the logical grouping or service
	/// associated with the event.
	/// </param>
	/// <param name="ev">
	/// The activity event to log. Indicates the action or state change that took place within the hub.
	/// </param>
	/// <param name="connectionId">
	/// The unique identifier for the connection associated with the activity. Used to correlate the
	/// event with a specific client session.
	/// </param>
	/// <param name="userId">
	/// The user identifier associated with the activity, if available. Can be null if the activity
	/// is not linked to a specific user.
	/// </param>
	/// <param name="remoteIpAddress">
	/// The remote IP address of the client that triggered the activity, if available. Can be null if
	/// the address is not known or not applicable.
	/// </param>
	/// <param name="remarks">
	/// Optional remarks or additional information to include with the activity log entry. Can be
	/// null if no extra details are needed.
	/// </param>
	/// <returns>A task that represents the asynchronous logging operation.</returns>
	public async Task LogAsync(HubType hub,
		HubActivityEvent ev,
		string connectionId,
		string? userId = null,
		string? remoteIpAddress = null,
		string? remarks = null)
	{
		var entry = new HubActivityLog
		{
			HubType = hub,
			Event = ev,
			ConnectionId = connectionId,
			UserId = userId,
			RemoteIpAddress = remoteIpAddress,
			Remarks = remarks,
			Timestamp = DateTime.UtcNow
		};

		await _dbContext.HubActivityLogs.AddAsync(entry);
		try
		{
			await _dbContext.SaveChangesAsync();
		}
		catch(Exception ex)
		{
			// Handle exceptions (e.g., log to a file or monitoring system)
			Console.Error.WriteLine($"Failed to log hub activity: {ex.Message}");
		}
	}
}