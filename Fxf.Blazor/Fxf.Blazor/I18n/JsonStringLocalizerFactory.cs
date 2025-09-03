using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace Fxf.Blazor.I18n;

/// <summary>
/// Factory for creating instances of <see cref="JsonStringLocalizer"/> for Blazor localization using JSON files.
/// </summary>
public class JsonStringLocalizerFactory(IDistributedCache cache) : IStringLocalizerFactory
{
	private readonly IDistributedCache _cache = cache;

	/// <summary>
	/// Creates a <see cref="JsonStringLocalizer"/> for the specified resource type.
	/// </summary>
	/// <param name="resourceSource">The type of the resource to localize.</param>
	/// <returns>A <see cref="IStringLocalizer"/> instance for the resource.</returns>
	public IStringLocalizer Create(Type resourceSource)
	{
		return new JsonStringLocalizer(_cache);
	}

	/// <summary>
	/// Creates a <see cref="JsonStringLocalizer"/> for the specified base name and location.
	/// </summary>
	/// <param name="baseName">The base name of the resource.</param>
	/// <param name="location">The location of the resource.</param>
	/// <returns>A <see cref="IStringLocalizer"/> instance for the resource.</returns>
	public IStringLocalizer Create(string baseName, string location)
	{
		return new JsonStringLocalizer(_cache);
	}
}