using System.ComponentModel.DataAnnotations;
using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents a log entry for a SignalR connection, used to track connection times, user identity, and hub activity.
/// Enables calculation of active time for different users, especially for authenticated sessions.
/// </summary>
public class SignalRConnection
{
	/// <summary>
	/// Gets or sets the unique identifier for the SignalR connection.
	/// </summary>
	[Key]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the user identifier associated with the connection, if authenticated.
	/// </summary>
	public string? UserId { get; set; } = null;

	/// <summary>
	/// Gets or sets the UTC timestamp when the connection was established.
	/// </summary>
	public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the UTC timestamp when the connection was closed, or null if still active.
	/// </summary>
	public DateTime? DisconnectedAt { get; set; } = null;

	/// <summary>
	/// Gets or sets the user agent string of the connecting client.
	/// </summary>
	public string? UserAgent { get; set; } = null;

	/// <summary>
	/// Gets or sets the IP address of the connecting client.
	/// </summary>
	public string? IpAddress { get; set; } = null;

	/// <summary>
	/// Gets or sets the hub type associated with this connection.
	/// </summary>
	public ActiveHub Hub { get; set; } = ActiveHub.IndexHub;

	/// <summary>
	/// Gets or sets a value indicating whether the user was authenticated during the connection.
	/// </summary>
	public bool IsAuthenticated { get; set; } = false;

	/// <summary>
	/// Gets the total active time for this connection, if disconnected; otherwise, the duration since connection.
	/// </summary>
	public TimeSpan? ActiveDuration =>
		 DisconnectedAt.HasValue ? DisconnectedAt.Value - ConnectedAt : DateTime.UtcNow - ConnectedAt;

	/// <summary>
	/// Gets or sets the user associated with the current context.
	/// </summary>
	public ApplicationUser? User { get; set; } = null;
}