using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fxf.Blazor.Api;

/// <summary>
/// Provides API endpoints for managing the user's language preference in the application.
/// </summary>
/// <remarks>
/// This controller enables clients to set the preferred culture for localization by storing the
/// culture code in a browser cookie. All endpoints are accessible under the 'api/language' route.
/// The controller relies on ASP.NET Core localization services to deliver localized content.
/// </remarks>
/// <param name="localizer">The string localizer used to provide localized error messages and responses.</param>
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
public class LanguageController(IStringLocalizer<LanguageController> localizer) : ControllerBase
{
	private readonly IStringLocalizer<LanguageController> _localizer = localizer;

	/// <summary>
	/// Sets the user's preferred language by storing the specified culture code in a browser cookie.
	/// </summary>
	/// <remarks>
	/// The culture preference is stored in a cookie named "BlazorCulture" that expires after one
	/// year. This method performs basic validation on the culture code and does not verify if the
	/// culture is supported by the application.
	/// </remarks>
	/// <param name="culture">
	/// The two-letter ISO culture code representing the language to set. Must not be null, empty, or
	/// contain only whitespace.
	/// </param>
	/// <returns>
	/// An HTTP 200 OK response if the culture code is valid and the language is set; otherwise, an
	/// HTTP 400 Bad Request response with an error message.
	/// </returns>
	[HttpGet("set/{culture}")]
	public IActionResult SetLanguage(string culture)
	{
		if(string.IsNullOrWhiteSpace(culture))
		{
			return BadRequest(new ErrorResponse(_localizer["Culture cannot be null or empty."]));
		}
		if(culture.Length != 2)
		{
			// Basic validation for culture code length
			return BadRequest(new ErrorResponse(_localizer["Invalid culture code."]));
		}

		// Set the culture in a cookie
		Response.Cookies.Append(
			"BlazorCulture",
			culture,
			new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
		);
		return Ok();
	}
}