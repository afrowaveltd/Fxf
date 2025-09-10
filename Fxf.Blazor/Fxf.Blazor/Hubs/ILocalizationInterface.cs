using Fxf.Blazor.Models.Settings;
using Fxf.Shared.Models;

namespace Fxf.Blazor.Hubs;

/// <summary>
/// SignalR client contract for localization-related notifications and requests.
/// Implement this interface on the client to receive updates about locales,
/// language metadata, dictionaries, and translation operations.
/// </summary>
public interface ILocalizationInterface
{
	/// <summary>
	/// Notifies the client that the locale has changed.
	/// </summary>
	/// <param name="locale">Code of the new locale.</param>
	/// <param name="isClient">Decides wharever it is a client or server localization.</param>
	Task OnLocaleChanged(string locale, bool isClient);

	/// <summary>
	/// Notifies the client that localization dictionaries have been refreshed for a given scope.
	/// </summary>
	/// <param name="scope">The scope that was refreshed. Expected values: "client" or "server".</param>
	/// <returns>A task that completes when the client finishes handling the refresh notification.</returns>
	Task OnDictionariesRefreshed(string scope);// "client" | "server"

	/// <summary>
	/// Requests the client to (re)load available locale files for the specified context.
	/// </summary>
	/// <param name="isClient">True to target client (WebAssembly) locales; false for server locales. Defaults to true.</param>
	/// <param name="dictionaries">The translation dictionaries data to provide to the client.</param>
	/// <returns>A task that completes when the client has handled the request.</returns>
	Task OnLocalesRequested(Response<TranslationTree> dictionaries, bool isClient = true);

	/// <summary>
	/// Requests the client to (re)load the list of supported languages.
	/// </summary>
	/// <returns>A task that completes when the client has handled the request.</returns>
	Task OnLanguagesRequested(List<Language> languages);

	/// <summary>
	/// Requests the client to load/display information for a specific language.
	/// </summary>
	/// <param name="language">The Language object.</param>
	/// <returns>A task that completes when the client has handled the request.</returns>
	Task OnLanguageInfoRequested(Language language);

	/// <summary>
	/// Requests the client to load the translation dictionary for a language and context.
	/// </summary>
	/// <param name="code">The ISO language code (e.g., "en", "cs").</param>
	/// <param name="isClient">True to target client (WebAssembly) dictionary; false for server.</param>
	/// <param name="dictionary">The translation dictionary data to provide to the client.</param>
	/// <returns>A task that completes when the client has handled the request.</returns>
	Task OnDictionaryByCodeRequested(string code, Response<Dictionary<string, string>> dictionary, bool isClient = true);

	/// <summary>
	/// Handles a localization request by processing the specified query and providing a translation result.
	/// </summary>
	/// <param name="query">The text to be localized. Cannot be null or empty.</param>
	/// <param name="result">A response object that will be populated with the translation result.  The caller is responsible for handling the
	/// response's success or failure state.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task OnLocalizationRequested(string query, Response<TranslateResult> result);

	/// <summary>
	/// Requests the client to handle a translation task for the given query and language pair.
	/// </summary>
	/// <param name="query">The text to translate.</param>
	/// <param name="from">The source language ISO code.</param>
	/// <param name="to">The target language ISO code.</param>
	/// <param name="result">A response object that will be populated with the translation result. The caller is responsible for handling the response's success or failure state.</param>
	/// <returns>A task that completes when the client has handled the request.</returns>
	Task OnTranslationRequested(string query, string from, string to, Response<TranslateResult> result);

	/// <summary>
	/// Requests the client to save a locale dictionary for the specified language and context.
	/// </summary>
	/// <param name="code">The ISO language code of the dictionary to save.</param>
	/// <param name="isClient">True to save to client (WebAssembly) locales; false to server locales.</param>
	/// <param name="success">True if the save operation succeeded; otherwise false.</param>
	/// <returns>A task that completes when the client has initiated/acknowledged the save operation.</returns>
	Task OnLocaleSaveRequested(string code, bool success, bool isClient);

	/// <summary>
	/// Notifies the client that a bulk save operation has completed.
	/// </summary>
	/// <param name="results">Per-language save results (key = language code, value = success).</param>
	/// <returns>A task that completes when the client has processed the results.</returns>
	Task OnBulkSaveRequested(Dictionary<string, bool> results);

	/// <summary>
	/// Provides the client with previously stored ("old") locale data for the given context.
	/// </summary>
	/// <param name="isClient">True when the data is for client (WebAssembly) context; false for server context.</param>
	/// <param name="data">The key/value dictionary representing the old locale content.</param>
	/// <returns>A task that completes when the client has handled the provided data.</returns>
	Task OnOldLocaleRequested(Dictionary<string, string> data, bool isClient = true);

	/// <summary>
	/// Notifies the client that saving the old locale data has completed.
	/// </summary>
	/// <param name="isClient">True if the operation targeted client (WebAssembly) data; false for server.</param>
	/// <param name="success">True if the save succeeded; otherwise false.</param>
	/// <returns>A task that completes when the client has acknowledged the result.</returns>
	Task OnOldLocaleSaveRequested(bool success, bool isClient);

	/// <summary>
	/// Handles an error by processing the provided error message asynchronously.
	/// </summary>
	/// <param name="name">Name of the error (header)</param>
	/// <param name="message">The error message to be processed. Cannot be null or empty.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task OnError(string message, string? name = "Error");

	/// <summary>
	/// Detects and processes the supported locales for the application.
	/// </summary>
	/// <remarks>This method is typically used to identify the locales that are supported by the application and
	/// perform any necessary initialization or configuration based on the detected locales.</remarks>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task OnDetectLocales(UiLocale localization);
}