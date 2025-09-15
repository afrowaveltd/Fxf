namespace Fxf.Blazor.Services;

/// <summary>
/// Provides a lightweight HTTP helper tailored for translation-related operations. Configures an
/// <see cref="HttpClient"/> using <see cref="Translator"/> settings and exposes convenience methods
/// for common HTTP scenarios (GET, JSON POST, form POST, multipart POST).
/// </summary>
public class HttpService : IHttpService
{
	private readonly string _apiKey;
	private readonly string _baseUrl;
	private readonly HttpClient _httpClient;
	private readonly bool _needsKey;

	private readonly JsonSerializerOptions _options = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		DefaultIgnoreCondition = JsonIgnoreCondition.Never,
		ReferenceHandler = ReferenceHandler.IgnoreCycles
	};

	private readonly Translator _translatorOptions;

	/// <summary>
	/// Initializes a new instance of the <see cref="HttpService"/> class using configuration-bound
	/// <see cref="Translator"/> options to set the base address and authorization header.
	/// </summary>
	/// <param name="configuration">
	/// Application configuration used to bind the <c>Translator</c> section.
	/// </param>
	public HttpService(IConfiguration configuration)
	{
		_translatorOptions = configuration.GetSection("Translator").Get<Translator>() ?? new Translator();
		_baseUrl = _translatorOptions.Host ?? string.Empty;
		_apiKey = _translatorOptions.ApiKey ?? string.Empty;
		_needsKey = _translatorOptions.NeedsKey;
		var handler = new HttpClientHandler();
		handler.ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) =>
		{
			return true;                      // bypass for this host only

			//return errors == System.Net.Security.SslPolicyErrors.None;
		};

		_httpClient = new(handler)
		{
			DefaultRequestHeaders =
			{
				{ "Accept", "application/json" },
				{ "User-Agent", "Fxf.Blazor" }
			}
		};
		if(_needsKey && !string.IsNullOrWhiteSpace(_apiKey))
		{
			_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
		}
	}

	/// <summary>
	/// Sends a GET request to the specified URL with optional headers.
	/// </summary>
	/// <param name="url">The request URL (relative or absolute).</param>
	/// <param name="headers">Optional headers to include in the request.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
	public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? headers = null)
	{
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
		AddHeaders(request, headers);
		return await _httpClient.SendAsync(request);
	}

	/// <summary>
	/// Gets the configured <see cref="HttpClient"/> instance.
	/// </summary>
	/// <returns>The configured <see cref="HttpClient"/>.</returns>
	public HttpClient GetLibreHttpClient() => _httpClient;

	/// <summary>
	/// Sends a POST request with <see cref="FormUrlEncodedContent"/> to the specified URL with
	/// optional headers.
	/// </summary>
	/// <param name="url">The request URL (relative or absolute).</param>
	/// <param name="formFields">The form fields to send.</param>
	/// <param name="headers">Optional headers to include in the request.</param>
	/// <returns>
	/// The <see cref="HttpResponseMessage"/> returned by the server; in case of an exception, a
	/// default message instance.
	/// </returns>
	public async Task<HttpResponseMessage> PostFormAsync(string url, Dictionary<string, string> formFields, Dictionary<string, string>? headers = null)
	{
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
		{
			Content = new FormUrlEncodedContent(formFields)
		};
		AddHeaders(request, headers);
		try
		{
			return await _httpClient.SendAsync(request);
		}
		catch
		{
			return new();
		}
	}

	/// <summary>
	/// Sends a POST request with a JSON payload to the specified URL with optional headers.
	/// </summary>
	/// <typeparam name="T">The payload type.</typeparam>
	/// <param name="url">The request URL (relative or absolute).</param>
	/// <param name="payload">The payload object to serialize as JSON.</param>
	/// <param name="headers">Optional headers to include in the request.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
	public async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T payload, Dictionary<string, string>? headers = null)
	{
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
		{
			Content = JsonContent.Create(payload)
		};
		AddHeaders(request, headers);
		return await _httpClient.SendAsync(request);
	}

	/// <summary>
	/// Sends a multipart/form-data POST request to the specified URL with optional headers.
	/// </summary>
	/// <param name="url">The request URL (relative or absolute).</param>
	/// <param name="formFields">Key-value pairs to include as form fields.</param>
	/// <param name="files">A collection of files to include in the request.</param>
	/// <param name="headers">Optional headers to include in the request.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
	public async Task<HttpResponseMessage> PostMultipartAsync(string url,
			Dictionary<string, string> formFields,
			IEnumerable<(string FieldName, string FileName, Stream Content, string ContentType)> files,
			Dictionary<string, string>? headers = null)
	{
		using MultipartFormDataContent content = new MultipartFormDataContent();

		// Add form fields
		foreach(KeyValuePair<string, string> field in formFields)
		{
			content.Add(new StringContent(field.Value), field.Key);
		}

		// Add files
		foreach((string FieldName, string FileName, Stream Content, string ContentType) file in files)
		{
			StreamContent streamContent = new StreamContent(file.Content);
			streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
			content.Add(streamContent, file.FieldName, file.FileName);
		}

		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
		{
			Content = content
		};

		AddHeaders(request, headers);
		return await _httpClient.SendAsync(request);
	}

	/// <summary>
	/// Reads HTTP content as JSON into a value of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The target type to deserialize into.</typeparam>
	/// <param name="content">The HTTP content to read.</param>
	/// <param name="options">
	/// Optional <see cref="JsonSerializerOptions"/>; when null, uses <see cref="JsonSerializerDefaults.Web"/>.
	/// </param>
	/// <returns>The deserialized value, or <see langword="null"/> if the payload is empty.</returns>
	public async Task<T?> ReadJsonAsync<T>(HttpContent content, JsonSerializerOptions? options = null)
			=> await content.ReadFromJsonAsync<T>(options ?? new JsonSerializerOptions(JsonSerializerDefaults.Web));

	/// <summary>
	/// Sends an HTTP request with the underlying <see cref="HttpClient"/>.
	/// </summary>
	/// <param name="request">The request message to send.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
	public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
		=> await _httpClient.SendAsync(request, cancellationToken);

	/// <summary>
	/// Adds custom headers to the specified request.
	/// </summary>
	/// <param name="request">The target request.</param>
	/// <param name="headers">The headers to add.</param>
	private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
	{
		if(headers == null)
		{
			return;
		}

		foreach((string key, string value) in headers)
		{
			_ = request.Headers.TryAddWithoutValidation(key, value);
		}
	}
}