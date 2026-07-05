using MonoMaint.Abstractions;

namespace MonoMaint.Plugin.Sample
{
    /// <summary>
    /// MonoMaint サンプルプラグインです。
    /// </summary>
    /// <remarks>
    /// Plugin SDK の実装例として使用します。
    /// </remarks>
    public sealed class SamplePlugin : IMonoMaintPlugin
    {
        /// <summary>
        /// プラグインのマニフェストを取得します。
        /// </summary>
        public PluginManifest Manifest { get; } =
            new(
                Id: "sample",
                Name: "Sample Plugin",
                Description: "MonoMaint Plugin SDK sample.",
                Version: "0.1.0");
    }
}
