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
			var cultureData = new System.Globalization.CultureInfo(cultureQuery);
			CultureInfo uiCulture = IsValidCulture(cultureQuery) ? new CultureInfo(cultureQuery) : new CultureInfo("en");
			CultureInfo culture = IsValidCulture(cultureQuery) ? new CultureInfo(cultureQuery) : new CultureInfo("en");

			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = uiCulture;
			context.Request.Headers.AcceptLanguage = cultureQuery;
			_cookieService.SetCookie("BlazorCulture", cultureQuery, 30 * 24 * 60); // 30 days
																										  // Check which locales are supported
		}
		else
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
			context.Request.Headers.AcceptLanguage = "en";
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