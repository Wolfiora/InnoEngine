using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using Inno.Core.IO;

namespace Inno.Build.Tasks;

internal static class BuildTaskHostRetirement
{
    private static readonly object Gate = new();
    private static readonly HashSet<string> InspectedRoots = new(StringComparer.Ordinal);

    internal static void Inspect(string engineRoot)
    {
        string root = Path.GetFullPath(Path.Combine(engineRoot, "artifacts", "build-tools", "tasks"));
        lock (Gate)
        {
            if (!InspectedRoots.Add(root))
                return;
            RegisterOwner(root);
            string loads = Path.Combine(root, "loads");
            if (Directory.Exists(loads))
            {
                foreach (string candidate in Directory.EnumerateDirectories(loads))
                    Retire(loads, candidate);
            }

        }
    }

    private static void RegisterOwner(string root)
    {
        string assembly = typeof(BuildTaskHostRetirement).Assembly.Location;
        string directory = Path.GetDirectoryName(assembly)!;
        string loads = Path.Combine(root, "loads");
        if (!directory.StartsWith(loads + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            return;
        string operation = Path.GetDirectoryName(directory)!;
        PathBoundary.RequireUnlinkedPath(loads, operation);
        string process = Environment.ProcessId.ToString(System.Globalization.CultureInfo.InvariantCulture);
        string owners = Path.Combine(operation, "owners");
        Directory.CreateDirectory(owners);
        AtomicFile.WriteAllBytes(Path.Combine(owners, process + ".pid"), Encoding.UTF8.GetBytes(process));

    }

    private static void Retire(
        string boundary,
        string candidate
    ) {
        try
        {
            string owners = Path.Combine(candidate, "owners");
            if (!Directory.Exists(owners))
                return;
            string[] markers = Directory.GetFiles(owners, "*.pid");
            if (markers.Length == 0)
                return;
            foreach (string marker in markers)
            {
                if (!int.TryParse(File.ReadAllText(marker), out int processId) || IsProcessAlive(processId))
                    return;
            }
            PathBoundary.RequireUnlinkedPath(boundary, candidate);
            Directory.Delete(candidate, recursive: true);
        }
        catch (IOException)
        {
            // Another cleanup may already own the directory; the next operation can retry.
        }
        catch (UnauthorizedAccessException)
        {
            // File permissions or a remaining loader pin delay retirement without invalidating products.
        }
    }

    private static bool IsProcessAlive(int processId)
    {
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
            // Keep the host directory when its owner cannot be inspected safely.
            return true;
        }
    }
}
