using MonoMaint.Abstractions;

namespace MonoMaint.Plugin.Sample
{
    /// <summary>
    /// MonoMaint のサンプルプラグインです。
    /// </summary>
    /// <remarks>
    /// Plugin SDK の実装例として利用します。
    /// 本プラグインは、PluginManifest・PluginRegistry・
    /// Plugin Menu の動作確認を目的としています。
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
                Version: "0.1.0",
                Route: "/plugins/sample",
                Icon: PluginIcon.Plugin);
    }
}
