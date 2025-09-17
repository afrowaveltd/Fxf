using Fxf.Blazor.Data;
using Microsoft.AspNetCore.SignalR;
using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Hubs;

/// <summary>
/// Provides a SignalR hub for managing worker connections and activities, with integrated logging
/// and access to application data.
/// </summary>
/// <remarks>
/// This hub is intended for scenarios where worker clients require real-time communication and
/// activity tracking. Inherits logging capabilities from <see cref="BaseLoggingHub"/> and exposes
/// worker-specific functionality. Thread safety and connection management are handled by the
/// underlying SignalR infrastructure.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the WorkerHub class with the specified activity logger and
/// application database context.
/// </remarks>
/// <param name="activityLogger">The activity logger used to record hub activity events and diagnostics.</param>
/// <param name="context">
/// The application database context used for data access operations within the hub.
/// </param>
public class WorkerHub(IHubActivityLogger activityLogger, ApplicationDbContext context) : BaseLoggingHub(activityLogger)
{
	private readonly ApplicationDbContext _context = context;

	/// <summary>
	/// Gets the type of hub associated with this instance.
	/// </summary>
	protected override HubType HubType => HubType.Worker;
}