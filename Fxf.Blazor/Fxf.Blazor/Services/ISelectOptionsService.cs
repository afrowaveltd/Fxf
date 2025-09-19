namespace Fxf.Blazor.Services;

public interface ISelectOptionsService
{
	List<SelectOption> GetThemes(string actualThemeName);
}