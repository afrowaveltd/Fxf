using Fxf.Blazor.Client.Services;
using Microsoft.Extensions.Localization;

namespace Fxf.Blazor.Client.I18n;

/// <summary>
/// Factory for creating <see cref="JsonStringLocalizer"/> instances used for client-side Blazor
/// localization. Resolves dependencies via DI ( <see cref="IApiClientService"/> and <see
/// cref="ILocaleService"/>) and supplies them to each created localizer. This implementation does
/// not differentiate resources by type/name; it returns a culture-aware JSON-backed localizer for
/// every request.
/// </summary>
public sealed class JsonStringLocalizerFactory(IApiClientService api, ILocaleService locale) : IStringLocalizerFactory
{
	private readonly IApiClientService _api = api;
	private readonly ILocaleService _locale = locale;

	/// <summary>
	/// Creates a new <see cref="JsonStringLocalizer"/> for the specified resource type.
	/// </summary>
	/// <param name="resourceSource">The resource type (unused; present for interface compatibility).</param>
	/// <returns>A <see cref="IStringLocalizer"/> instance.</returns>
	public IStringLocalizer Create(Type resourceSource)
	{
		return (IStringLocalizer)new JsonStringLocalizer(_api, _locale);
	}

	/// <summary>
	/// Creates a new <see cref="JsonStringLocalizer"/> for the specified base name and location.
	/// </summary>
	/// <param name="baseName">Base name of the resource (ignored in this implementation).</param>
	/// <param name="location">Resource location (ignored in this implementation).</param>
	/// <returns>A <see cref="IStringLocalizer"/> instance.</returns>
	IStringLocalizer IStringLocalizerFactory.Create(string baseName, string location)
	{
		return (IStringLocalizer)new JsonStringLocalizer(_api, _locale);
	}
}