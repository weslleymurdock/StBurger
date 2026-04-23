using Microsoft.AspNetCore.Http.HttpResults;
using StBurger.App.Filters;
using StBurger.Infrastructure.Handlers;
var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
{
    // User secrets only outside docker containers and at development environment
    builder.Configuration.AddUserSecrets<Program>();
}
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddScalarTransformers();
    options.AddOperationTransformer<DefaultContentTypeOperationTransformer>();
});

builder.Services.ConfigureStBurgerServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.Use(async (context, next) =>
{
    var accept = context.Request.Headers.Accept.ToString();

    if (string.IsNullOrWhiteSpace(accept) ||
        accept.Contains("text/plain", StringComparison.OrdinalIgnoreCase))
    {
        context.Request.Headers.Accept = "application/json";
    }

    await next();
});

app.UseExceptionHandler();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar/stburger", options => {
        options.WithTitle("StBurger RESTful API Documentation")
           .WithTheme(ScalarTheme.BluePlanet)
           .WithDotNetFlag(true)
           .WithDocumentDownloadType(DocumentDownloadType.Json)
           .ForceDarkMode()
           .ShowOperationId()
           .ExpandAllTags()
           .SortTagsAlphabetically()
           .SortOperationsByMethod()
           .WithSearchHotKey("s")
           .PreserveSchemaPropertyOrder();
    });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
 

await app.MigrateDatabaseAsync();

if (!app.Environment.IsProduction())
{
    await app.SeedDatabaseAsync();
}

await app.RunAsync();
