using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Hubs;

/// <summary>
/// Defines the contract for worker status notifications and stage reporting in the background worker hub.
/// </summary>
/// <remarks>
/// This interface is used for communication between the server and clients regarding the progress and status
/// of background worker operations. It provides methods for signaling the start of a cycle, completion of stages,
/// and status changes.
/// </remarks>
public interface IWorkerInterface
{
	/// <summary>
	/// Notifies clients that a new worker cycle has started.
	/// </summary>
	Task CycleStarted();

	/// <summary>
	/// Notifies clients that a specific stage of the worker process has completed, providing the status and results.
	/// </summary>
	/// <typeparam name="T">The type of the results for the completed stage.</typeparam>
	/// <param name="status">The status of the worker at the time of stage completion.</param>
	/// <param name="stageResults">The results of the completed stage.</param>
	Task StageCompleted<T>(WorkerStatus status, T stageResults);

	/// <summary>
	/// Notifies clients that the worker status has changed.
	/// </summary>
	/// <param name="status">The new status of the worker.</param>
	Task StatusChanged(WorkerStatus status);
}