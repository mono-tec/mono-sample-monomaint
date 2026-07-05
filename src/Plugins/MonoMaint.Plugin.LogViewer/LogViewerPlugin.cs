using MonoMaint.Abstractions;

namespace MonoMaint.Plugin.LogViewer;

/// <summary>
/// Log Viewer プラグインです。
/// </summary>
public sealed class LogViewerPlugin : PluginBase<LogViewerPlugin>
{
    protected override string Name => "Log Viewer";

    protected override string Description =>
        "Displays application and maintenance logs.";

    protected override PluginIcon Icon => PluginIcon.LogViewer;
}