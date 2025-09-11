using Fxf.Blazor.Client.Handlers;
using Fxf.Blazor.Client.I18n;
using Fxf.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();

// Program.cs (Blazor WASM)
builder.Services.AddTransient(sp => new AcceptLanguageHandler(
	 () => CultureInfo.CurrentUICulture.Name // napø. "cs-CZ" nebo "cs"
));

builder.Services.AddHttpClient<IApiClientService, ApiClientService>(client =>
{
	client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress.TrimEnd('/') + "/api");
})
.AddHttpMessageHandler<AcceptLanguageHandler>();

// Singleton services
builder.Services.AddSingleton<IApiClientService, ApiClientService>();
builder.Services.AddSingleton<ILocaleService, LocaleService>();

// Scoped services

// Transient services
builder.Services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

var app = builder.Build();

// Load locales

var localeSvc = app.Services.GetRequiredService<ILocaleService>();
var saved = await localeSvc.GetPreferredCultureAsync();

string clientLanguage;
if(!string.IsNullOrWhiteSpace(saved))
{
	await localeSvc.ApplyCultureAsync(saved!, persist: false);
	clientLanguage = saved!;
}
else
{
	var locales = await localeSvc.GetBrowserLocalesAsync();
	var pick = locales?.Length > 0 ? locales[0] : "en";
	var code = pick.Split('-')[0]; // "cs-CZ" -> "cs"
	await localeSvc.ApplyCultureAsync(code, persist: false);
	clientLanguage = code;
}

// OPTIONAL: nastav i cookie, pokud chceš primárnì cookie flow
// await localeSvc.SaveCookieCultureAsync(clientLanguage);

// Pøednaèti dictionary pro klienta (rychlejší první render)
var api = app.Services.GetRequiredService<IApiClientService>();
await api.LoadDictionary(clientLanguage, isClient: true);

await app.RunAsync();