using System.Globalization;

namespace Fxf.Blazor.Middlewares;

/// <summary>
/// Middleware for setting the current request's culture based on cookies, query parameters, or
/// Accept-Language headers.
/// </summary>
/// <remarks>
/// This middleware checks for a culture value in the following order: cookie, query string,
/// Accept-Language header. If none are found, it defaults to "en".
/// </remarks>
public class LocalizationMiddleware(ICookieService cookieService) : IMiddleware
{
	private readonly ICookieService _cookieService = cookieService;

	/// <summary>
	/// Invokes the middleware to set the current thread's culture and UI culture for the request.
	/// </summary>
	/// <param name="context">The current HTTP context.</param>
	/// <param name="next">The next middleware in the pipeline.</param>
	/// <returns>A task that represents the completion of request processing.</returns>
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

	/// <summary>
	/// Determines whether the specified culture name is valid.
	/// </summary>
	/// <param name="name">The culture name to validate.</param>
	/// <returns><c>true</c> if the culture is valid; otherwise, <c>false</c>.</returns>
	private static bool IsValidCulture(string? name)
	{
		return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, name, StringComparison.CurrentCultureIgnoreCase));
	}

	/// <summary>
	/// Gets the culture value from the request by checking cookie, query string, and Accept-Language header.
	/// </summary>
	/// <param name="context">The current HTTP context.</param>
	/// <returns>The culture name if found; otherwise, "en".</returns>
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
}