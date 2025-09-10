using Fxf.Blazor.Models.Settings;
using Fxf.Shared.Models;

namespace Fxf.Blazor.Services;

/// <summary>
/// Defines operations for language metadata and localization dictionary management.
/// Provides methods to query supported languages, load and save locale JSON files,
/// and perform helper operations such as RTL checks and discovery of available locales.
/// </summary>
public interface ILanguageService
{
	/// <summary>
	/// Asynchronously retrieves translation dictionaries for all languages that have locale files present.
	/// </summary>
	/// <param name="isFrontend">When true, reads from the client (WebAssembly) locales folder; otherwise from the server locales folder.</param>
	/// <returns>A <see cref="Response{T}"/> containing a <see cref="TranslationTree"/> with translations per language.</returns>
	Task<Response<TranslationTree>> GetAllDictionariesAsync(bool isFrontend = false);

	/// <summary>
	/// Gets the list of all supported languages with their metadata.
	/// </summary>
	/// <returns>A list of <see cref="Language"/> objects.</returns>
	List<Language> GetAllLanguages();

	/// <summary>
	/// Asynchronously loads a language dictionary from a locale JSON file.
	/// </summary>
	/// <param name="code">Two-letter ISO language code (e.g., "en", "fr").</param>
	/// <param name="isFrontend">When true, reads from the client (WebAssembly) locales folder; otherwise from the server locales folder.</param>
	/// <returns>A <see cref="Response{T}"/> with the key/value dictionary on success.</returns>
	Task<Response<Dictionary<string, string>>> GetDictionaryAsync(string code, bool isFrontend = false);

	/// <summary>
	/// Retrieves language metadata by its code.
	/// </summary>
	/// <param name="code">The ISO language code to search for.</param>
	/// <returns>A <see cref="Response{T}"/> containing the language on success; otherwise a failed response.</returns>
	Response<Language>? GetLanguageByCode(string code);

	/// <summary>
	/// Gets the display names of all supported languages, sorted alphabetically.
	/// </summary>
	/// <returns>A list of language display names.</returns>
	List<string> GetLanguageNames();

	/// <summary>
	/// Asynchronously retrieves the last stored translation dictionary from a temporary file.
	/// </summary>
	/// <param name="isFrontend">When true, reads the client translation path; otherwise the server translation path.</param>
	/// <returns>A <see cref="Response{T}"/> with the last stored dictionary when available.</returns>
	Task<Response<Dictionary<string, string>>> GetLastStored(bool isFrontend = false);

	/// <summary>
	/// Discovers languages that have locale JSON files present in the configured directory.
	/// </summary>
	/// <param name="isFrontend">When true, scans the client (WebAssembly) locales folder; otherwise the server locales folder.</param>
	/// <returns>A <see cref="Response{T}"/> with the list of <see cref="Language"/> entries whose files are present.</returns>
	Response<List<Language>> GetRequiredLanguagesAsync(bool isFrontend = false);

	/// <summary>
	/// Resolves language metadata for a collection of ISO codes.
	/// </summary>
	/// <param name="languages">A list of ISO language codes.</param>
	/// <returns>A <see cref="Response{T}"/> with detailed <see cref="Language"/> entries for each provided code.
	/// Unknown codes may be returned as placeholder <see cref="Language"/> objects.</returns>
	Response<List<Language>> GetSelectedLanguagesInfo(List<string> languages);

	/// <summary>
	/// Indicates whether the specified language is written right-to-left (RTL).
	/// </summary>
	/// <param name="code">The ISO language code (case-insensitive).</param>
	/// <returns><see langword="true"/> if the language is RTL; otherwise <see langword="false"/>.</returns>
	bool IsRtl(string code);

	/// <summary>
	/// Asynchronously saves a translation dictionary to a locale JSON file for the specified language code.
	/// </summary>
	/// <param name="code">The two-letter ISO language code.</param>
	/// <param name="data">The dictionary of key/value translations to save.</param>
	/// <param name="isFrontend">When true, saves to the client (WebAssembly) locales folder; otherwise to the server locales folder.</param>
	/// <returns>A <see cref="Response{Boolean}"/> indicating success or failure.</returns>
	Task<Response<bool>> SaveDictionaryAsync(string code, Dictionary<string, string> data, bool isFrontend = false);

	/// <summary>
	/// Asynchronously stores the provided translation dictionary as the last known translation snapshot (temporary file).
	/// </summary>
	/// <param name="data">The dictionary of translations to store.</param>
	/// <param name="isFrontend">When true, stores to the client (WebAssembly) temp location; otherwise to the server temp location.</param>
	/// <returns>A <see cref="Response{Boolean}"/> indicating success or failure.</returns>
	Task<Response<bool>> SaveOldTranslationAsync(Dictionary<string, string> data, bool isFrontend = false);

	/// <summary>
	/// Asynchronously saves all language dictionaries contained in a <see cref="TranslationTree"/>.
	/// </summary>
	/// <param name="translationTree">The translation tree containing all language dictionaries to save.</param>
	/// <param name="isFrontend">When true, saves to the client (WebAssembly) locales folder; otherwise to the server locales folder.</param>
	/// <returns>A <see cref="Response{T}"/> with a per-language success map (key = language code, value = success).</returns>
	Task<Response<Dictionary<string, bool>>> SaveTranslationsAsync(TranslationTree translationTree, bool isFrontend = false);

	/// <summary>
	/// Gets the list of language codes for which translation JSON files are present.
	/// </summary>
	/// <param name="isFrontend">When true, scans the client (WebAssembly) locales folder; otherwise the server locales folder.</param>
	/// <returns>An array of ISO language codes.</returns>
	string[] TranslationsPresented(bool isFrontend = false);
}