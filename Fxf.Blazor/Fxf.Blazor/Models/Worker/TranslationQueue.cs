namespace Fxf.Blazor.Models.Worker;

public class TranslationQueue
{
	public bool IsFrontend { get; set; } = true;
	public string Language { get; set; } = string.Empty;
}