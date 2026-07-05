using MonoMaint.Abstractions;

namespace MonoMaint.Plugin.HealthDashboard;

/// <summary>
/// プラグインのマニフェストを取得します。
/// </summary>
public sealed class HealthDashboardPlugin : PluginBase<HealthDashboardPlugin>
{
    protected override string Name => "Health Dashboard";

    protected override string Description =>
        "Displays system health and service status.";

    protected override PluginIcon Icon => PluginIcon.HealthDashboard;
}



