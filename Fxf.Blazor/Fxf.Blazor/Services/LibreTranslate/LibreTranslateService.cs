namespace Fxf.Blazor.Services.LibreTranslate;

/// <summary>
/// Service wrapper around a LibreTranslate-compatible API. Provides utilities to query available
/// languages, translate text and files, and detect language. Uses <see cref="IHttpService"/> for
/// HTTP operations and supports retries based on <see cref="Translator"/> settings.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="LibreTranslateService"/> class.</remarks>
/// <param name="configuration">Application configuration containing the <c>Translator</c> section.</param>
/// <param name="httpService">HTTP abstraction used to perform API calls.</param>
/// <param name="hub">SignalR hub context for server push notifications (reserved for future use).</param>
public class LibreTranslateService(IConfiguration configuration, IHttpService httpService, IHubContext<LocalizationHub> hub) : ILibreTranslateService
{
	private readonly IHttpService _httpService = httpService;
	private readonly IHubContext<LocalizationHub> _hub = hub;

	private readonly JsonSerializerOptions _options = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = JsonIgnoreCondition.Never,
		ReferenceHandler = ReferenceHandler.IgnoreCycles
	};

	private readonly Translator _translatorOptions = configuration.GetSection("Translators").Get<List<Translator>>()?[0] ?? new Translator();

	/// <summary>
	/// Detects the language of the provided text.
	/// </summary>
	/// <param name="text">The text for which the language should be detected.</param>
	/// <returns>
	/// A <see cref="Response{T}"/> containing the detection result with language and confidence.
	/// </returns>
	public async Task<Response<Detections>> DetectLanguageAsync(string text)
	{
		var formFields = new Dictionary<string, string>
			{
				{ "q", text }
			};
		if(!string.IsNullOrEmpty(_translatorOptions.ApiKey) && _translatorOptions.NeedsKey)
		{
			formFields.Add("api_key", _translatorOptions.ApiKey);
		}
		var response = await _httpService.PostFormAsync(_translatorOptions.Host + _translatorOptions.DetectLanguageEndpoint, formFields);
		if(!response.IsSuccessStatusCode)
		{
			return Response<Detections>.Fail($"Failed to detect language: {response.ReasonPhrase}");
		}
		var result = await _httpService.ReadJsonAsync<Detections>(response.Content, _options);
		if(result == null)
		{
			return Response<Detections>.Fail("Failed to deserialize detection result.");
		}
		return Response<Detections>.Successful(result ?? new(), "Language detection successful");
	}

	/// <summary>
	/// Gets the list of available language codes from the translation service.
	/// </summary>
	/// <remarks>
	/// Retries are performed according to <see cref="Translator.RetriesOnFailure"/> and <see
	/// cref="Translator.WaitSecondBeforeRetry"/> when transient HTTP errors occur.
	/// </remarks>
	/// <returns>A <see cref="Response{T}"/> with an array of language codes (e.g., "en", "cs").</returns>
	public async Task<Response<string[]>> GetAvailableLanguagesAsync()
	{
		bool repeat = true;
		int retries = 0;
		Response<string[]> result = new();
		DateTime start = DateTime.UtcNow;

		while(repeat && retries < _translatorOptions.RetriesOnFailure)
		{
			try
			{
				var response = await _httpService.GetAsync(_translatorOptions.Host + _translatorOptions.LanguagesEndpoint);

				response.EnsureSuccessStatusCode();
				var languages = await _httpService.ReadJsonAsync<List<LibreLanguage>>(response.Content, _options) ?? new();

				if(languages.Count == 0)
				{
					repeat = false;
					result.Success = true;
					result.Warning = true;
					result.Message += "Warning: No supported languages found";
					retries = 0;
				}

				result.Data = [.. languages.Select(c => c.Code)];
				retries = 0;
				repeat = false;
				result.Success = true;
			}
			catch(HttpRequestException ex)
			{
				retries++;
				await Task.Delay(_translatorOptions.WaitSecondBeforeRetry * 1000);
				result.Success = false;
				result.Warning = false;
				result.Message += $"Error: Try {retries}: {ex.Message}\n";
			}
		}
		result.ExecutionTime = DateTime.UtcNow.Subtract(start).Milliseconds;
		return result;
	}

	/// <summary>
	/// Translates a file from the specified source language to the target language.
	/// </summary>
	/// <param name="fileStream">The file content stream.</param>
	/// <param name="sourceLanguage">Two-letter ISO code of the source language.</param>
	/// <param name="targetLanguage">Two-letter ISO code of the target language.</param>
	/// <param name="fileName">The original file name (used for multipart content).</param>
	/// <returns>A <see cref="Response{T}"/> containing a <see cref="TranslateFileResult"/>.</returns>
	public async Task<Response<TranslateFileResult>> TranslateFileAsync(Stream fileStream, string sourceLanguage, string targetLanguage, string fileName)
	{
		DateTime start = DateTime.Now;
		bool leaveLoop = false;
		int retries = 0;
		int maxRetries = _translatorOptions.RetriesOnFailure == 0 ? 1 : _translatorOptions.RetriesOnFailure;
		int delay = _translatorOptions.WaitSecondBeforeRetry == 0 ? 1 : _translatorOptions.WaitSecondBeforeRetry;

		var formFields = new Dictionary<string, string>
			{
				{ "source", sourceLanguage },
				{ "target", targetLanguage },
				{ "format", "text" }
			};

		if(!string.IsNullOrEmpty(_translatorOptions.ApiKey) && _translatorOptions.NeedsKey)
		{
			formFields.Add("api_key", _translatorOptions.ApiKey);
		}

		var files = new List<(string FieldName, string FileName, Stream Content, string ContentType)>
			{
				("file", fileName, fileStream, "application/octet-stream")
			};

		while(!leaveLoop)
		{
			var response = await _httpService.PostMultipartAsync(_translatorOptions.Host + _translatorOptions.TranslateFileEndpoint, formFields, files);
			if(response.StatusCode == System.Net.HttpStatusCode.BadGateway
					|| response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
			{
				if(retries < maxRetries)
				{
					retries++;

					await Task.Delay(delay);
				}
				else
				{
					leaveLoop = true;

					return new Response<TranslateFileResult>
					{
						Data = new TranslateFileResult() { },
						Success = false,
						ExecutionTime = DateTime.Now.Subtract(start).Milliseconds
					};
				}
			}
			if(!response.IsSuccessStatusCode)
			{
				return Response<TranslateFileResult>.Fail($"Failed to translate file: {response.ReasonPhrase}", DateTime.Now.Subtract(start).Milliseconds);
			}
			var result = await _httpService.ReadJsonAsync<TranslateFileResult>(response.Content, _options);
			if(result == null)
			{
				return Response<TranslateFileResult>.Fail("Failed to deserialize translation result.", DateTime.Now.Subtract(start).Milliseconds);
			}
			return Response<TranslateFileResult>.Successful(result ?? new(), "Translation successful", DateTime.Now.Subtract(start).Milliseconds);
		}
		return Response<TranslateFileResult>.Fail("Translation unsuccessful");
	}

	/// <summary>
	/// Translates a file from an auto-detected language to the specified target language.
	/// </summary>
	/// <param name="fileStream">The file content stream.</param>
	/// <param name="targetLanguage">Two-letter ISO code of the target language.</param>
	/// <param name="fileName">The original file name (used for multipart content).</param>
	/// <returns>A <see cref="Response{T}"/> containing a <see cref="TranslateFileResult"/>.</returns>
	public async Task<Response<TranslateFileResult>> TranslateFileFromAnyLanguageAsync(Stream fileStream, string targetLanguage, string fileName)
	{
		return await TranslateFileAsync(fileStream, "auto", targetLanguage, fileName);
	}

	/// <summary>
	/// Translates a text from a specified source language to a target language.
	/// </summary>
	/// <param name="text">The text to translate. Cannot be null or empty.</param>
	/// <param name="sourceLanguage">
	/// Two-letter ISO code of the source language. Must not be "auto" here.
	/// </param>
	/// <param name="targetLanguage">Two-letter ISO code of the target language.</param>
	/// <returns>A <see cref="Response{T}"/> containing a <see cref="TranslateResult"/> on success.</returns>
	public async Task<Response<TranslateResult>> TranslateTextAsync(string text, string sourceLanguage, string targetLanguage)
	{
		DateTime start = DateTime.Now;
		bool exitTheLoop = false;
		bool decapitalized = false; // Libre translate sometimes fails to translate words starting with a capital letter
		int retries = 0;
		int maxRetries = _translatorOptions.RetriesOnFailure;
		if(maxRetries == 0)
			maxRetries = 1;
		int delay = _translatorOptions.WaitSecondBeforeRetry * 1000; // seconds to ms

		if(targetLanguage.Length != 2)
		{
			return new Response<TranslateResult>()
			{
				Success = false,
				Data = new TranslateResult(),
				Message = "Invalid target language",
				ExecutionTime = DateTime.Now.Subtract(start).Milliseconds,
			};
		}

		if(string.IsNullOrEmpty(text))
		{
			return new Response<TranslateResult>()
			{
				Success = false,
				Data = new TranslateResult(),
				Message = "Text cannot be null or empty",
				ExecutionTime = DateTime.Now.Subtract(start).Milliseconds
			};
		}

		if(string.IsNullOrEmpty(sourceLanguage) || sourceLanguage.Length != 2 || sourceLanguage == "auto")
		{
			return new Response<TranslateResult>()
			{
				Success = false,
				Data = new TranslateResult(),
				Message = "Invalid source language",
				ExecutionTime = DateTime.Now.Subtract(start).Milliseconds
			};
		}

		var formFields = new Dictionary<string, string>
			{
				{ "q", text },
				{ "source", sourceLanguage },
				{ "target", targetLanguage },
				{ "format", "text" },
				{ "alternatives", "2" }
			};
		if(!string.IsNullOrEmpty(_translatorOptions.ApiKey) && _translatorOptions.NeedsKey)
		{
			formFields.Add("api_key", _translatorOptions.ApiKey);
		}

		while(!exitTheLoop)
		{
			var response = await _httpService.PostFormAsync(_translatorOptions.Host + _translatorOptions.TranslateEndpoint, formFields);
			if(!response.IsSuccessStatusCode)
			{
				if(response.StatusCode == System.Net.HttpStatusCode.BadGateway
					|| response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					if(retries < maxRetries)
					{
						retries++;
						await Task.Delay(delay);
					}
					else
					{
						exitTheLoop = true;
						return new Response<TranslateResult>
						{
							Data = new TranslateResult() { TranslatedText = text },
							Success = false,
							ExecutionTime = DateTime.Now.Subtract(start).Milliseconds
						};
					}
				}
				else
				{
					return new Response<TranslateResult>
					{
						Data = new TranslateResult() { TranslatedText = text },
						Success = false,
						Message = response.ReasonPhrase,
						ExecutionTime = DateTime.Now.Subtract(start).Milliseconds
					};
				}
			}
			try
			{
				var result = await _httpService.ReadJsonAsync<TranslateResult>(response.Content, _options);

				if(result == null)
				{
					return Response<TranslateResult>.Fail("Failed to deserialize translation result.", DateTime.Now.Subtract(start).Milliseconds);
				}
				if(result.TranslatedText == text || string.IsNullOrEmpty(result.TranslatedText))
				{
					if(text == text.ToLower() || decapitalized)
					{
						return Response<TranslateResult>.Successful(result ?? new(), "Translation successful", DateTime.Now.Subtract(start).Milliseconds);
					}
					else
					{
						text = text.ToLower();
						decapitalized = true;
					}
				}
				else
				{
					return Response<TranslateResult>.Successful(result ?? new(), "Translation successful", DateTime.Now.Subtract(start).Milliseconds);
				}
			}
			catch(Exception e)
			{
				return new() { Success = false, Message = e.Message };
			}
		}
		return new();
	}

	/// <summary>
	/// Translates text from any language (auto-detected) to the specified target language.
	/// </summary>
	/// <param name="text">The text to translate.</param>
	/// <param name="targetLanguage">Two-letter ISO code of the target language.</param>
	/// <returns>A <see cref="Response{T}"/> with the translation result.</returns>
	public async Task<Response<TranslateResult>> TranslateTextFromAnyLanugageAsync(string text, string targetLanguage)
	{
		return await TranslateTextAsync(text, "auto", targetLanguage);
	}
}