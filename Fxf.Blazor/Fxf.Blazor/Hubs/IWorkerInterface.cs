using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Hubs;

public interface IWorkerInterface
{
	Task CycleStarted();

	Task StageCompleted<T>(WorkerStatus status, T stageResults);

	Task StatusChanged(WorkerStatus status);
}