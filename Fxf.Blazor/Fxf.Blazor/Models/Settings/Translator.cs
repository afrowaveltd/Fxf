namespace Fxf.Blazor.Models.Settings;

/// <summary>
/// Represents configuration settings for the translation service.
/// </summary>
public class Translator
{
	/// <summary>
	/// Gets or sets the API key used for authenticating requests to the service.
	/// </summary>
	public string ApiKey { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the endpoint URL used to detect the language from provided text.
	/// </summary>
	public string DetectLanguageEndpoint { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the host name or IP address of the server.
	/// </summary>
	public string Host { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the endpoint URL used to retrieve supported languages.
	/// </summary>
	public string LanguagesEndpoint { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets a value indicating whether a key is required for the operation.
	/// </summary>
	public bool NeedsKey { get; set; } = false;

	/// <summary>
	/// Gets or sets the number of retry attempts to perform when an operation fails. Set this value
	/// to 0 to disable retries.
	/// </summary>
	public int RetriesOnFailure { get; set; } = 10;

	/// <summary>
	/// Gets or sets the endpoint URL for the translation service.
	/// </summary>
	public string TranslateEndpoint { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the endpoint URL used for file translation requests.
	/// </summary>
	public string TranslateFileEndpoint { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the number of seconds to wait before retrying a failed operation.
	/// </summary>
	public int WaitSecondBeforeRetry { get; set; } = 2;
}