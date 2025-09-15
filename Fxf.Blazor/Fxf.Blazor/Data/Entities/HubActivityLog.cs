using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents a log entry for activity events on a SignalR hub connection, including connection
/// details, user information, and event metadata.
/// </summary>
public class HubActivityLog
{
	/// <summary>
	/// Gets or sets the unique SignalR connection identifier associated with the event.
	/// </summary>
	[Required]
	public string ConnectionId { get; set; } = default!;

	/// <summary>
	/// Gets or sets the type of activity event (e.g., connected, disconnected) for the hub connection.
	/// </summary>
	[Required]
	public HubActivityEvent Event { get; set; } = HubActivityEvent.Unknown;

	/// <summary>
	/// Gets or sets the type of hub where the activity occurred.
	/// </summary>
	[Required]
	public HubType HubType { get; set; } = HubType.Index;

	/// <summary>
	/// Gets or sets the unique identifier for the log entry.
	/// </summary>
	[Key]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets any additional remarks or metadata related to the activity event.
	/// </summary>
	public string? Remarks { get; set; }

	/// <summary>
	/// Gets or sets the remote IP address of the client that triggered the event, if available.
	/// </summary>
	public string? RemoteIpAddress { get; set; }

	/// <summary>
	/// Gets or sets the UTC timestamp when the activity event occurred.
	/// </summary>
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the application user associated with the connection, if available.
	/// </summary>
	public ApplicationUser? User { get; set; }

	/// <summary>
	/// Gets or sets the user identifier associated with the connection, if available.
	/// </summary>
	public string? UserId { get; set; }
}