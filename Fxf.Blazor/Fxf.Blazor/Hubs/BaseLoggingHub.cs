using System.Security.Claims;
using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Hubs;

/// <summary>
/// Provides a base implementation for SignalR hubs that automatically log connection and disconnection events using an activity logger.
/// </summary>
/// <remarks>
/// Inherit from this class to enable automatic logging of hub activity events, including connection and disconnection, for auditing and monitoring purposes.
/// </remarks>
/// <param name="activityLogger">The activity logger used to record hub events.</param>
public abstract class BaseLoggingHub(IHubActivityLogger activityLogger) : Hub
{
   private readonly IHubActivityLogger _activityLogger = activityLogger;

   /// <summary>
   /// Gets the type of hub associated with this logging hub.
   /// </summary>
   protected abstract HubType HubType { get; }

   /// <summary>
   /// Called when a new connection is established with the hub. Logs the connection event asynchronously.
   /// </summary>
   /// <returns>A task that represents the asynchronous connect operation.</returns>
   public override async Task OnConnectedAsync()
   {
      var connectionId = Context.ConnectionId;
      var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var remoteIpAddress = Context.GetHttpContext()?.Connection.RemoteIpAddress?.ToString();

      await _activityLogger.LogAsync(HubType, HubActivityEvent.Connected, connectionId, userId, remoteIpAddress, "Connected");
      await base.OnConnectedAsync();
   }

   /// <summary>
   /// Called when a connection with the hub is terminated. Logs the disconnection event asynchronously.
   /// </summary>
   /// <param name="exception">The exception that caused the disconnect, if any.</param>
   /// <returns>A task that represents the asynchronous disconnect operation.</returns>
   public override async Task OnDisconnectedAsync(Exception? exception)
   {
      var connectionId = Context.ConnectionId;
      var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var remoteIpAddress = Context.GetHttpContext()?.Connection.RemoteIpAddress?.ToString();
      var remarks = exception != null ? $"Disconnected with error: {exception.Message}" : "Graceful Disconnect";
      await _activityLogger.LogAsync(HubType, HubActivityEvent.Disconnected, connectionId, userId, remoteIpAddress, remarks);
      await base.OnDisconnectedAsync(exception);
   }
}
