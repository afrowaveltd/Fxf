using Fxf.Blazor.Components;
using Fxf.Blazor.Components.Account;
using Fxf.Blazor.Data;
using Fxf.Blazor.I18n;
using Fxf.Blazor.Middlewares;
using Fxf.Blazor.Services;
using Fxf.Blazor.Services.LibreTranslate;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
	// Set property naming policy to camelCase
	options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

	// Allow complex object types like Lists<T> or other nested members
	options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

	// Add support for preserving references if needed (useful for circular references)
	options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

	// Customize any other settings as needed (e.g., number or date handling)
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
	options.ForwardedHeaders =
	ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddOpenApi("FxF");
// Add services to the container.
builder.Services.AddRazorComponents()
	 .AddInteractiveServerComponents()
	 .AddInteractiveWebAssemblyComponents()
	 .AddAuthenticationStateSerialization();

builder.Services.AddControllers()
			 .AddJsonOptions(options =>
			 {
				 // Set property naming policy to camelCase
				 options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

				 // Allow Lists and nested objects
				 options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;

				 // Handle circular references if applicable
				 options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
			 })
			 .AddXmlDataContractSerializerFormatters();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery(options =>
	 {
		 options.HeaderName = "X-XSRF-TOKEN";
		 options.Cookie.Name = "XSRF-TOKEN";
		 options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
		 options.Cookie.SameSite = SameSiteMode.Strict;
		 options.Cookie.HttpOnly = false;
	 });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddLocalization();
builder.Services.AddCascadingAuthenticationState();

// middlewares
builder.Services.AddTransient<LocalizationMiddleware>();

// Transient services
builder.Services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();

// Scoped services
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<ICookieService, CookieService>();

// Singleton services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<ILibreTranslateService, LibreTranslateService>();
builder.Services.AddSingleton<ILanguageService, LanguageService>();
// Worker services

// Identity configuration
builder.Services.AddAuthentication(options =>
	 {
		 options.DefaultScheme = IdentityConstants.ApplicationScheme;
		 options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
	 })
	 .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	 options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
	 {
		 options.SignIn.RequireConfirmedAccount = true;
		 options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
	 })
	 .AddEntityFrameworkStores<ApplicationDbContext>()
	 .AddSignInManager()
	 .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
string[] supportedCultures = ["en"];
ILanguageService languageService = app.Services.GetRequiredService<ILanguageService>();
if(languageService != null)
{
	supportedCultures = languageService.TranslationsPresented();
}

app.UseMiddleware<LocalizationMiddleware>();
app.UseRequestLocalization(options =>
{
	options.AddSupportedCultures(supportedCultures)
		 .AddSupportedUICultures(supportedCultures)
		 .SetDefaultCulture("en")
		 .ApplyCurrentCultureToResponseHeaders = true;
});

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);

app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.MapOpenApi()
		  .CacheOutput();
app.MapScalarApiReference(options =>
{
	_ = options
		 .WithTitle("F@F Open API Explorer")
		 .WithTheme(ScalarTheme.Mars)
		 .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	 .AddInteractiveServerRenderMode()
	 .AddInteractiveWebAssemblyRenderMode()
	 .AddAdditionalAssemblies(typeof(Fxf.Blazor.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();
app.MapControllers();
// Here we add starting procedures for localization.

app.Run();