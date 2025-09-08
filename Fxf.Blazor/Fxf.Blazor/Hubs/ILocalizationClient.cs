namespace Fxf.Blazor.Hubs;

/// <summary>
/// SignalR client contract for localization-related notifications and requests.
/// Implement this interface on the client to receive updates about locales,
/// language metadata, dictionaries, and translation operations.
/// </summary>
public interface ILocalizationClient
{
    /// <summary>
    /// Notifies the client that the active locale has changed.
    /// </summary>
    /// <param name="locale">The new locale identifier (e.g., two-letter ISO code like "en" or a culture name like "en-US").</param>
    /// <returns>A task that completes when the notification has been processed on the client.</returns>
    Task OnLocaleChanged(string locale);

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
    /// <returns>A task that completes when the client has handled the request.</returns>
    Task OnLocalesRequested(bool isClient = true);

    /// <summary>
    /// Requests the client to (re)load the list of supported languages.
    /// </summary>
    /// <returns>A task that completes when the client has handled the request.</returns>
    Task OnLanguagesRequested();

    /// <summary>
    /// Requests the client to load/display information for a specific language.
    /// </summary>
    /// <param name="code">The ISO language code to query (e.g., "en", "cs").</param>
    /// <returns>A task that completes when the client has handled the request.</returns>
    Task OnLanguageInfoRequested(string code);

    /// <summary>
    /// Requests the client to load the translation dictionary for a language and context.
    /// </summary>
    /// <param name="code">The ISO language code (e.g., "en", "cs").</param>
    /// <param name="isClient">True to target client (WebAssembly) dictionary; false for server.</param>
    /// <returns>A task that completes when the client has handled the request.</returns>
    Task OnDictionaryByCodeRequested(string code, bool isClient = true);

    /// <summary>
    /// Requests the client to resolve or display a localized resource for the given key.
    /// </summary>
    /// <param name="code">The localization key to resolve.</param>
    /// <returns>A task that completes when the client has handled the request.</returns>
    Task OnLocalizationRequested(string code);

    /// <summary>
    /// Requests the client to handle a translation task for the given query and language pair.
    /// </summary>
    /// <param name="query">The text to translate.</param>
    /// <param name="from">The source language ISO code.</param>
    /// <param name="to">The target language ISO code.</param>
    /// <returns>A task that completes when the client has handled the request.</returns>
    Task OnTranslationRequested(string query, string from, string to);

    /// <summary>
    /// Requests the client to save a locale dictionary for the specified language and context.
    /// </summary>
    /// <param name="code">The ISO language code of the dictionary to save.</param>
    /// <param name="isClient">True to save to client (WebAssembly) locales; false to server locales.</param>
    /// <returns>A task that completes when the client has initiated/acknowledged the save operation.</returns>
    Task OnLocaleSaveRequested(string code, bool isClient);

    /// <summary>
    /// Notifies the client that a bulk save operation has completed.
    /// </summary>
    /// <param name="success">Overall success indicator across all languages.</param>
    /// <param name="results">Per-language save results (key = language code, value = success).</param>
    /// <returns>A task that completes when the client has processed the results.</returns>
    Task OnBulkSaveRequested(bool success, Dictionary<string, bool> results);

    /// <summary>
    /// Provides the client with previously stored ("old") locale data for the given context.
    /// </summary>
    /// <param name="isClient">True when the data is for client (WebAssembly) context; false for server context.</param>
    /// <param name="data">The key/value dictionary representing the old locale content.</param>
    /// <returns>A task that completes when the client has handled the provided data.</returns>
    Task OnOldLocaleRequested(bool isClient, Dictionary<string, string> data);

    /// <summary>
    /// Notifies the client that saving the old locale data has completed.
    /// </summary>
    /// <param name="isClient">True if the operation targeted client (WebAssembly) data; false for server.</param>
    /// <param name="success">True if the save succeeded; otherwise false.</param>
    /// <returns>A task that completes when the client has acknowledged the result.</returns>
    Task OnOldLocaleSaveRequested(bool isClient, bool success);
}