using Microsoft.AspNetCore.Identity;

namespace Fxf.Blazor.Data.Entities
{
	/// <summary>
	/// Represents an application user with support for SignalR connections.
	/// </summary>
	/// <remarks>Inherits from <see cref="IdentityUser"/> to provide standard ASP.NET Core Identity functionality.
	/// Adds support for tracking SignalR connections associated with the user.</remarks>
	public class ApplicationUser : IdentityUser
	{
		/// <summary>
		/// Gets or sets the collection of active SignalR connections associated with this instance.
		/// </summary>
		public List<SignalRConnection>? Connections { get; set; } = [];
	}
}