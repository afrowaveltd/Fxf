using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents an error that occurred during the execution of a worker process.
/// </summary>
/// <remarks>
/// This class is typically used to log or track errors encountered by background workers or
/// services. It contains information about the error message, the time the error occurred, and the
/// status of the worker at the time of the error.
/// </remarks>
public class WorkerError
{
	/// <summary>
	/// Gets or sets the message content.
	/// </summary>
	public string Message { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the date and time value associated with this instance.
	/// </summary>
	public DateTime Time { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the current status of the worker.
	/// </summary>
	public WorkerStatus WorkerStatus { get; set; } = new();
}