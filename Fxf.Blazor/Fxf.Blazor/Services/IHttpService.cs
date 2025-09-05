using System.Text.Json;

namespace Fxf.Blazor.Services;

/// <summary>
/// Defines a lightweight HTTP helper abstraction used by translation and localization features.
/// Exposes convenience methods for common HTTP scenarios (GET, JSON POST, form POST, multipart POST),
/// plus helpers for sending requests and reading JSON payloads.
/// </summary>
public interface IHttpService
{
	/// <summary>
	/// Sends a GET request to the specified URL with optional headers.
	/// </summary>
	/// <param name="url">The request URL (relative or absolute).</param>
	/// <param name="headers">Optional headers to include in the request.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
	Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? headers = null);

	/// <summary>
	/// Gets the configured <see cref="HttpClient"/> instance.
	/// </summary>
	/// <returns>The configured <see cref="HttpClient"/>.</returns>
	HttpClient GetLibreHttpClient();

	/// <summary>
	/// Sends a POST request with <see cref="FormUrlEncodedContent"/> to the specified URL with optional headers.
	/// </summary>
	/// <param name="url">The request URL (relative or absolute).</param>
	/// <param name="formFields">The form fields to send.</param>
	/// <param name="headers">Optional headers to include in the request.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server; in case of an exception, a default message instance.</returns>
	Task<HttpResponseMessage> PostFormAsync(string url, Dictionary<string, string> formFields, Dictionary<string, string>? headers = null);

	/// <summary>
	/// Sends a POST request with a JSON payload to the specified URL with optional headers.
	/// </summary>
	/// <typeparam name="T">The payload type.</typeparam>
	/// <param name="url">The request URL (relative or absolute).</param>
	/// <param name="payload">The payload object to serialize as JSON.</param>
	/// <param name="headers">Optional headers to include in the request.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
	Task<HttpResponseMessage> PostJsonAsync<T>(string url, T payload, Dictionary<string, string>? headers = null);

	/// <summary>
	/// Sends a multipart/form-data POST request to the specified URL with optional headers.
	/// </summary>
	/// <param name="url">The request URL (relative or absolute).</param>
	/// <param name="formFields">Key-value pairs to include as form fields.</param>
	/// <param name="files">A collection of files to include in the request.</param>
	/// <param name="headers">Optional headers to include in the request.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
	Task<HttpResponseMessage> PostMultipartAsync(string url, Dictionary<string, string> formFields, IEnumerable<(string FieldName, string FileName, Stream Content, string ContentType)> files, Dictionary<string, string>? headers = null);

	/// <summary>
	/// Reads HTTP content as JSON into a value of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The target type to deserialize into.</typeparam>
	/// <param name="content">The HTTP content to read.</param>
	/// <param name="options">Optional <see cref="JsonSerializerOptions"/>; when null, uses <see cref="JsonSerializerDefaults.Web"/>.</param>
	/// <returns>The deserialized value, or <see langword="null"/> if the payload is empty.</returns>
	Task<T?> ReadJsonAsync<T>(HttpContent content, JsonSerializerOptions? options = null);

	/// <summary>
	/// Sends an HTTP request with the underlying <see cref="HttpClient"/>.
	/// </summary>
	/// <param name="request">The request message to send.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
	Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
}