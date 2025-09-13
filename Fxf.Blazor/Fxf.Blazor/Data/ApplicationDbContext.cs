using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fxf.Blazor.Data
{
	/// <summary>
	/// Represents the database context for the application, providing access to the application's data stores.
	/// </summary>
	/// <remarks>This context integrates with ASP.NET Core Identity and is configured to use the <see
	/// cref="ApplicationUser"/> entity  for user management. It inherits from <see cref="IdentityDbContext{TUser}"/> to
	/// provide Identity-related functionality.</remarks>
	/// <param name="options"></param>
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
	{
		/// <summary>
		/// Gets or sets the collection of worker results in the database context.
		/// </summary>
		public DbSet<WorkerResults> WorkerResults { get; set; } = null!;

		/// <summary>
		/// Configures the entity model for the context using the specified model builder.
		/// </summary>
		/// <remarks>This method is called by Entity Framework when the model for the context is being created.
		/// Override this method to customize the model by configuring entities, relationships, and owned types. Always call
		/// the base implementation to ensure that the default configuration is applied.</remarks>
		/// <param name="builder">The builder used to construct the model for the context. Provides configuration for entity types, relationships,
		/// and owned types.</param>
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<WorkerResults>();
		}
	}
}