using System.Reflection;
using MonoMaint.Abstractions;

namespace MonoMaint.Core;

/// <summary>
/// Plugin Assembly から MonoMaint Plugin を検出します。
/// </summary>
public static class PluginDiscovery
{
    /// <summary>
    /// 指定された Assembly から IMonoMaintPlugin 実装を検出します。
    /// </summary>
    /// <param name="assemblies">検索対象の Assembly 一覧です。</param>
    /// <returns>検出した Plugin 一覧を返します。</returns>
    public static IReadOnlyList<IMonoMaintPlugin> Discover(
        params Assembly[] assemblies)
    {
        var plugins = new List<IMonoMaintPlugin>();

        foreach (var assembly in assemblies)
        {
            var pluginTypes = assembly
                .GetTypes()
                .Where(type =>
                    typeof(IMonoMaintPlugin).IsAssignableFrom(type) &&
                    type is { IsAbstract: false, IsInterface: false } &&
                    type.GetConstructor(Type.EmptyTypes) is not null);

            foreach (var pluginType in pluginTypes)
            {
                if (Activator.CreateInstance(pluginType) is IMonoMaintPlugin plugin)
                {
                    plugins.Add(plugin);
                }
            }
        }

        return plugins;
    }
}