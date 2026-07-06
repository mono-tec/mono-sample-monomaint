using MonoMaint.Abstractions;

namespace MonoMaint.Host.Components.Icons;

/// <summary>
/// PluginIcon を SVG 文字列へ変換します。
/// </summary>
public static class PluginIconRenderer
{
        private static readonly IReadOnlyDictionary<PluginIcon, string> Icons =
        new Dictionary<PluginIcon, string>
        {
        { PluginIcon.Plugin, PluginIconPlugin.Svg },
        { PluginIcon.DiskMonitor, PluginIconDiskMonitor.Svg },
        { PluginIcon.LogViewer, PluginIconLogViewer.Svg },
        { PluginIcon.HealthDashboard, PluginIconHealthDashboard.Svg },
        { PluginIcon.Settings, PluginIconSettings.Svg },
        };

    /// <summary>
    /// SVG文字列を取得します。
    /// </summary>
    public static string GetSvg(PluginIcon icon)
    {
        return Icons.TryGetValue(icon, out var svg)
            ? svg
            : PluginIconPlugin.Svg;
    }

}