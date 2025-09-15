using System.ComponentModel.DataAnnotations;
using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Data.Entities;

/// <summary>
/// Represents the number of translation requests to add, remove, or update during a worker process.
/// </summary>
public class TranslationRequests
{
	/// <summary>
	/// Gets or sets the language code for which the translation requests are tracked.
	/// </summary>
	public string LanguageCode { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the number of translation requests to add.
	/// </summary>
	public string Phrase { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the number of translation requests to remove.
	/// </summary>
	public PhraseChange ChangeType { get; set; } = PhraseChange.Add;
}