namespace Fxf.Blazor.Client.Services;

// Services/ILocaleService.cs
using System.Threading.Tasks;

public interface ILocaleService
{
	Task ApplyCultureAsync(string culture, bool persist = true);

	Task<string[]> GetBrowserLocalesAsync();

	Task<string?> GetPreferredCultureAsync();

	Task<string?> GetTimeZoneAsync();

	Task SavePreferredCultureAsync(string culture);
}