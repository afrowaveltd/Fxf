using static Fxf.Blazor.Models.Enums;

namespace Fxf.Blazor.Models.Worker;

/// <summary>
/// Represents a translation entry to be added, updated, or removed in a localization system.
/// </summary>
/// <remarks>Use this class to specify the details of a translation modification, including the target language,
/// the translation key, the new value, and the type of change to apply. The default language is "en" and the default
/// change type is <see cref="PhraseChange.Add"/>.</remarks>
public class TranslationToModify
{
	public string Language { get; set; } = "en";
	public string Key { get; set; } = string.Empty;
	public string Value { get; set; } = string.Empty;
	public bool IsFrontend { get; set; } = true;
	public PhraseChange Change { get; set; } = PhraseChange.Add;
}