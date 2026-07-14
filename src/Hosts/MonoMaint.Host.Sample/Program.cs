using MonoMaint.Core;
using MonoMaint.Host.Sample.Components;
using MonoMaint.Plugin.DiskMonitor;
using MonoMaint.Plugin.HealthDashboard;
using MonoMaint.Plugin.LogViewer;
using MonoMaint.Plugin.Sample;
using MonoMaint.Plugin.Settings;
using Serilog;

///////////////////////////////////////////////////////////////////////////////
// MonoMaint Sample Host
//
// Hostの責務
//   ・DI登録
//   ・Plugin登録
//   ・Logging設定
//   ・Blazor Host構成
//
// PluginはILogger<T>のみ利用し、
// Serilog等のLoggingライブラリには依存しません。
///////////////////////////////////////////////////////////////////////////////

var builder = WebApplication.CreateBuilder(args);

///////////////////////////////////////////////////////////////////////////////
// Logging
//
// HostでSerilogを構成します。
// PluginはILogger<T>のみ利用することで、
//
//  ・Console
//  ・File
//  ・Windows EventLog
//  ・journalctl
//  ・Application Insights
//
// などへHost側だけで切り替え可能になります。
///////////////////////////////////////////////////////////////////////////////

builder.Host.UseSerilog((context, services, logger) =>
{
    logger
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

///////////////////////////////////////////////////////////////////////////////
// Razor Components
///////////////////////////////////////////////////////////////////////////////

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

///////////////////////////////////////////////////////////////////////////////
// Plugin Services
//
// Pluginが必要とするDIを登録します。
///////////////////////////////////////////////////////////////////////////////

builder.Services.AddDiskMonitorPlugin();

///////////////////////////////////////////////////////////////////////////////
// Plugin Discovery
//
// Hostに組み込むPluginを検索し、PluginRegistryへ登録します。
//
// 将来的には
//   ・DLL配置
//   ・設定ファイル
//   ・AssemblyLoadContext
//
// などへ置き換え可能です。
///////////////////////////////////////////////////////////////////////////////

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

///////////////////////////////////////////////////////////////////////////////
// Build Host
///////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

///////////////////////////////////////////////////////////////////////////////
// Output Host Information
///////////////////////////////////////////////////////////////////////////////
var logger = app.Services.GetRequiredService<ILogger<Program>>();
var registry = app.Services.GetRequiredService<PluginRegistry>();

logger.LogInformation(
    "MonoMaint Host started. Environment: {Environment}",
    app.Environment.EnvironmentName);

var plugins = registry.Plugins;

if (plugins.Count == 0)
{
    logger.LogWarning("No plugins are registered.");
}
else
{
    logger.LogInformation(
        "{PluginCount} plugin(s) are registered.",
        plugins.Count);

    foreach (var plugin in plugins)
    {
        logger.LogInformation(
            "Plugin enabled: Id={PluginId}, Name={PluginName}, Version={PluginVersion}, Route={PluginRoute}",
            plugin.Manifest.Id,
            plugin.Manifest.Name,
            plugin.Manifest.Version,
            plugin.Manifest.Route);
    }
}

///////////////////////////////////////////////////////////////////////////////
// HTTP Request Pipeline
///////////////////////////////////////////////////////////////////////////////

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // HSTSは本番環境のみ有効
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

///////////////////////////////////////////////////////////////////////////////
// Blazor Routing
//
// Hostが読み込むPlugin Assemblyを追加します。
///////////////////////////////////////////////////////////////////////////////

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(
        typeof(SamplePlugin).Assembly,
        typeof(DiskMonitorPlugin).Assembly,
        typeof(LogViewerPlugin).Assembly,
        typeof(HealthDashboardPlugin).Assembly,
        typeof(SettingsPlugin).Assembly);

///////////////////////////////////////////////////////////////////////////////
// Run
///////////////////////////////////////////////////////////////////////////////

app.Run();