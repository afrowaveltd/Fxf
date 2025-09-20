using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Data
{
	/// <summary>
	/// Represents the database context for the application, providing access to the application's
	/// data stores.
	/// </summary>
	/// <remarks>
	/// This context integrates with ASP.NET Core Identity and is configured to use the <see
	/// cref="ApplicationUser"/> entity for user management. It inherits from <see
	/// cref="IdentityDbContext{TUser}"/> to provide Identity-related functionality.
	/// </remarks>
	/// <param name="options"></param>
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole, string>(options)
	{
		/// <summary>
		/// Gets or sets the collection of hub activity log entries for the context.
		/// </summary>
		public DbSet<HubActivityLog> HubActivityLogs { get; set; } = null!;

		/// <summary>
		/// Gets or sets the collection of SignalR connection entities for the context.
		/// </summary>
		public DbSet<SignalRConnection> SignalRConnections { get; set; } = null!;

		/// <summary>
		/// Gets or sets the collection of worker results in the database context.
		/// </summary>
		public DbSet<WorkerResults> WorkerResults { get; set; } = null!;

		/// <summary>
		/// Configures the entity model for the context using the specified model builder.
		/// </summary>
		/// <remarks>
		/// This method is called by Entity Framework when the model for the context is being created.
		/// Override this method to customize the model by configuring entities, relationships, and
		/// owned types. Always call the base implementation to ensure that the default configuration
		/// is applied.
		/// </remarks>
		/// <param name="builder">
		/// The builder used to construct the model for the context. Provides configuration for entity
		/// types, relationships, and owned types.
		/// </param>
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var json = new JsonSerializerOptions { WriteIndented = false };
			var hubConverter = new EnumToStringConverter<HubType>();
			var hubEventConverter = new EnumToStringConverter<HubActivityEvent>();
			var workerResults = builder.Entity<WorkerResults>();
			var hubActivityLog = builder.Entity<HubActivityLog>();
			workerResults.ToTable("WorkerResults");

			workerResults.OwnsOne(x => x.CycleChecks, cc =>
			{
				cc.ToTable("WorkerResults"); // table-splitting

				// --- FrontendTranslations (OwnsOne) ---
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

			workerResults.OwnsOne(wr => wr.BackendTranslations, bt =>
			{
				bt.ToTable("BackendTranslations");

				bt.OwnsMany(s => s.RequestedTranslations, tr =>
				{
					bt.ToTable("BackendTranslationRequests");
					bt.WithOwner().HasForeignKey("TranslationsId");
					bt.Property<int>("Id").ValueGeneratedOnAdd();
					bt.HasKey("Id");
					bt.HasIndex("TranslationsId");
				});

				bt.OwnsMany(s => s.ResultOfTranslating, rt =>
				{
					rt.ToTable("BackendTranslationResults");
					rt.WithOwner().HasForeignKey("TranslationsId");
					rt.Property<int>("Id").ValueGeneratedOnAdd();
					rt.HasKey("Id");
					rt.HasIndex("TranslationsId");
				});
			});

			workerResults.OwnsOne(wr => wr.FrontendTranslations, ft =>
			{
				ft.ToTable("FrontendTranslations");

				ft.OwnsMany(x => x.RequestedTranslations, tr =>
				{
					ft.ToTable("FrontendTranslationRequests");
					ft.WithOwner().HasForeignKey("TranslationsId");
					ft.Property<int>("Id").ValueGeneratedOnAdd();
					ft.HasKey("Id");
					ft.HasIndex("TranslationsId");
				});

				ft.OwnsMany(s => s.ResultOfTranslating, rt =>
				{
					ft.ToTable("FrontendTranslationResults");
					ft.WithOwner().HasForeignKey("TranslationsId");
					ft.Property<int>("Id").ValueGeneratedOnAdd();
					ft.HasKey("Id");
					ft.HasIndex("TranslationsId");
				});
			});

			workerResults.OwnsMany(x => x.ErrorMessages, em =>
			{
				em.ToTable("WorkerErrorMessages");
				em.WithOwner().HasForeignKey("WorkerResultsId");
				em.Property<int>("Id").ValueGeneratedOnAdd();
				em.HasKey("Id");
				em.HasIndex("WorkerResultsId");
			});

			workerResults.OwnsMany(x => x.TranslationErrors, x =>
			{
				x.ToTable("WorkerErrors");
				x.WithOwner().HasForeignKey("WorkerResultsId");
				x.Property<int>("Id").ValueGeneratedOnAdd();
				x.HasKey("Id");
				x.HasIndex("WorkerResultsId");
			});

			workerResults.OwnsOne(wr => wr.CleanupResults, cr =>
			{
				cr.ToTable("WorkerResults");
			});

			hubActivityLog.Property(e => e.HubType).HasConversion(hubConverter);
			hubActivityLog.Property(e => e.Event).HasConversion(hubEventConverter);
			hubActivityLog.HasIndex(e => e.ConnectionId);
			hubActivityLog.HasIndex(e => e.UserId);
			hubActivityLog.HasOne(e => e.User)
				.WithMany(s => s.HubActivities)
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}