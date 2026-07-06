using System;
using System.Collections.Generic;
using System.Text;

namespace MonoMaint.Abstractions
{
    /// <summary>
    /// MonoMaint プラグインの基本インターフェースです。
    /// </summary>
    /// <remarks>
    /// すべてのプラグインは本インターフェースを実装します。
    /// Host は本インターフェースを通じてプラグインのマニフェストを取得し、
    /// メニュー生成や画面表示などを行います。
    /// </remarks>
    public interface IMonoMaintPlugin
    {
        /// <summary>
        /// プラグインのマニフェストを取得します。
        /// </summary>
        PluginManifest Manifest { get; }
    }
}
