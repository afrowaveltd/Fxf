using Fxf.Blazor.Models.Settings;

namespace Fxf.Blazor.SchedulledService;

/// <summary>
/// Provides a background scheduled service for periodic localization and translation updates.
/// Uses a timer to trigger translation operations at configurable intervals.
/// </summary>
/// <remarks>
/// The scheduler loads configuration from the <c>Localization</c> section, and periodically invokes
/// <see cref="IUITranslationService.RunAsync"/> to update translations. It supports dependency injection
/// for translation and language services, and ensures only one operation runs at a time.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="Scheduller"/> class.
/// </remarks>
/// <param name="configuration">The application configuration.</param>
/// <param name="serviceProvider">The service provider for creating scopes and resolving services.</param>
public class Scheduller(
	IConfiguration configuration,
	IServiceProvider serviceProvider) : IHostedService, IDisposable
{
	private readonly IConfiguration _configuration = configuration;
	private readonly IServiceProvider _serviceProvider = serviceProvider;
	private Timer? _timer;
	private volatile bool _isProcessing = false;

	/// <summary>
	/// Gets the localization configuration section.
	/// </summary>
	private Localization Localization => _configuration.GetSection("Localization").Get<Localization>() ?? new Localization();

	private int IntervalMinutes => Localization.MinutesBetweenCycles != 0 ? Localization.MinutesBetweenCycles : 10;

	/// <summary>
	/// Starts the scheduled background service.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token for graceful shutdown.</param>
	/// <returns>A completed task.</returns>
	public Task StartAsync(CancellationToken cancellationToken)
	{
		_timer = new Timer(async _ => await DoWorkAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(IntervalMinutes));
		return Task.CompletedTask;
	}

	/// <summary>
	/// Stops the scheduled background service.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token for graceful shutdown.</param>
	/// <returns>A completed task.</returns>
	public Task StopAsync(CancellationToken cancellationToken)
	{
		_timer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Disposes the timer and releases resources.
	/// </summary>
	public void Dispose()
	{
		_timer?.Dispose();
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Executes the scheduled work asynchronously, ensuring only one operation runs at a time.
	/// Invokes the <see cref="IUITranslationService.RunAsync"/> method in a scoped context.
	/// </summary>
	private async Task DoWorkAsync()
	{
		if(_isProcessing)
		{
			return;
		}
		_isProcessing = true;

		try
		{
			using IServiceScope scope = _serviceProvider.CreateScope();
			IUITranslationService uiTranslationService = scope.ServiceProvider.GetRequiredService<IUITranslationService>();
			await uiTranslationService.RunAsync();
		}
		catch(Exception ex)
		{
			// Log the exception (you can use any logging framework you prefer)
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
		finally
		{
			_isProcessing = false;
		}
	}
}