using Microsoft.Extensions.DependencyInjection;
using MonoMaint.Plugin.DiskMonitor.Services;

namespace MonoMaint.Plugin.DiskMonitor;

/// <summary>
/// Disk Monitor Plugin のサービス登録を提供します。
/// </summary>
public static class PluginRegistration
{
    /// <summary>
    /// Disk Monitor Plugin をサービスコレクションへ登録します。
    /// </summary>
    public static IServiceCollection AddDiskMonitorPlugin(
        this IServiceCollection services)
    {
        if (OperatingSystem.IsWindows())
        {
            services.AddSingleton<IDiskInfoService, WindowsDiskInfoService>();
        }
        else
        {
            services.AddSingleton<IDiskInfoService, LinuxDiskInfoService>();
        }

        return services;
    }
}