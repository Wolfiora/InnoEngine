using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Build.TaskHosting;

/// <summary>
/// Retires completed task hosts after every registered process owner has exited.
/// </summary>
public sealed partial class PublishTaskHostTask
{
    private static void RetireReaders(
        string root,
        string currentSnapshot
    ) {
        string loads = Path.Combine(root, "loads");
        string hosts = Path.Combine(root, "hosts");
        foreach (string candidate in Directory.EnumerateDirectories(loads).ToArray())
        {
            try
            {
                string owners = Path.Combine(candidate, "owners");
                string[] markers = Directory.Exists(owners) ? Directory.GetFiles(owners, "*.pid") : [];
                if (markers.Length == 0 || markers.Any(IsOwnerAlive))
                    continue;
                string[] sources = Directory.GetFiles(candidate, "source-host.txt", SearchOption.AllDirectories)
                    .Select(File.ReadAllText).Select(Path.GetFullPath).ToArray();
                PathBoundary.RequireUnlinkedPath(loads, candidate);
                Directory.Delete(candidate, recursive: true);
                foreach (string source in sources)
                {
                    string snapshot = Path.GetDirectoryName(Path.GetDirectoryName(source))!;
                    if (snapshot == currentSnapshot || !Directory.Exists(snapshot))
                        continue;
                    PathBoundary.RequireUnlinkedPath(hosts, snapshot);
                    using FileLease ownership = FileLease.AcquireAsync(snapshot + ".lock", TimeSpan.Zero)
                        .AsTask().GetAwaiter().GetResult();
                    if (Directory.EnumerateFiles(loads, "source-host.txt", SearchOption.AllDirectories)
                        .Any(marker => Path.GetFullPath(File.ReadAllText(marker)) == source))
                        continue;
                    Directory.Delete(snapshot, recursive: true);
                }
            }
            catch (IOException)
            {
                // A remaining loader pin or another publisher delays retirement until a later build.
            }
            catch (UnauthorizedAccessException)
            {
                // Retirement never invalidates the already published current runtime.
            }
            catch (TimeoutException)
            {
                // A publisher still owns this snapshot; its registered readers will be checked later.
            }
            catch (ArgumentException)
            {
                // A damaged owner record does not grant permission to remove its referenced location.
            }
        }
        RetirePublishers(root);
    }

    private static void RetirePublishers(string root)
    {
        string publishers = Path.Combine(root, "publishers");
        if (!Directory.Exists(publishers))
            return;
        foreach (string candidate in Directory.EnumerateDirectories(publishers).ToArray())
        {
            try
            {
                string owners = Path.Combine(candidate, "owners");
                string[] markers = Directory.Exists(owners) ? Directory.GetFiles(owners, "*.pid") : [];
                if (markers.Length == 0 || markers.Any(IsOwnerAlive))
                    continue;
                PathBoundary.RequireUnlinkedPath(publishers, candidate);
                Directory.Delete(candidate, recursive: true);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }

    private static bool IsOwnerAlive(string marker)
    {
        if (!int.TryParse(File.ReadAllText(marker), out int processId))
            return true;
        try
        {
            using Process process = Process.GetProcessById(processId);
            return !process.HasExited;
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
        catch (Win32Exception)
        {
            return true;
        }
    }
}
