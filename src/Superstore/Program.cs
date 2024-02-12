using Superstore.Components;
using Syncfusion.Blazor;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Superstore.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSyncfusionBlazor();
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ISuperstoreCache, SuperstoreCache>();
builder.Services.AddScoped<ICsvDataService, CsvDataService>();
var keyVaultEndpoint = builder.Configuration["KeyVault:BaseUrl"];
if (!string.IsNullOrEmpty(keyVaultEndpoint))
{
	var clientId = builder.Configuration["AzureAd:ClientId"];
	var clientSecret = builder.Configuration["AzureAd:ClientSecret"];
	var tenantId = builder.Configuration["AzureAd:TenantId"];
	var secretClient = new SecretClient(new Uri(keyVaultEndpoint), new ClientSecretCredential(tenantId, clientId, clientSecret));
	var syncfSecretKey = secretClient.GetSecret("syncf").Value.Value;
	if (syncfSecretKey != null) Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfSecretKey);

}
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
