namespace Fxf.Blazor.Services;

/// <summary>
/// Provides methods for managing HTTP cookies, including setting, retrieving, and removing cookies.
/// </summary>
/// <remarks>This service is designed to simplify cookie management in ASP.NET Core applications by leveraging the
/// <see cref="IHttpContextAccessor"/> to access the current HTTP context. It supports creating cookies with optional
/// expiration times, retrieving cookie values, and removing cookies.</remarks>
/// <param name="httpContextAccessor"></param>
public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	/// <summary>
	/// Set a cookie with a key, value, and optional expiration time
	/// </summary>
	/// <param name="key">Cookie key</param>
	/// <param name="value">Cookie value</param>
	/// <param name="expireTime">Expiration time</param>
	public void SetCookie(string key, string value, int expireTime = 0)
	{
		CookieOptions options = new CookieOptions
		{
			Expires = expireTime == 0 ? DateTime.Now.AddYears(1) : DateTime.Now.AddMinutes(expireTime),
			IsEssential = true,
			Secure = true,
		};

		_httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
	}

	/// <summary>
	/// Set a cookie with a key, value, and optional expiration time
	/// </summary>
	/// <param name="key">Cookie key</param>
	/// <param name="value">Cookie value</param>
	/// <param name="expireTime">Expiration time</param>
	public void SetHttpOnlyCookie(string key, string value, int expireTime = 0)
	{
		CookieOptions options = new CookieOptions
		{
			Expires = expireTime == 0 ? DateTime.Now.AddYears(10) : DateTime.Now.AddMinutes(expireTime),
			IsEssential = true,
			Secure = true,
			HttpOnly = true,
		};
		_httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
	}

	/// <summary>
	/// Get a cookie by key
	/// </summary>
	/// <param name="key">name of the cookie to get</param>
	/// <returns>The cookie value or empty string</returns>
	public string GetCookie(string key)
	{
		return _httpContextAccessor.HttpContext?.Request.Cookies[key] ?? string.Empty;
	}

	/// <summary>
	/// Remove a cookie by key
	/// </summary>
	/// <param name="key">Cookie key</param>
	public void RemoveCookie(string key)
	{
		_httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
	}

	/// <summary>
	/// Remove all cookies
	/// </summary>
	public void RemoveAllCookies()
	{
		if(_httpContextAccessor.HttpContext == null) return;
		foreach(string cookie in _httpContextAccessor.HttpContext?.Request.Cookies.Keys)
		{
			_httpContextAccessor.HttpContext?.Response.Cookies.Delete(cookie);
		}
	}

	/// <summary>
	/// Get a cookie by key, or create it if it doesn't exist
	/// </summary>
	/// <param name="key">Cookie key</param>
	/// <param name="value">Cookie value</param>
	/// <param name="expireTime">Cookie expiration time</param>
	/// <returns></returns>
	public string GetOrCreateCookie(string key, string value, int expireTime = 0)
	{
		if(string.IsNullOrEmpty(GetCookie(key)))
		{
			SetCookie(key, value, expireTime);
			return value;
		}
		else
		{
			return GetCookie(key);
		}
	}
}