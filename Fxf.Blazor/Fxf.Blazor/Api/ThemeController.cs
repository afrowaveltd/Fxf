using Microsoft.AspNetCore.Mvc;

namespace Fxf.Blazor.Api;

/// <summary>
/// Provides API endpoints for managing and applying themes within the application.
/// </summary>
/// <param name="themeService">The service used to manage theme state and retrieve theme-related information.</param>
/// <param name="localizer">The string localizer used to provide localized error messages and responses for this controller.</param>
[Microsoft.AspNetCore.Mvc.Route("/api/theme")]
[ApiController]
public class ThemeController(IThemeService themeService, IStringLocalizer<ThemeController> localizer) : ControllerBase
{
	private readonly IThemeService _themeService = themeService;
	private readonly IStringLocalizer<ThemeController> _localizer = localizer;

	/// <summary>
	/// Sets the current application theme and returns a link to the theme resource.
	/// </summary>
	/// <param name="themeName">The name of the theme to apply. Cannot be null or empty.</param>
	/// <returns>An <see cref="OkObjectResult"/> containing the link to the theme resource if successful; otherwise, a <see
	/// cref="BadRequestObjectResult"/> with an error message if <paramref name="theme"/> is null or empty.</returns>
	[HttpGet("set/{themeName}")]
	public IActionResult SetTheme(string themeName)
	{
		if(string.IsNullOrEmpty(themeName))
		{
			return BadRequest(new ErrorResponse(_localizer["The query cannot be null or empty."].Value));
		}
		_themeService.CurrentTheme = themeName;

		return Ok(_themeService.GetThemeLink(themeName));
	}
}