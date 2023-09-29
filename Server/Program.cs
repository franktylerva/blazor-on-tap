using Keycloak.AuthServices.Authentication;
using WasmHosted.Shared;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var host = builder.Host;
host.ConfigureKeycloakConfigurationSource("keycloak.json");

// Add services to the container.
var services = builder.Services;

services.AddAuthentication().AddJwtBearer(o =>
{
    DotnetServiceBinding sc = new DotnetServiceBinding();
    Dictionary<string, string> oauth2Bindings = sc.GetBindings("oauth2");
    
    o.MetadataAddress = oauth2Bindings["issuer-uri"] + "/.well-known/openid-configuration";
    o.Authority = oauth2Bindings["issuer-uri"];
    o.Audience = oauth2Bindings["client-id"];;
    o.RequireHttpsMetadata = false;
});


// services.AddKeycloakAuthentication(configuration);
services.AddAuthorization();

services.AddControllersWithViews();
services.AddRazorPages();

services.AddEndpointsApiExplorer();

services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Auth",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OpenIdConnect,
        OpenIdConnectUrl = new Uri("http://localhost:8080/realms/test/.well-known/openid-configuration"),
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, Array.Empty<string>()}
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseSwagger().UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
