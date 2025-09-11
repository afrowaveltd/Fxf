using Fxf.Blazor.Client.Services;
using Fxf.Blazor.Client.StaticClasses;
using Fxf.Shared.Models;
using Microsoft.Extensions.Localization;

namespace Fxf.Blazor.Client.I18n;

/// <summary>
/// Provides JSON-backed string localization for the Blazor client.
/// Loads translation dictionaries on-demand via <see cref="IApiClientService"/> and resolves
/// individual keys, performing a remote translation fallback when a key is missing.
/// </summary>
public class JsonStringLocalizer
{
	private readonly IApiClientService _api;
	private readonly ILocaleService? _locale;
	private const string DefaultCulture = "en";

	/// <summary>
	/// Initializes a new instance of the <see cref="JsonStringLocalizer"/> class.
	/// </summary>
	/// <param name="api">API client used to fetch localization resources.</param>
	//public JsonStringLocalizer(IApiClientService api)
	// {
	//	  _api = api;
	// }

	/// <summary>
	/// Initializes a new instance of the <see cref="JsonStringLocalizer"/> class with culture support.
	/// </summary>
	/// <param name="api">API client used to fetch localization resources.</param>
	/// <param name="locale">Locale service used to determine the preferred culture.</param>
	public JsonStringLocalizer(IApiClientService api, ILocaleService locale)
	{
		_api = api;
		_locale = locale;
	}

	/// <summary>
	/// Gets a localized string for the specified resource name.
	/// </summary>
	/// <param name="name">The resource key.</param>
	public LocalizedString this[string name]
	{
		get
		{
			var value = GetString(name);
			return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
		}
	}

	/// <summary>
	/// Gets a formatted localized string for the specified resource name and formatting arguments.
	/// </summary>
	/// <param name="name">The resource key.</param>
	/// <param name="arguments">Formatting arguments.</param>
	public LocalizedString this[string name, params object[] arguments]
	{
		get
		{
			LocalizedString actualValue = this[name];
			return !actualValue.ResourceNotFound
				  ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
				  : actualValue;
		}
	}

	/// <summary>
	/// Returns all localized strings currently available in the loaded dictionary.
	/// If the dictionary is not yet loaded it will be loaded for the preferred (or default) culture.
	/// </summary>
	/// <param name="includeParentCultures">Ignored (included for API compatibility).</param>
	/// <returns>Enumeration of localized strings.</returns>
	public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
		 => GetAllStringsInternal(includeParentCultures);

	/// <summary>
	/// Ensures the in-memory dictionary is loaded for the preferred (or default) culture.
	/// </summary>
	private void EnsureLocalesLoaded()
	{
		if(LocaleDictionary.Locales is not null)
		{
			Console.WriteLine("Locales are loaded");
			return;
		}

		var culture = GetPreferredCultureSafe();
		LoadDictionary(culture);
		if(LocaleDictionary.Locales is null || LocaleDictionary.Locales.Count == 0)
		{
			// Fallback to default culture if initial culture failed.
			Console.WriteLine($"Falling back to default culture {DefaultCulture}");
			LoadDictionary(DefaultCulture);
		}
	}

	/// <summary>
	/// Gets (or lazily translates) the value for a given key.
	/// </summary>
	/// <param name="key">Translation key.</param>
	/// <returns>Localized value or null if not found.</returns>
	private string? GetString(string key)
	{
		Console.WriteLine($"Localizing {key}");
		if(string.IsNullOrWhiteSpace(key))
		{
			return key;
		}

		EnsureLocalesLoaded();
		Console.WriteLine($"Found {LocaleDictionary.Locales.Count} phrazes");
		if(LocaleDictionary.Locales != null && LocaleDictionary.Locales.TryGetValue(key, out var value))
		{
			return value;
		}
		else
		{
			Console.WriteLine("Locales are empty or null");
		}

		// Attempt on-demand translation (remote) as a last resort.
		var targetCulture = GetPreferredCultureSafe();
		var translation = TranslateSync(key, DefaultCulture, targetCulture);
		return translation?.TranslatedText ?? key;
	}

	/// <summary>
	/// Loads a dictionary for a specified culture synchronously.
	/// </summary>
	/// <param name="code">Culture code.</param>
	private void LoadDictionary(string code)
	{
		try
		{
			Console.WriteLine($"Loading locales for {code}");
			LocaleDictionary.Locales = [];
			LocaleDictionary.Locales = _api.GetClientDictionary(code).GetAwaiter().GetResult();
			Console.WriteLine($"Locales for {code} loaded successfully");
		}
		catch(Exception ex)
		{
			Console.WriteLine($"Failed loading locales for {code}: {ex.Message}");
		}
	}

	/// <summary>
	/// Performs a synchronous translation call (blocking) using the API client.
	/// </summary>
	/// <param name="text">Text to translate.</param>
	/// <param name="from">Source language code (or 'en' default).</param>
	/// <param name="to">Target language code.</param>
	/// <returns>Translation result; may be empty.</returns>
	private TranslateResult TranslateSync(string text, string from, string to)
	{
		try
		{
			return _api
				.Translate(text, from, to)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}
		catch(Exception ex)
		{
			Console.WriteLine($"Translation failed: {ex.Message}");
			return new TranslateResult { TranslatedText = text };
		}
	}

	/// <summary>
	/// Gets the preferred culture from the locale service or falls back to the default culture.
	/// </summary>
	private string GetPreferredCultureSafe()
	{
		if(_locale is null)
		{
			return DefaultCulture;
		}
		try
		{
			return _locale.GetPreferredCultureAsync().GetAwaiter().GetResult() ?? DefaultCulture;
		}
		catch
		{
			return DefaultCulture;
		}
	}

	private IEnumerable<LocalizedString> GetAllStringsInternal(bool includeParentCultures)
	{
		EnsureLocalesLoaded();
		if(LocaleDictionary.Locales is null) yield break;
		foreach(var kvp in LocaleDictionary.Locales)
			yield return new LocalizedString(kvp.Key, kvp.Value, resourceNotFound: false);
	}
}