using MonoMaint.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoMaint.Core
{
    /// <summary>
    /// MonoMaint プラグインの登録情報を管理します。
    /// </summary>
    /// <remarks>
    /// 現時点では手動登録されたプラグインを保持します。
    /// 将来的には設定ファイルや外部アセンブリからの動的読み込みに拡張します。
    /// </remarks>
    public sealed class PluginRegistry
    {
        private readonly List<IMonoMaintPlugin> _plugins = [];

        /// <summary>
        /// 登録済みプラグイン一覧を取得します。
        /// </summary>
        public IReadOnlyList<IMonoMaintPlugin> Plugins => _plugins;

        /// <summary>
        /// プラグインを登録します。
        /// </summary>
        /// <param name="plugin">登録するプラグインです。</param>
        public void Add(IMonoMaintPlugin plugin)
        {
            ArgumentNullException.ThrowIfNull(plugin);
            _plugins.Add(plugin);
        }
    }
}
