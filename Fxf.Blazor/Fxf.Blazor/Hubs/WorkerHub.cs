using Fxf.Blazor.Data;
using Microsoft.AspNetCore.SignalR;

namespace Fxf.Blazor.Hubs;

public class WorkerHub(ApplicationDbContext context) : Hub
{
	private readonly ApplicationDbContext _context = context;
}