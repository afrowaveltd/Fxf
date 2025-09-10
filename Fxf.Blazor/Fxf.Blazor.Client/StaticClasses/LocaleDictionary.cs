namespace Fxf.Blazor.Client.StaticClasses;

/// <summary>
/// Provides a dictionary for storing and retrieving locale-specific string mappings.
/// </summary>
/// <remarks>The <see cref="Locales"/> property is a static dictionary that can be used to manage key-value pairs
/// where the key represents a locale identifier (e.g., "en-US") and the value represents the corresponding  localized
/// string. This class is designed for scenarios where locale-specific data needs to be accessed  or modified globally
/// within an application.</remarks>
public static class LocaleDictionary
{
	/// <summary>
	/// Gets or sets a dictionary of locale identifiers and their corresponding display names.
	/// </summary>
	/// <remarks>This property can be used to manage and retrieve localized display names for supported
	/// locales.</remarks>
	public static Dictionary<string, string> Locales { get; set; } = [];

	/// <summary>
	/// Resets the application state by clearing all configured locales.
	/// </summary>
	/// <remarks>This method removes all entries from the <c>Locales</c> collection, effectively resetting it to an
	/// empty state. Use this method to reinitialize the locale configuration.</remarks>
	public static void Reset()
	{
		Locales = [];
	}
}