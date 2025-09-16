namespace Fxf.Blazor.Hubs;

public class IndexHub : BaseLoggingHub
{
	public IndexHub(IHubActivityLogger activityLogger) : base(activityLogger)
	{
	}

	protected override Enums.HubType HubType => Enums.HubType.Localization;
}