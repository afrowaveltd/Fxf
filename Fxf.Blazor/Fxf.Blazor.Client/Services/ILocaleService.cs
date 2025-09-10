namespace Fxf.Blazor.Client.Services;

// Services/ILocaleService.cs
using System.Threading.Tasks;

public interface ILocaleService
{
	Task<string[]> GetBrowserLocalesAsync();

	Task<string?> GetPreferredCultureAsync();

	Task SavePreferredCultureAsync(string culture);

	Task<string?> GetTimeZoneAsync();

	Task ApplyCultureAsync(string culture, bool persist = true);
}