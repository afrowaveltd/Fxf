namespace Fxf.Blazor.Services;

public interface ICookieService
{
	string GetCookie(string key);
	string GetOrCreateCookie(string key, string value, int expireTime = 0);
	void RemoveAllCookies();
	void RemoveCookie(string key);
	void SetCookie(string key, string value, int expireTime = 0);
	void SetHttpOnlyCookie(string key, string value, int expireTime = 0);
}