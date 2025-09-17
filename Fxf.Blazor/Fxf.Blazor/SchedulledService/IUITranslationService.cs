namespace Fxf.Blazor.SchedulledService;

/// <summary>
/// Defines a service for running user interface translation operations asynchronously.
/// </summary>
public interface IUITranslationService
{
	/// <summary>
	/// Asynchronously executes the operation represented by the implementing class.
	/// </summary>
	/// <returns>A task that represents the asynchronous execution of the operation.</returns>
	Task RunAsync();
}