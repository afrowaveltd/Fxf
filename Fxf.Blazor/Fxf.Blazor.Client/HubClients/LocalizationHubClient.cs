
using Microsoft.AspNetCore.SignalR.Client;

namespace Fxf.Blazor.Client.HubClients;

public class LocalizationHubClient : IAsyncDisposable
{
	private readonly HubConnection _conn;
	public async ValueTask DisposeAsync()
	{
		try { await _conn.DisposeAsync(); }
		catch { /* ignore */ }
	}
}
