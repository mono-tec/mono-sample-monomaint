using MonoMaint.Abstractions;

namespace MonoMaint.Plugin.Settings;

/// <summary>
/// プラグインのマニフェストを取得します。
/// </summary>
public sealed class SettingsPlugin : PluginBase<SettingsPlugin>
{
    protected override string Name => "Settings";

    protected override string Description =>
        "Provides application settings.";

    protected override PluginIcon Icon => PluginIcon.Settings;
}