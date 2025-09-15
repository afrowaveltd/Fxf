namespace Fxf.Blazor.Services;

/// <summary>
/// Defines a contract for managing HTTP cookies, including setting, retrieving, and removing cookies.
/// </summary>
/// <remarks>
/// Implementations typically interact with the current HTTP context to manage cookies. This is
/// intended for server-side scenarios (e.g., ASP.NET Core/Blazor Server) where an
/// <c>HttpContext</c> is available and may not be applicable to Blazor WebAssembly.
/// </remarks>
public interface ICookieService
{
	/// <summary>
	/// Gets the value of a cookie by key.
	/// </summary>
	/// <param name="key">The cookie key.</param>
	/// <returns>The cookie value if found; otherwise, an empty string.</returns>
	string GetCookie(string key);

	/// <summary>
	/// Gets the value of a cookie by key, or creates it if it does not exist.
	/// </summary>
	/// <param name="key">The cookie key.</param>
	/// <param name="value">The value to set if the cookie does not exist.</param>
	/// <param name="expireTime">
	/// The expiration time in minutes. Use <c>0</c> to apply the service's default.
	/// </param>
	/// <returns>The existing cookie value if found; otherwise, the provided value that was set.</returns>
	string GetOrCreateCookie(string key, string value, int expireTime = 0);

	/// <summary>
	/// Removes all cookies available in the current HTTP request context.
	/// </summary>
	void RemoveAllCookies();

	/// <summary>
	/// Removes a cookie by key.
	/// </summary>
	/// <param name="key">The cookie key.</param>
	void RemoveCookie(string key);

	/// <summary>
	/// Sets a cookie with the specified key, value, and optional expiration time.
	/// </summary>
	/// <param name="key">The cookie key.</param>
	/// <param name="value">The cookie value.</param>
	/// <param name="expireTime">
	/// The expiration time in minutes. Use <c>0</c> to apply the service's default.
	/// </param>
	void SetCookie(string key, string value, int expireTime = 0);

	/// <summary>
	/// Sets an HTTP-only cookie with the specified key, value, and optional expiration time.
	/// </summary>
	/// <param name="key">The cookie key.</param>
	/// <param name="value">The cookie value.</param>
	/// <param name="expireTime">
	/// The expiration time in minutes. Use <c>0</c> to apply the service's default.
	/// </param>
	void SetHttpOnlyCookie(string key, string value, int expireTime = 0);
}