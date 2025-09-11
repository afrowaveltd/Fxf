using Fxf.Shared.Models;

namespace Fxf.Blazor.Client.Services;

/// <summary>
/// Defines methods for interacting with the backend localization and translation API.
/// Provides access to locale dictionaries, language metadata, and translation operations.
/// </summary>
public interface IApiClientService
{
	/// <summary>
	/// Gets the underlying <see cref="HttpClient"/> instance for advanced scenarios.
	/// </summary>
	HttpClient Client { get; }

	/// <summary>
	/// Gets all locale translations (tree) for either client or server side.
	/// </summary>
	/// <param name="isClient">True for client locales, false for server locales.</param>
	/// <returns>A translation tree (possibly empty).</returns>
	Task<TranslationTree> GetAllLocales(bool isClient);

	/// <summary>
	/// Gets the client-side dictionary for a language code.
	/// </summary>
	/// <param name="code">Language code.</param>
	/// <returns>The dictionary or an empty dictionary if unavailable.</returns>
	Task<Dictionary<string, string>> GetClientDictionary(string code);

	/// <summary>
	/// Gets the default (English) client dictionary.
	/// </summary>
	/// <returns>The dictionary or an empty dictionary if unavailable.</returns>
	Task<Dictionary<string, string>> GetDefaultDictionary();

	/// <summary>
	/// Retrieves the complete supported language list from the server.
	/// </summary>
	/// <returns>List of <see cref="Language"/> objects (possibly empty).</returns>
	Task<List<Language>> GetFullLanguageList();

	/// <summary>
	/// Gets detailed language information by code.
	/// </summary>
	/// <param name="code">Language code (must be at least 2 chars).</param>
	/// <returns>The language info or a new empty instance.</returns>
	Task<Language> GetLanguageInfo(string code);

	/// <summary>
	/// Retrieves a list of languages supported by the Libre service.
	/// </summary>
	/// <remarks>The returned list may vary depending on the configuration or version of the Libre service.
	/// Ensure that the service is properly configured and accessible before calling this method.</remarks>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see cref="Language"/>
	/// objects representing the supported languages.</returns>
	Task<List<Language>> GetLibreLanguages();

	/// <summary>
	/// Retrieves the list of language codes supported by the Libre service.
	/// </summary>
	/// <remarks>The returned language codes follow standard ISO 639-1 or similar conventions, depending on the
	/// service's implementation.</remarks>
	/// <returns>An array of strings containing the language codes. The array will be empty if no languages are supported.</returns>
	Task<string[]> GetLibreLanguagesCodes();

	/// <summary>
	/// Gets an older translation dictionary snapshot (for comparison or rollback).
	/// </summary>
	/// <param name="isClient">True to retrieve client dictionary, false for server.</param>
	/// <returns>A dictionary (empty if unavailable).</returns>
	Task<Dictionary<string, string>> GetOldTranslation(bool isClient);

	/// <summary>
	/// Gets the server-side dictionary for a language code.
	/// </summary>
	/// <param name="code">Language code.</param>
	/// <returns>The dictionary or an empty dictionary if unavailable.</returns>
	Task<Dictionary<string, string>> GetServerDictionary(string code);

	/// <summary>
	/// Loads the default (English) client dictionary.
	/// </summary>
	Task LoadDefaultDictionary();

	/// <summary>
	/// Loads a locale dictionary for the specified language code and stores it in <see cref="LocaleDictionary"/>.
	/// Falls back to English if the requested code is unavailable.
	/// </summary>
	/// <param name="code">Two-letter language code (e.g. "en"). Defaults to "en" if null.</param>
	/// <param name="isClient">Whether to load the client dictionary (true) or server dictionary (false).</param>
	Task LoadDictionary(string code, bool isClient = true);

	/// <summary>
	/// Saves (bulk) multiple locale dictionaries in a translation tree.
	/// </summary>
	/// <param name="translations">Translation tree to store.</param>
	/// <param name="isClient">True for client dictionaries, false for server dictionaries.</param>
	Task SaveBulkLocales(TranslationTree translations, bool isClient);

	/// <summary>
	/// Persists a single locale dictionary to the server.
	/// </summary>
	/// <param name="code">Two-letter language code.</param>
	/// <param name="dictionary">Key/value translation pairs.</param>
	Task SaveLocale(string code, Dictionary<string, string> dictionary);

	/// <summary>
	/// Triggers storing the current dictionary state as the "old" snapshot on the server.
	/// </summary>
	/// <param name="isClient">True for client dictionary snapshot, false for server.</param>
	Task SaveOldTranslation(bool isClient);

	/// <summary>
	/// Translates a text string from one language to another (or auto-detected source).
	/// </summary>
	/// <param name="query">Text to translate.</param>
	/// <param name="from">Source language code ("auto" if unknown).</param>
	/// <param name="to">Target language code (defaults to "en" if invalid).</param>
	/// <returns>The translation result (empty if failed or invalid request).</returns>
	Task<TranslateResult> Translate(string query, string from, string to);
}