using MudBlazor.Services;
using Orders.Frontend.Components;
using Orders.Frontend.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("https://localhost:7184/") });
builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
