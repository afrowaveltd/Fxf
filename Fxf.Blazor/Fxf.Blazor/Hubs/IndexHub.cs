using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Hubs;

/// <summary>
/// SignalR hub for the main index page. Handles real-time communication between client and server
/// for index-related events. Inherits logging functionality from BaseLoggingHub.
/// </summary>
public class IndexHub : BaseLoggingHub
{
	/// <summary>
	/// Initializes a new instance of the <see cref="IndexHub"/> class with the specified activity logger.
	/// </summary>
	/// <param name="activityLogger">Logger for hub activity events.</param>
	public IndexHub(IHubActivityLogger activityLogger) : base(activityLogger)
	{
	}

	/// <summary>
	/// Gets the type of hub associated with this logging hub.
	/// </summary>
	protected override HubType HubType => HubType.Index;
}