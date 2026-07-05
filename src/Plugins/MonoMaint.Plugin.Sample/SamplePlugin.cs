using MonoMaint.Abstractions;

namespace MonoMaint.Plugin.Sample;

/// <summary>
/// プラグインのマニフェストを取得します。
/// </summary>
public sealed class SamplePlugin : PluginBase<SamplePlugin>
{
    protected override string Name => "Sample Plugin";

    protected override string Description =>
        "MonoMaint Plugin SDK sample.";

    protected override PluginIcon Icon => PluginIcon.Plugin;
}