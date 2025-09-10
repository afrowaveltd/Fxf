using Fxf.Blazor.Client.Services;
using Fxf.Blazor.Client.StaticClasses;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();

// Singleton services

builder.Services.AddSingleton<IApiClientService, ApiClientService>();

// Scoped services
builder.Services.AddScoped<ILocaleService, LocaleService>();

// Transient services

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

var app = builder.Build();

// Load locales

string[] locales;
string[] parts;
string clientLanguage = string.Empty;
var localeSvc = app.Services.GetRequiredService<ILocaleService>();
var saved = await localeSvc.GetPreferredCultureAsync();
if(!string.IsNullOrWhiteSpace(saved))
{
	await localeSvc.ApplyCultureAsync(saved!, persist: false);
	Console.WriteLine("Language loaded: " + saved);
	clientLanguage = saved!;
}
else
{
	locales = await localeSvc.GetBrowserLocalesAsync();

	string pick = locales?.Length > 0 ? locales[0] : "en";
	parts = pick.Split("-");
	clientLanguage = parts[0];
	await localeSvc.ApplyCultureAsync(parts[0], persist: false);
}
Console.WriteLine("Language selected: " + clientLanguage);

var localizationService = app.Services.GetRequiredService<IApiClientService>();

LocaleDictionary.Locales = await localizationService.GetClientDictionary(saved ?? "en");
foreach(var line in LocaleDictionary.Locales)
	Console.WriteLine($"{line.Key} - {line.Value}");
await app.RunAsync();