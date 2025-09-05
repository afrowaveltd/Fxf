using Fxf.Blazor.Models.LibreTranslate;
using Fxf.Shared.Models;

namespace Fxf.Blazor.Services.LibreTranslate;
public interface ILibreTranslateService
{
	Task<Response<Detections>> DetectLanguageAsync(string text);
	Task<Response<string[]>> GetAvailableLanguagesAsync();
	Task<Response<TranslateFileResult>> TranslateFileAsync(Stream fileStream, string sourceLanguage, string targetLanguage, string fileName);
	Task<Response<TranslateFileResult>> TranslateFileFromAnyLanguageAsync(Stream fileStream, string targetLanguage, string fileName);
	Task<Response<TranslateResult>> TranslateTextAsync(string text, string sourceLanguage, string targetLanguage);
	Task<Response<TranslateResult>> TranslateTextFromAnyLanugageAsync(string text, string targetLanguage);
}