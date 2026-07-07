using MonoMaint.Plugin.DiskMonitor.Models;

namespace MonoMaint.Plugin.DiskMonitor.Services;

/// <summary>
/// Linux環境のディスク情報取得サービスです。
/// </summary>
public sealed class LinuxDiskInfoService : IDiskInfoService
{
    /// <inheritdoc />
    public IReadOnlyList<DiskInfo> GetDisks()
    {
        return DriveInfo.GetDrives()
            .Where(drive => drive.IsReady)
            .Select(drive => new DiskInfo
            {
                Name = drive.Name,
                TotalSize = drive.TotalSize,
                FreeSpace = drive.AvailableFreeSpace
            })
            .ToList();
    }
}