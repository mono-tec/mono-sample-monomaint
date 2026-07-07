using MonoMaint.Core;
using MonoMaint.Host.Sample.Components;
using MonoMaint.Plugin.DiskMonitor;
using MonoMaint.Plugin.HealthDashboard;
using MonoMaint.Plugin.LogViewer;
using MonoMaint.Plugin.Sample;
using MonoMaint.Plugin.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDiskMonitorPlugin();
builder.Services.AddSingleton<PluginRegistry>(_ =>
{
    var registry = new PluginRegistry();

    var plugins = PluginDiscovery.Discover(
        typeof(SamplePlugin).Assembly,
        typeof(DiskMonitorPlugin).Assembly,
        typeof(LogViewerPlugin).Assembly,
        typeof(HealthDashboardPlugin).Assembly,
        typeof(SettingsPlugin).Assembly);

    foreach (var plugin in plugins)
    {
        registry.Add(plugin);
    }

    return registry;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(
        typeof(SamplePlugin).Assembly,
        typeof(DiskMonitorPlugin).Assembly,
        typeof(LogViewerPlugin).Assembly,
        typeof(HealthDashboardPlugin).Assembly,
        typeof(SettingsPlugin).Assembly);

app.Run();
