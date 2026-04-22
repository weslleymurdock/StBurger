using StBurger.App.Handlers; 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.ConfigureStBurgerServices(builder.Configuration);

var app = builder.Build();
 
app.UseMiddleware<HttpErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
