using Scalar.AspNetCore;

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
             options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

             // Handle circular references if applicable
             options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
          })
          .AddXmlDataContractSerializerFormatters();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery(options =>
{
   options.HeaderName = "X-XSRF-TOKEN";
   options.Cookie.Name = "XSRF-TOKEN";
   options.Cookie.SecurePolicy = CookieSecurePolicy.None;
   options.Cookie.SameSite = SameSiteMode.Strict;
   options.Cookie.HttpOnly = true;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddLocalization();
builder.Services.AddCascadingAuthenticationState();

// Authorization
builder.Services.AddAuthorization(options =>
{
   options.AddPolicy("RequireAdministrator", policy => policy.RequireRole("Administrator"));
   options.AddPolicy("RequireUser", policy => policy.RequireRole("User"));
   options.AddPolicy("RequireTranslator", policy => policy.RequireRole("Translator"));
   options.AddPolicy("RequireOwner", policy => policy.RequireRole("Owner"));
});

// middlewares
builder.Services.AddTransient<LocalizationMiddleware>();

// Singleton services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<ILibreTranslateService, LibreTranslateService>();

// Scoped services
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<ICookieService, CookieService>();
builder.Services.AddScoped<IHubActivityLogger, HubActivityLogger>();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddSingleton<ILanguageService, LanguageService>();

// Transient services
builder.Services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddTransient<ISelectOptionsService, SelectOptionsService>();

// Worker services

// Identity configuration
/*builder.Services.AddAuthentication(options =>
{
   options.DefaultScheme = IdentityConstants.ApplicationScheme;
   options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();
*/
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
   options.SignIn.RequireConfirmedAccount = true;
   options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddSignalR()
    .AddJsonProtocol(o =>
    {
       // sladění se System.Text.Json nastavením v projektu
       o.PayloadSerializerOptions.PropertyNamingPolicy = null; // nechá PascalCase pokud používáš
    });
builder.Services.AddMemoryCache();

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

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();
app.UseForwardedHeaders();

// Důležité: Přidání autentizace a autorizace
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi()
        .CacheOutput();
app.MapScalarApiReference(options =>
{
   _ = options
       .WithTitle("F x F Open API Explorer")
       .WithTheme(ScalarTheme.Mars)
       .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseAntiforgery();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Fxf.Blazor.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components. app.UseRouting();
app.MapAdditionalIdentityEndpoints();
app.MapControllers();

// Here we add starting procedures for localization.
app.MapHub<LocalizationHub>("/localization_hub");
app.MapHub<WorkerHub>("/worker_hub");
app.MapHub<IndexHub>("/index_hub");

// Seed výchozích rolí
using(var scope = app.Services.CreateScope())
{
   var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
   var roles = new[] { "Administrator", "User", "Translator", "Owner" };

   foreach(var role in roles)
   {
      if(!await roleManager.RoleExistsAsync(role))
      {
         await roleManager.CreateAsync(new IdentityRole(role));
      }
   }
}

app.Run();