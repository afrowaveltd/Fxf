using Fxf.Blazor.Data.Entities;

namespace Fxf.Blazor.SchedulledService;

internal class WorkerResult
{
	public DateTime StartTime { get; set; }
	public CycleChecks CycleChecks { get; set; }
}