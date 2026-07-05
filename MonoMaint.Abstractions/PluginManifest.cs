using System;
using System.Collections.Generic;
using System.Text;

namespace MonoMaint.Abstractions
{
    /// <summary>
    /// MonoMaint プラグインのマニフェストを表します。
    /// </summary>
    /// <remarks>
    /// Host がプラグインを識別・表示するための基本情報を保持します。
    /// 本クラスはイミュータブルオブジェクトとして実装しています。
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
    /// <param name="Route">
    /// プラグイン画面へ遷移するためのルートURLです。
    /// </param>
    /// <param name="Icon">
    /// プラグインのアイコンです。
    /// </param>
    public sealed record PluginManifest(
       string Id,
       string Name,
       string Description,
       string Version,
       string Route,
       PluginIcon Icon);
}
