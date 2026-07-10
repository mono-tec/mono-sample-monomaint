using MonoMaint.Plugin.DiskMonitor.Models;

namespace MonoMaint.Plugin.DiskMonitor.Services;

/// <summary>
/// Linux環境のディスク情報取得サービスです。
/// </summary>
public sealed class LinuxDiskInfoService : IDiskInfoService
{
    private static readonly string[] ExcludedMountPrefixes =
    [
        "/proc",
        "/sys",
        "/dev",
        "/run",
        "/etc",
        "/opt/render-ssh"
    ];

    /// <inheritdoc />
    public IReadOnlyList<DiskInfo> GetDisks()
    {
        var disks = new List<DiskInfo>();

        foreach (var drive in DriveInfo.GetDrives())
        {
            try
            {
                if (!drive.IsReady)
                {
                    continue;
                }

                if (drive.TotalSize <= 0)
                {
                    continue;
                }

                if (IsExcludedMount(drive.Name))
                {
                    continue;
                }

                disks.Add(new DiskInfo
                {
                    Name = drive.Name,
                    TotalSize = drive.TotalSize,
                    FreeSpace = drive.AvailableFreeSpace
                });
            }
            catch
            {
                // Docker/Linux環境では一部の仮想マウントで例外が出る場合があるため除外する。
            }
        }

        return disks;
    }

    private static bool IsExcludedMount(string mountName)
    {
        return ExcludedMountPrefixes.Any(prefix =>
            mountName.Equals(prefix, StringComparison.OrdinalIgnoreCase) ||
            mountName.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase));
    }
}