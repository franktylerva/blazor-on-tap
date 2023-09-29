using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WasmHosted.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

RegisterHttpClient(builder);

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.MetadataUrl = "https://unsafe-test-login.appsso.tapv-stirred-caribou.tapsandbox.com/.well-known/openid-configuration";
    options.ProviderOptions.Authority = "https://unsafe-test-login.appsso.tapv-stirred-caribou.tapsandbox.com";
    options.ProviderOptions.ClientId = "apps_blazor-on-tap-sso-pw84x-m5xr2";
    options.ProviderOptions.ResponseType = "id_token token";

    options.UserOptions.NameClaim = "preferred_username";
    options.UserOptions.RoleClaim = "roles";
    options.UserOptions.ScopeClaim = "scope";
});

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();

static void RegisterHttpClient(
    WebAssemblyHostBuilder builder)
{
    var httpClientName = "Default";

    builder.Services.AddHttpClient(httpClientName,
            client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
        .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddScoped(
        sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient(httpClientName));
}