
namespace Fxf.Blazor.Services;

public interface ISelectOptionsService
{
	List<SelectListItem> GetThemes(string actualThemeName);
}