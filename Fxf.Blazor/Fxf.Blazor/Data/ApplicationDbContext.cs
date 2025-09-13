using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

			var json = new JsonSerializerOptions { WriteIndented = false };

			var workerResults = builder.Entity<WorkerResults>();
			workerResults.ToTable("WorkerResults");

			workerResults.OwnsOne(x => x.CycleChecks, cc =>
			{
				cc.ToTable("WorkerResults"); // table-splitting

				// --- FrontendTranslations (OwnsOne) ---
				cc.OwnsOne(c => c.FrontendTranslations, ft =>
				{
					ft.ToTable("WorkerResults");

					// Kolekce ServerRequestedTranslations (FRONTEND) -> vlastní tabulka
					ft.OwnsMany(t => t.ServerRequestedTranslations, req =>
					{
						req.ToTable("Frontend_ServerRequestedTranslations");
						req.WithOwner().HasForeignKey("WorkerResultsId");
						req.Property<int>("Id").ValueGeneratedOnAdd();
						req.HasKey("Id");
						req.HasIndex("WorkerResultsId");
					});

					// Kolekce ServerResultOfTranslating (FRONTEND) -> vlastní tabulka + JSON converter
					ft.OwnsMany(t => t.ServerResultOfTranslating, res =>
					{
						res.ToTable("Frontend_ServerResultOfTranslating");
						res.WithOwner().HasForeignKey("WorkerResultsId");
						res.Property<int>("Id").ValueGeneratedOnAdd();
						res.HasKey("Id");
						res.HasIndex("WorkerResultsId");

						res.Property(r => r.TranslationErrors)
							.HasConversion(
								 v => JsonSerializer.Serialize(v ?? new List<string>(), json),
								 v => string.IsNullOrWhiteSpace(v)
										? new List<string>()
										: (JsonSerializer.Deserialize<List<string>>(v!, json) ?? new List<string>())
							);
					});
				});

				cc.OwnsOne(c => c.BackendTranslations, bt =>
				{
					bt.ToTable("WorkerResults");

					bt.OwnsMany(t => t.ServerRequestedTranslations, req =>
					{
						req.ToTable("Backend_ServerRequestedTranslations");
						req.WithOwner().HasForeignKey("WorkerResultsId");
						req.Property<int>("Id").ValueGeneratedOnAdd();
						req.HasKey("Id");
						req.HasIndex("WorkerResultsId");
					});

					bt.OwnsMany(t => t.ServerResultOfTranslating, res =>
					{
						res.ToTable("Backend_ServerResultOfTranslating");
						res.WithOwner().HasForeignKey("WorkerResultsId");
						res.Property<int>("Id").ValueGeneratedOnAdd();
						res.HasKey("Id");
						res.HasIndex("WorkerResultsId");

						res.Property(r => r.TranslationErrors)
							.HasConversion(
								 v => JsonSerializer.Serialize(v ?? new List<string>(), json),
								 v => string.IsNullOrWhiteSpace(v)
										? new List<string>()
										: (JsonSerializer.Deserialize<List<string>>(v!, json) ?? new List<string>())
							);
					});
				});
			});

			workerResults.OwnsOne(wr => wr.LanguagesTranslations, lt =>
			{
				lt.ToTable("WorkerResults");

				lt.OwnsMany(l => l.FailedTranslations, ft =>
				{
					ft.ToTable("FailedTranslations");
					ft.WithOwner().HasForeignKey("WorkerResultsId");
					ft.Property<int>("Id").ValueGeneratedOnAdd();
					ft.HasKey("Id");
					ft.HasIndex("WorkerResultsId");
				});
			});

			workerResults.OwnsOne(wr => wr.TranslationRequests, tr =>
			{
				tr.ToTable("WorkerResults");
			});

			workerResults.OwnsOne(wr => wr.TranslationResults, tr =>
			{
				tr.ToTable("WorkerResults");
				tr.Property(r => r.TranslationErrors)
				.HasConversion(
					v => JsonSerializer.Serialize(v ?? new List<string>(), json),
					v => string.IsNullOrWhiteSpace(v)
					? new List<string>() : JsonSerializer.Deserialize<List<string>>(v, json) ?? new List<string>());
			});

			workerResults.OwnsMany(x => x.ErrorMessages, em =>
			{
				em.ToTable("WorkerErrorMessages");
				em.WithOwner().HasForeignKey("WorkerResultsId");
				em.Property<int>("Id").ValueGeneratedOnAdd();
				em.HasKey("Id");
				em.HasIndex("WorkerResultsId");
			});
		}
	}
}