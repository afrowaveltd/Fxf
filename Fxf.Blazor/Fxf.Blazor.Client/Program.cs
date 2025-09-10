using Fxf.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();

// Singleton services
builder.Services.AddSingleton<IApiClientService, ApiClientService>();
builder.Services.AddSingleton<ILocaleService, LocaleService>();

// Scoped services

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
	clientLanguage = parts[0]!;
	await localeSvc.ApplyCultureAsync(parts[0], persist: false);
}
Console.WriteLine("Language selected: " + clientLanguage);


await app.RunAsync();