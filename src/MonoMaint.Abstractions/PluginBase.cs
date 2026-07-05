namespace MonoMaint.Abstractions;

/// <summary>
/// MonoMaint プラグインの基底クラスです。
/// </summary>
/// <typeparam name="TPlugin">
/// プラグイン実装クラスの型です。
/// </typeparam>
public abstract class PluginBase<TPlugin> : IMonoMaintPlugin
{
    /// <summary>
    /// プラグイン名です。
    /// </summary>
    protected abstract string Name { get; }

    /// <summary>
    /// プラグインの説明です。
    /// </summary>
    protected abstract string Description { get; }

    /// <summary>
    /// プラグインのアイコンです。
    /// </summary>
    protected abstract PluginIcon Icon { get; }

    /// <summary>
    /// プラグインのマニフェストを取得します。
    /// </summary>
    public PluginManifest Manifest =>
        PluginManifest.Create<TPlugin>(
            name: Name,
            description: Description,
            icon: Icon);
}