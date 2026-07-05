using MonoMaint.Abstractions;

namespace MonoMaint.Plugin.DiskMonitor;

/// <summary>
/// プラグインのマニフェストを取得します。
/// </summary>
public sealed class DiskMonitorPlugin : PluginBase<DiskMonitorPlugin>
{
    protected override string Name => "Disk Monitor";

    protected override string Description =>
        "Displays disk usage information.";

    protected override PluginIcon Icon => PluginIcon.DiskMonitor;
}