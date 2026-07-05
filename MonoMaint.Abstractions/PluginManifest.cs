using System;
using System.Collections.Generic;
using System.Text;

namespace MonoMaint.Abstractions
{
    /// <summary>
    /// MonoMaint プラグインのマニフェストを表します。
    /// </summary>
    /// <remarks>
    /// プラグインの識別情報や表示情報など、Host がプラグインを
    /// 管理・表示するために必要なメタデータを保持します。
    /// 本クラスはイミュータブルオブジェクトとして record を採用しています。
    /// </remarks>
    /// <param name="Id">
    /// プラグインを一意に識別するIDです。
    /// </param>
    /// <param name="Name">
    /// プラグイン名です。
    /// </param>
    /// <param name="Description">
    /// プラグインの説明です。
    /// </param>
    /// <param name="Version">
    /// プラグインのバージョンです。
    /// </param>
    public sealed record PluginManifest(
        string Id,
        string Name,
        string Description,
        string Version);   
}
