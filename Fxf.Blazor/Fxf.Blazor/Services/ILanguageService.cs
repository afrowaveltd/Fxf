using Fxf.Blazor.Models.LibreTranslate;
using Fxf.Blazor.Models.Settings;
using Fxf.Shared.Models;

namespace Fxf.Blazor.Services;

/// <summary>
/// Defines operations for working with application languages and translation dictionaries,
/// including discovery of available languages, loading and saving locale files, and utility helpers.
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
	/// Gets a list of all supported languages with metadata.
	/// </summary>
	/// <returns>A list of <see cref="Language"/> objects.</returns>
	List<Language> GetAllLanguages();

	/// <summary>
	/// Asynchronously loads a translation dictionary for the specified two-letter language code.
	/// </summary>
	/// <param name="code">The ISO 2-letter language code (e.g., "en", "cs").</param>
	/// <param name="isFrontend">When true, reads from the client (WebAssembly) locales folder; otherwise from the server locales folder.</param>
	/// <returns>A <see cref="Response{T}"/> containing the key-value dictionary for the language.</returns>
	Task<Response<Dictionary<string, string>>> GetDictionaryAsync(string code, bool isFrontend = false);

	/// <summary>
	/// Gets language metadata by its code.
	/// </summary>
	/// <param name="code">The ISO language code to look up.</param>
	/// <returns>A <see cref="Response{T}"/> containing the matching <see cref="Language"/> on success; otherwise a failed response.</returns>
	Response<Language>? GetLanguageByCode(string code);

	/// <summary>
	/// Gets the display names of all supported languages, sorted alphabetically.
	/// </summary>
	/// <returns>A list of language names.</returns>
	List<string> GetLanguageNames();

	/// <summary>
	/// Asynchronously retrieves the last stored translation dictionary from a temporary storage location.
	/// </summary>
	/// <param name="isFrontend">When true, reads the client (WebAssembly) temp file; otherwise the server temp file.</param>
	/// <returns>A <see cref="Response{T}"/> with the last stored dictionary, if available.</returns>
	Task<Response<Dictionary<string, string>>> GetLastStored(bool isFrontend = false);

	/// <summary>
	/// Determines which languages are required based on the presence of locale files in the configured directory.
	/// </summary>
	/// <param name="isFrontend">When true, inspects the client (WebAssembly) locales folder; otherwise the server locales folder.</param>
	/// <returns>A <see cref="Response{T}"/> containing the list of required <see cref="Language"/> items.</returns>
	Response<List<Language>> GetRequiredLanguagesAsync(bool isFrontend = false);

	/// <summary>
	/// Resolves language metadata for a provided list of language codes.
	/// </summary>
	/// <param name="languages">The collection of language codes to resolve.</param>
	/// <returns>A <see cref="Response{T}"/> with detailed <see cref="Language"/> entries for each provided code.</returns>
	Response<List<Language>> GetSelectedLanguagesInfo(List<string> languages);

	/// <summary>
	/// Indicates whether the specified language is written right-to-left (RTL).
	/// </summary>
	/// <param name="code">The ISO language code.</param>
	/// <returns><see langword="true"/> if the language is RTL; otherwise <see langword="false"/>.</returns>
	bool IsRtl(string code);

	/// <summary>
	/// Asynchronously saves a translation dictionary for the specified language to a JSON locale file.
	/// </summary>
	/// <param name="code">The ISO 2-letter language code.</param>
	/// <param name="data">The key-value translation dictionary to persist.</param>
	/// <param name="isFrontend">When true, saves to the client (WebAssembly) locales folder; otherwise to the server locales folder.</param>
	/// <returns>A <see cref="Response{T}"/> indicating whether the operation succeeded.</returns>
	Task<Response<bool>> SaveDictionaryAsync(string code, Dictionary<string, string> data, bool isFrontend = false);

	/// <summary>
	/// Asynchronously stores the provided translation dictionary as the last known (temporary) translation snapshot.
	/// </summary>
	/// <param name="data">The translation dictionary to store.</param>
	/// <param name="isFrontend">When true, stores to the client (WebAssembly) temp location; otherwise to the server temp location.</param>
	/// <returns>A <see cref="Response{T}"/> indicating success or failure.</returns>
	Task<Response<bool>> SaveOldTranslationAsync(Dictionary<string, string> data, bool isFrontend = false);

	/// <summary>
	/// Asynchronously saves all language dictionaries contained in a <see cref="TranslationTree"/>.
	/// </summary>
	/// <param name="translationTree">The translation tree containing all language dictionaries to save.</param>
	/// <param name="isFrontend">When true, saves to the client (WebAssembly) locales folder; otherwise to the server locales folder.</param>
	/// <returns>A <see cref="Response{T}"/> with a per-language success map where the key is the language code and the value indicates success.</returns>
	Task<Response<Dictionary<string, bool>>> SaveTranslationsAsync(TranslationTree translationTree, bool isFrontend = false);

	/// <summary>
	/// Gets the list of language codes for which translation JSON files are present.
	/// </summary>
	/// <param name="isFrontend">When true, scans the client (WebAssembly) locales folder; otherwise the server locales folder.</param>
	/// <returns>An array of ISO language codes.</returns>
	string[] TranslationsPresented(bool isFrontend = false);
}