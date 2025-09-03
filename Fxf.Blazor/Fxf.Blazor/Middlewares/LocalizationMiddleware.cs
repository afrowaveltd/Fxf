using Fxf.Blazor.Services;

namespace Fxf.Blazor.Middlewares;

public class LocalizationMiddleware(ICookieService cookieService) : IMiddleware
{
	private readonly ICookieService _cookieService = cookieService;

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		// we will check cookie first, then query and acceptLanguage hearders last - default en

		string? cultureQuery = context.Request.Query["culture"];

		if(!string.IsNullOrWhiteSpace(cultureQuery))
		{
			_cookieService.SetCookie("BlazorCulture", cultureQuery, 30 * 24 * 60); // 30 days
		}
		await next(context);
	}


}