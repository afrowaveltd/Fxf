using Fxf.Blazor.Client.StaticClasses;
using Fxf.Shared.Models;
using System.Net.Http.Json;

namespace Fxf.Blazor.Client.Services;

/// <summary>
/// Provides strongly-typed access to the backend localization / translation API.
/// Handles loading/storing locale dictionaries, language metadata and performing text translations.
/// </summary>
public class ApiClientService : IApiClientService
{
	private readonly HttpClient _client;
	private readonly string getLocalesUri = "/localization/get_locales";                    // done
	private readonly string getSupportedLanguagesUri = "/localization/get_languages";       // done
	private readonly string getLanguageInfoUri = "/localization/get_language_info";         // done
	private readonly string getLanguageByCodeUri = "/localization/get_language_by_code";    // done
	private readonly string localizeUri = "/localization/localize";                         // done
	private readonly string saveLocaleUri = "/localization/save_locale";                    // done
	private readonly string saveBulkUri = "/localization/save_locale_bulk";                 // done
	private readonly string getOldUri = "/localization/get_old";                            // done
	private readonly string saveOldUri = "/localization/save_old";                          // done

	/// <summary>
	/// Creates a new <see cref="ApiClientService"/> with a configured <see cref="HttpClient"/>.
	/// Ensures the base address ends with "/api/".
	/// </summary>
	/// <param name="client">The underlying <see cref="HttpClient"/> (injected).</param>
	public ApiClientService()
	{
		_client = new HttpClient();
		_client.BaseAddress = new Uri(_client.BaseAddress!.AbsoluteUri.TrimEnd('/') + "/api/");
	}

	/// <summary>
	/// Gets the underlying <see cref="HttpClient"/> instance for advanced scenarios.
	/// </summary>
	public HttpClient Client => _client;

	/// <summary>
	/// Loads a locale dictionary for the specified language code and stores it in <see cref="LocaleDictionary"/>.
	/// Falls back to English if the requested code is unavailable.
	/// </summary>
	/// <param name="code">Two-letter language code (e.g. "en"). Defaults to "en" if null.</param>
	/// <param name="isClient">Whether to load the client dictionary (true) or server dictionary (false).</param>
	public async Task LoadDictionary(string code, bool isClient = true)
	{
		code ??= "en";

		string url = getLanguageByCodeUri + "/" + code + "/" + isClient;

		var response = await _client.GetAsync(url).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			var actual_dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>().ConfigureAwait(false);
			if(actual_dictionary != null)
			{
				LocaleDictionary.Locales = actual_dictionary;
				return;
			}

			if(code == "en")
			{
				return; // nothing more we can do
			}

			await LoadDefaultDictionary().ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Loads the default (English) client dictionary.
	/// </summary>
	public async Task LoadDefaultDictionary() => await LoadDictionary("en", true).ConfigureAwait(false);

	/// <summary>
	/// Gets the server-side dictionary for a language code.
	/// </summary>
	/// <param name="code">Language code.</param>
	/// <returns>The dictionary or an empty dictionary if unavailable.</returns>
	public async Task<Dictionary<string, string>> GetServerDictionary(string code) => await GetDictionary(code, false).ConfigureAwait(false);

	/// <summary>
	/// Gets the client-side dictionary for a language code.
	/// </summary>
	/// <param name="code">Language code.</param>
	/// <returns>The dictionary or an empty dictionary if unavailable.</returns>
	public async Task<Dictionary<string, string>> GetClientDictionary(string code) => await GetDictionary(code, true).ConfigureAwait(false);

	/// <summary>
	/// Gets the default (English) client dictionary.
	/// </summary>
	public async Task<Dictionary<string, string>> GetDefaultDictionary() => await GetDictionary("en", true).ConfigureAwait(false);

	/// <summary>
	/// Gets all locale translations (tree) for either client or server side.
	/// </summary>
	/// <param name="isClient">True for client locales, false for server locales.</param>
	/// <returns>A translation tree (possibly empty).</returns>
	public async Task<TranslationTree> GetAllLocales(bool isClient)
	{
		string url = getLocalesUri + "/" + isClient;
		var response = await _client.GetAsync(url).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			var actual_tree = await response.Content.ReadFromJsonAsync<Response<TranslationTree>>().ConfigureAwait(false);
			if(actual_tree != null)
			{
				return actual_tree.Data ?? new();
			}
		}
		return new();
	}

	/// <summary>
	/// Retrieves the complete supported language list from the server.
	/// </summary>
	/// <returns>List of <see cref="Language"/> objects (possibly empty).</returns>
	public async Task<List<Language>> GetFullLanguageList()
	{
		string url = getSupportedLanguagesUri;
		var response = await _client.GetAsync(url).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<List<Language>>().ConfigureAwait(false) ?? [];
		}
		return [];
	}

	/// <summary>
	/// Gets detailed language information by code.
	/// </summary>
	/// <param name="code">Language code (must be at least 2 chars).</param>
	/// <returns>The language info or a new empty instance.</returns>
	public async Task<Language> GetLanguageInfo(string code)
	{
		if(code == null || code.Length < 2)
		{
			return new();
		}
		string url = getLanguageInfoUri + "/" + code;
		var response = await _client.GetAsync(url).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<Language>().ConfigureAwait(false) ?? new();
		}
		return new();
	}

	/// <summary>
	/// Translates a text string from one language to another (or auto-detected source).
	/// </summary>
	/// <param name="query">Text to translate.</param>
	/// <param name="from">Source language code ("auto" if unknown).</param>
	/// <param name="to">Target language code (defaults to "en" if invalid).</param>
	/// <returns>The translation result (empty if failed or invalid request).</returns>
	public async Task<TranslateResult> Translate(string query, string from, string to)
	{
		if(string.IsNullOrWhiteSpace(query))
		{
			return new();
		}
		if(to == null || to.Length != 2)
		{
			to = "en";
		}
		if(from == null || from.Length != 2)
		{
			from = "auto";
		}
		if(from == to)
		{
			return new() { TranslatedText = query };
		}

		string safeQuery = Uri.EscapeDataString(query);
		string url = $"{localizeUri}/{safeQuery}/{from}/{to}";
		var response = await _client.GetAsync(url).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<TranslateResult>().ConfigureAwait(false);
			return result ?? new();
		}
		return new();
	}

	/// <summary>
	/// Persists a single locale dictionary to the server.
	/// </summary>
	/// <param name="code">Two-letter language code.</param>
	/// <param name="dictionary">Key/value translation pairs.</param>
	public async Task SaveLocale(string code, Dictionary<string, string> dictionary)
	{
		if(code == null || dictionary == null || code.Length != 2 || dictionary.Count == 0)
		{
			return;
		}
		string url = saveLocaleUri + "/" + code;

		var response = await _client.PostAsJsonAsync(url, dictionary).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			Console.WriteLine("Stored successfully");
			return;
		}
		Console.WriteLine("Error storing locale");
	}

	/// <summary>
	/// Saves (bulk) multiple locale dictionaries in a translation tree.
	/// </summary>
	/// <param name="translations">Translation tree to store.</param>
	/// <param name="isClient">True for client dictionaries, false for server dictionaries.</param>
	public async Task SaveBulkLocales(TranslationTree translations, bool isClient)
	{
		if(translations == null)
		{
			return;
		}
		string url = saveBulkUri + "/" + isClient;
		var response = await _client.PostAsJsonAsync(url, translations).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			Console.WriteLine("Stored successfully");
			return;
		}
		Console.WriteLine("Error storing locale");
	}

	/// <summary>
	/// Gets an older translation dictionary snapshot (for comparison or rollback).
	/// </summary>
	/// <param name="isClient">True to retrieve client dictionary, false for server.</param>
	/// <returns>A dictionary (empty if unavailable).</returns>
	public async Task<Dictionary<string, string>> GetOldTranslation(bool isClient)
	{
		string url = getOldUri + "/" + isClient;
		var response = await _client.GetAsync(url).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>().ConfigureAwait(false);
			return result ?? [];
		}
		return [];
	}

	/// <summary>
	/// Triggers storing the current dictionary state as the "old" snapshot on the server.
	/// </summary>
	/// <param name="isClient">True for client dictionary snapshot, false for server.</param>
	public async Task SaveOldTranslation(bool isClient)
	{
		string url = saveOldUri + "/" + isClient;
		var response = await _client.GetAsync(url).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			Console.WriteLine("Old translation saved successfully");
		}
		else
		{
			Console.WriteLine("Error saving old translation");
		}
	}

	/// <summary>
	/// Internal helper to load a dictionary for a specific code and scope.
	/// Falls back to default dictionary if initial request fails.
	/// </summary>
	/// <param name="code">Language code.</param>
	/// <param name="isClient">Client or server scope.</param>
	/// <returns>The dictionary or empty collection.</returns>
	private async Task<Dictionary<string, string>> GetDictionary(string code, bool isClient)
	{
		string url = getLanguageByCodeUri + "/" + code + "/" + isClient;

		var response = await _client.GetAsync(url).ConfigureAwait(false);
		if(response.IsSuccessStatusCode)
		{
			var actual_dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
			if(actual_dictionary != null)
			{
				return actual_dictionary;
			}

			if(code == "en")
			{
				return [];
			}

			// Fallback to default dictionary and return it
			return await GetDefaultDictionary().ConfigureAwait(false);
		}
		return [];
	}
}