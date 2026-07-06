using System.Reflection;

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
       PluginIcon Icon)
    {
        /// <summary>
        /// プラグイン型情報から ID、Route、Version を自動生成して
        /// PluginManifest を作成します。
        /// </summary>
        /// <typeparam name="TPlugin">プラグイン型です。</typeparam>
        /// <param name="name">プラグイン表示名です。</param>
        /// <param name="description">プラグイン説明です。</param>
        /// <param name="icon">プラグインアイコンです。</param>
        /// <returns>PluginManifest を返します。</returns>
        public static PluginManifest Create<TPlugin>(
            string name,
            string description,
            PluginIcon icon)
        {
            var assembly = typeof(TPlugin).Assembly;

            var id = CreatePluginId(assembly);
            var route = $"/plugins/{id}";
            var version = GetDisplayVersion(assembly);

            return new PluginManifest(
                Id: id,
                Name: name,
                Description: description,
                Version: version,
                Route: route,
                Icon: icon);
        }

        /// <summary>
        /// Assembly名からプラグインIDを生成します。
        /// </summary>
        private static string CreatePluginId(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name ?? string.Empty;

            var pluginName = assemblyName
                .Replace("MonoMaint.Plugin.", string.Empty)
                .Replace("Plugin", string.Empty);

            return ToKebabCase(pluginName);
        }

        /// <summary>
        /// 表示用バージョンを取得します。
        /// </summary>
        private static string GetDisplayVersion(Assembly assembly)
        {
            var version =
                assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    ?.InformationalVersion
                ?? assembly.GetName().Version?.ToString(3)
                ?? "0.1.0";

            return version.Split('+')[0];
        }

        /// <summary>
        /// PascalCase文字列を kebab-case へ変換します。
        /// </summary>
        private static string ToKebabCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "unknown";
            }

            var chars = new List<char>();

            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];

                if (char.IsUpper(c) && i > 0)
                {
                    chars.Add('-');
                }

                chars.Add(char.ToLowerInvariant(c));
            }

            return new string(chars.ToArray());
        }
    }
}