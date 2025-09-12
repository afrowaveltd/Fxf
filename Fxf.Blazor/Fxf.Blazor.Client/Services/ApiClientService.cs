using Fxf.Blazor.Client.StaticClasses;
using Fxf.Shared.Models;
using System.Net.Http.Json;

namespace Fxf.Blazor.Client.Services;

/// <summary>
/// Provides strongly-typed access to the backend localization / translation API.
/// Handles loading/storing locale dictionaries, language metadata and performing text translations.
/// </summary>
/// <remarks>
/// Creates a new <see cref="ApiClientService"/> with a configured <see cref="HttpClient"/>.
/// Ensures the base address ends with "/api/".
/// </remarks>
/// <param name="client">The underlying <see cref="HttpClient"/> (injected).</param>
public class ApiClientService(HttpClient client) : IApiClientService
{
	private readonly HttpClient _client = client;

	// Base route segment for localization controller
	private const string Base = "api/Localization"; // Matches [Route("api/[controller]")]

	private readonly string getLocalesUri = $"{Base}/get_locales";                    // done
	private readonly string getSupportedLanguagesUri = $"{Base}/get_languages";       // done
	private readonly string getLanguageInfoUri = $"{Base}/get_language_info";         // done
	private readonly string getLanguageByCodeUri = $"{Base}/get_language_by_code";    // done
	private readonly string localizeUri = $"{Base}/localize";                         // done
	private readonly string saveLocaleUri = $"{Base}/save_locale";                    // done
	private readonly string saveBulkUri = $"{Base}/save_locale_bulk";                 // done
	private readonly string getOldUri = $"{Base}/get_old";                            // done
	private readonly string saveOldUri = $"{Base}/save_old";                          // done
	private readonly string getLibreLanguagesCodesListUri = $"{Base}/get_libre_languages_code_list";
	private readonly string getLibreLanguagesFullListUri = $"{Base}/get_libre_languages_full_list";

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

		string url = $"{getLanguageByCodeUri}/{code}/{isClient}";

		var response = await _client.GetAsync(url);
		if(response.IsSuccessStatusCode)
		{
			var actual_dictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
			if(actual_dictionary != null)
			{
				LocaleDictionary.Locales = actual_dictionary;
				Console.WriteLine($"Found: {LocaleDictionary.Locales.Count} translations");
				return;
			}

			if(code == "en")
			{
				return; // nothing more we can do
			}
			Console.WriteLine($"Locale not found");
			await LoadDefaultDictionary();
		}
	}

	/// <summary>
	/// Loads the default (English) client dictionary.
	/// </summary>
	public async Task LoadDefaultDictionary() => await LoadDictionary("en", true);

	/// <summary>
	/// Gets the server-side dictionary for a language code.
	/// </summary>
	/// <param name="code">Language code.</param>
	/// <returns>The dictionary or an empty dictionary if unavailable.</returns>
	public async Task<Dictionary<string, string>> GetServerDictionary(string code) => await GetDictionary(code, false);

	/// <summary>
	/// Gets the client-side dictionary for a language code.
	/// </summary>
	/// <param name="code">Language code.</param>
	/// <returns>The dictionary or an empty dictionary if unavailable.</returns>
	public async Task<Dictionary<string, string>> GetClientDictionary(string code) => await GetDictionary(code, true);

	/// <summary>
	/// Gets the default (English) client dictionary.
	/// </summary>
	public async Task<Dictionary<string, string>> GetDefaultDictionary() => await GetDictionary("en", true);

	/// <summary>
	/// Gets all locale translations (tree) for either client or server side.
	/// </summary>
	/// <param name="isClient">True for client locales, false for server locales.</param>
	/// <returns>A translation tree (possibly empty).</returns>
	public async Task<TranslationTree> GetAllLocales(bool isClient)
	{
		string url = $"{getLocalesUri}/{isClient}";
		var response = await _client.GetAsync(url);
		if(response.IsSuccessStatusCode)
		{
			// Kontroler vrací přímo TranslationTree, ne Response<TranslationTree>
			var tree = await response.Content.ReadFromJsonAsync<TranslationTree>();
			return tree ?? new();
		}
		return new();
	}

	private static async Task<(bool flowControl, TranslationTree value)> NewMethod(HttpResponseMessage response)
	{
		var actual_tree = await response.Content.ReadFromJsonAsync<Response<TranslationTree>>();
		if(actual_tree != null)
		{
			return (flowControl: false, value: actual_tree.Data ?? new());
		}

		return (flowControl: true, value: null);
	}

	/// <summary>
	/// Retrieves the complete supported language list from the server.
	/// </summary>
	/// <returns>List of <see cref="Language"/> objects (possibly empty).</returns>
	public async Task<List<Language>> GetFullLanguageList()
	{
		string url = getSupportedLanguagesUri;
		var response = await _client.GetAsync(url);
		if(response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<List<Language>>() ?? [];
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
		string url = $"{getLanguageInfoUri}/{code}";
		var response = await _client.GetAsync(url);
		if(response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<Language>() ?? new();
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
		string url = $"{localizeUri}/{safeQuery}/{to}/{from}"; // Note: controller expects localize/{query}/{target?}/{source?}
		var response = await _client.GetAsync(url);
		if(response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<TranslateResult>();
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
		string url = $"{saveLocaleUri}/{code}";

		var response = await _client.PostAsJsonAsync(url, dictionary);
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
		string url = $"{saveBulkUri}/{isClient}";
		var response = await _client.PostAsJsonAsync(url, translations);
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
		string url = $"{getOldUri}/{isClient}";
		var response = await _client.GetAsync(url);
		if(response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
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
		string url = $"{saveOldUri}/{isClient}";
		var response = await _client.GetAsync(url);
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
	/// Retrieves a list of language codes supported by the Libre service.
	/// </summary>
	public async Task<string[]> GetLibreLanguagesCodes()
	{
		string url = getLibreLanguagesCodesListUri;
		var response = await _client.GetAsync(url);
		if(response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<string[]>();
			return result ?? [];
		}
		return [];
	}

	/// <summary>
	/// Retrieves the full list of available languages from the Libre service.
	/// </summary>
	public async Task<List<Language>> GetLibreLanguages()
	{
		string url = getLibreLanguagesFullListUri;
		var response = await _client.GetAsync(url);
		if(response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<List<Language>>();
			return result ?? [];
		}
		return [];
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
		string url = $"{getLanguageByCodeUri}/{code}/{isClient}";

		var response = await _client.GetAsync(url);
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
			return await GetDefaultDictionary();
		}
		return [];
	}
}