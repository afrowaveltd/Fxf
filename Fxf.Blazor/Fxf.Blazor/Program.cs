using Fxf.Blazor.Components;
using Fxf.Blazor.Components.Account;
using Fxf.Blazor.Data;
using Fxf.Blazor.I18n;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	 .AddInteractiveServerComponents()
	 .AddInteractiveWebAssemblyComponents()
	 .AddAuthenticationStateSerialization();

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

builder.Services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();

builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

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
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	 .AddInteractiveServerRenderMode()
	 .AddInteractiveWebAssemblyRenderMode()
	 .AddAdditionalAssemblies(typeof(Fxf.Blazor.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();