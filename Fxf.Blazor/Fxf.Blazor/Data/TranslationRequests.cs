using System.ComponentModel.DataAnnotations;

namespace Fxf.Blazor.Data;

/// <summary>
/// Represents the number of translation requests to add, remove, or update during a worker process.
/// </summary>
public class TranslationRequests
{
	/// <summary>
	/// Gets or sets the unique identifier for the entity.
	/// </summary>
	[Key]
	public int Id { get; set; }
   /// <summary>
   /// Gets or sets the language code for which the translation requests are tracked.
   /// </summary>
   public string LanguageCode { get; set; } = string.Empty;
   /// <summary>
   /// Gets or sets the number of translation requests to add.
   /// </summary>
   public int ToAdd { get; set; } = 0;

	/// <summary>
	/// Gets or sets the number of translation requests to remove.
	/// </summary>
	public int ToRemove { get; set; } = 0;

	/// <summary>
	/// Gets or sets the number of translation requests to update.
	/// </summary>
	public int ToUpdate { get; set; } = 0;
}
