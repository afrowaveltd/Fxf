using Fxf.Blazor.Client.Handlers;
using Fxf.Blazor.Client.I18n;
using Fxf.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();

// Register AcceptLanguageHandler
builder.Services.AddTransient(sp => new AcceptLanguageHandler(
    () => CultureInfo.CurrentUICulture.Name
));

// Build a temporary provider to get NavigationManager for constructing HttpClient base address
var tempProvider = builder.Services.BuildServiceProvider();
var navigation = tempProvider.GetRequiredService<NavigationManager>();

// Use site root as HttpClient BaseAddress; include full API path in service (avoids leading slash
// reset issues)
builder.Services.AddHttpClient<IApiClientService, ApiClientService>(client =>
{
   client.BaseAddress = new Uri(navigation.BaseUri); // e.g. https://localhost:5001/
})
.AddHttpMessageHandler<AcceptLanguageHandler>();

// Singleton services
builder.Services.AddSingleton<ILocaleService, LocaleService>();

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
   var code = pick.Split('-')[0]; // e.g. "cs-CZ" -> "cs"
   await localeSvc.ApplyCultureAsync(code, persist: false);
   clientLanguage = code;
}

// Preload dictionary for client for faster first render
try
{
   var api = app.Services.GetRequiredService<IApiClientService>();
   await api.LoadDictionary(clientLanguage, isClient: true);
   Console.WriteLine($"Locales preloaded for {clientLanguage}.");
}
catch(Exception ex)
{
   Console.WriteLine("Preload locales failed: " + ex);
}

await app.RunAsync();