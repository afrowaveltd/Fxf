using Fxf.Blazor.Services;
using System.Globalization;

namespace Fxf.Blazor.Middlewares;

public class LocalizationMiddleware(ICookieService cookieService) : IMiddleware
{
	private readonly ICookieService _cookieService = cookieService;

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		// we will check cookie first, then query and acceptLanguage hearders last - default en

		string? cultureQuery = GetCultureFromRequest(context);

		if(!string.IsNullOrWhiteSpace(cultureQuery))
		{
			_cookieService.SetCookie("BlazorCulture", cultureQuery, 30 * 24 * 60); // 30 days
			var culture = new System.Globalization.CultureInfo(cultureQuery);
			System.Globalization.CultureInfo.CurrentCulture = culture;
			// Check which locales are supported
		}
		await next(context);
	}

	private string? GetCultureFromRequest(HttpContext context)
	{
		string? culture = _cookieService.GetCookie("BlazorCulture");
		if(!string.IsNullOrWhiteSpace(culture))
		{
			return culture;
		}
		culture = context.Request.Query["culture"];
		if(!string.IsNullOrWhiteSpace(culture))
		{
			return culture;
		}
		culture = context.Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault();
		if(!string.IsNullOrWhiteSpace(culture))
		{
			return culture;
		}
		return "en";
	}

	private static bool IsValidCulture(string? name)
	{
		return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, name, StringComparison.CurrentCultureIgnoreCase));
	}
}