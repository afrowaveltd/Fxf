using Fxf.Shared.Models;

namespace Fxf.Blazor.Services.LibreTranslate;

/// <summary>
/// Defines methods for interacting with the LibreTranslate service, including language detection, translation, and file translation operations.
/// </summary>
public interface ILibreTranslateService
{
	/// <summary>
	/// Detects the language of the specified text.
	/// </summary>
	/// <param name="text">The text to analyze for language detection.</param>
	/// <returns>A response containing the detected language and confidence score.</returns>
	Task<Response<Detections>> DetectLanguageAsync(string text);

	/// <summary>
	/// Retrieves the list of available languages supported by the translation service.
	/// </summary>
	/// <returns>A response containing an array of language codes.</returns>
	Task<Response<string[]>> GetAvailableLanguagesAsync();

	/// <summary>
	/// Translates the contents of a file from a specified source language to a target language.
	/// </summary>
	/// <param name="fileStream">The stream representing the file to translate.</param>
	/// <param name="sourceLanguage">The language code of the source file.</param>
	/// <param name="targetLanguage">The language code to translate the file into.</param>
	/// <param name="fileName">The name of the file being translated.</param>
	/// <returns>A response containing the result of the file translation, including the URL of the translated file.</returns>
	Task<Response<TranslateFileResult>> TranslateFileAsync(Stream fileStream, string sourceLanguage, string targetLanguage, string fileName);

	/// <summary>
	/// Translates the contents of a file from any detected language to a specified target language.
	/// </summary>
	/// <param name="fileStream">The stream representing the file to translate.</param>
	/// <param name="targetLanguage">The language code to translate the file into.</param>
	/// <param name="fileName">The name of the file being translated.</param>
	/// <returns>A response containing the result of the file translation, including the URL of the translated file.</returns>
	Task<Response<TranslateFileResult>> TranslateFileFromAnyLanguageAsync(Stream fileStream, string targetLanguage, string fileName);

	/// <summary>
	/// Translates the specified text from a source language to a target language.
	/// </summary>
	/// <param name="text">The text to translate.</param>
	/// <param name="sourceLanguage">The language code of the source text.</param>
	/// <param name="targetLanguage">The language code to translate the text into.</param>
	/// <returns>A response containing the translation result.</returns>
	Task<Response<TranslateResult>> TranslateTextAsync(string text, string sourceLanguage, string targetLanguage);

	/// <summary>
	/// Translates the specified text from any detected language to a target language.
	/// </summary>
	/// <param name="text">The text to translate.</param>
	/// <param name="targetLanguage">The language code to translate the text into.</param>
	/// <returns>A response containing the translation result.</returns>
	Task<Response<TranslateResult>> TranslateTextFromAnyLanugageAsync(string text, string targetLanguage);
}