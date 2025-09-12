namespace Fxf.Blazor.Client.Services;

using Microsoft.JSInterop;
using System.Globalization;

/// <summary>
/// Provides functionality for managing locale-related operations, including retrieving browser locales, managing
/// user-preferred cultures, and obtaining the browser's time zone.
/// </summary>
/// <remarks>This service interacts with a JavaScript module to perform locale-related operations, such as
/// retrieving browser locales and managing user preferences. It also allows applying a culture to the current thread
/// and optionally persisting it in local storage. The service implements <see cref="IAsyncDisposable"/> to ensure
/// proper disposal of the JavaScript module reference.</remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="LocaleService"/> class.
/// </remarks>
/// <param name="js">The JavaScript runtime for interop calls.</param>
public class LocaleService(IJSRuntime js) : ILocaleService, IAsyncDisposable
{
	private readonly IJSRuntime _js = js;
	private IJSObjectReference? _mod;

	/// <summary>
	/// Loads the JavaScript module for locale operations.
	/// </summary>
	/// <returns>The JavaScript module reference.</returns>
	private async Task<IJSObjectReference> Module()
		 => _mod ??= await _js.InvokeAsync<IJSObjectReference>("import", "./js/locale.js");

	/// <summary>
	/// Gets the list of preferred browser locales.
	/// </summary>
	/// <returns>An array of locale strings.</returns>
	public async Task<string[]> GetBrowserLocalesAsync()
		 => await (await Module()).InvokeAsync<string[]>("getBrowserLocales");

	/// <summary>
	/// Gets the user's preferred culture from local storage.
	/// </summary>
	/// <returns>The two-letter culture code, or null if not set.</returns>
	public async Task<string?> GetPreferredCultureAsync()
		 => await (await Module()).InvokeAsync<string?>("getPreferredCulture");

	/// <summary>
	/// Saves the specified culture information in a browser cookie for use in Blazor applications.
	/// </summary>
	/// <remarks>This method sets a browser cookie to store the specified culture, enabling culture-specific
	/// behavior in Blazor applications. The culture name must be a valid culture identifier.</remarks>
	/// <param name="culture">The culture name to be saved, such as "en-US" or "fr-FR".</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async Task SaveCookieCultureAsync(string culture)
	{
		var mod = await Module();
		await mod.InvokeVoidAsync("setBlazorCultureCookie", culture);
	}

	/// <summary>
	/// Saves the user's preferred culture to local storage.
	/// </summary>
	/// <param name="culture">The culture code to save.</param>
	public async Task SavePreferredCultureAsync(string culture)
		 => await (await Module()).InvokeVoidAsync("savePreferredCulture", culture);

	/// <summary>
	/// Gets the browser's current time zone.
	/// </summary>
	/// <returns>The IANA time zone name, or null if unavailable.</returns>
	public async Task<string?> GetTimeZoneAsync()
		 => await (await Module()).InvokeAsync<string?>("getTimeZone");

	/// <summary>
	/// Applies the specified culture to the current thread and optionally persists it.
	/// </summary>
	/// <param name="culture">The culture code to apply.</param>
	/// <param name="persist">Whether to persist the culture in local storage. Default is true.</param>
	public async Task ApplyCultureAsync(string culture, bool persist = true)
	{
		var ci = new CultureInfo(culture);

		// Nastaví kulturu celému vláknu (Blazor použije pro formaty, resx, atd.)
		CultureInfo.DefaultThreadCurrentCulture = ci;
		CultureInfo.DefaultThreadCurrentUICulture = ci;

		if(persist)
			await SavePreferredCultureAsync(culture);
	}

	/// <summary>
	/// Disposes the JavaScript module reference if it has been loaded.
	/// </summary>
	public async ValueTask DisposeAsync()
	{
		if(_mod is not null)
		{
			try { await _mod.DisposeAsync(); } catch { /* ignore */ }
		}
		GC.SuppressFinalize(this);
	}
}