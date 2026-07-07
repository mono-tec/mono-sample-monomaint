using MonoMaint.Plugin.DiskMonitor.Models;

namespace MonoMaint.Plugin.DiskMonitor.Services;

/// <summary>
/// ディスク情報取得サービスです。
/// </summary>
public interface IDiskInfoService
{
    /// <summary>
    /// ディスク情報一覧を取得します。
    /// </summary>
    IReadOnlyList<DiskInfo> GetDisks();
}