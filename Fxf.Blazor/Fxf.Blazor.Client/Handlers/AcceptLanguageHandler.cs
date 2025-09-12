namespace Fxf.Blazor.Client.Handlers;

/// <summary>
/// A message handler that sets the "Accept-Language" header for outgoing HTTP requests based on the culture provided by
/// a delegate.
/// </summary>
/// <remarks>This handler is typically used to ensure that HTTP requests include an "Accept-Language" header
/// reflecting the desired culture for localization purposes. The culture is determined dynamically by invoking the
/// provided delegate for each request. If the delegate returns an empty string or <see langword="null"/>, the
/// "Accept-Language" header will not be set.</remarks>
public class AcceptLanguageHandler : DelegatingHandler
{
	private readonly Func<string> _getCulture;

	/// <summary>
	/// A message handler that sets the "Accept-Language" header for outgoing HTTP requests based on the culture provided
	/// by a delegate.
	/// </summary>
	/// <remarks>This handler is typically used to ensure that HTTP requests include an "Accept-Language" header
	/// reflecting the desired culture for localization purposes. The delegate is invoked for each request, allowing
	/// dynamic determination of the culture.</remarks>
	/// <param name="getCulture">A delegate that returns the culture string to be used for the "Accept-Language" header. The returned value must be
	/// a valid culture name (e.g., "en-US", "fr-FR") or an empty string if no header should be set.</param>
	public AcceptLanguageHandler(Func<string> getCulture) => _getCulture = getCulture;

	/// <summary>
	/// Sends an HTTP request asynchronously with the specified request message and cancellation token.
	/// </summary>
	/// <remarks>If a culture is determined by the internal logic, the request's "Accept-Language" header is updated
	/// to reflect the culture before sending the request.</remarks>
	/// <param name="req">The HTTP request message to send. Must not be <see langword="null"/>.</param>
	/// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message received from
	/// the server.</returns>
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct)
	{
		var lang = _getCulture();
		if(!string.IsNullOrWhiteSpace(lang))
		{
			req.Headers.AcceptLanguage.Clear();
			req.Headers.AcceptLanguage.ParseAdd(lang);
		}
		return base.SendAsync(req, ct);
	}
}