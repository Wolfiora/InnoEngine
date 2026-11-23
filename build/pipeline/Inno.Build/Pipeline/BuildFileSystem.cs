using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build;

internal static class BuildFileSystem
{
    internal static async ValueTask MergeDirectoryAsync(
        string source,
        string destination,
        CancellationToken cancellationToken
    ) {
        if (!Directory.Exists(source))
        {
            return;
        }
        Directory.CreateDirectory(destination);
        foreach (string directory in Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Directory.CreateDirectory(Path.Combine(destination, Path.GetRelativePath(source, directory)));
        }
        foreach (string file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            if (File.Exists(target))
            {
                throw new IOException($"Target artifact '{Path.GetRelativePath(destination, target)}' collides with runtime content.");
            }
            await CopyFileAsync(file, target, cancellationToken).ConfigureAwait(false);
        }
    }

    internal static async ValueTask CopyDirectoryAsync(
        string source,
        string destination,
        CancellationToken cancellationToken
    ) {
        if (!Directory.Exists(source))
        {
            throw new DirectoryNotFoundException($"Build input directory '{source}' does not exist.");
        }
        Directory.CreateDirectory(destination);
        foreach (string directory in Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Directory.CreateDirectory(Path.Combine(destination, Path.GetRelativePath(source, directory)));
        }
        foreach (string file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            await CopyFileAsync(file, target, cancellationToken).ConfigureAwait(false);
        }
    }

    internal static async ValueTask CopyFileAsync(
        string source,
        string destination,
        CancellationToken cancellationToken
    ) {
        await using var input = new FileStream(
            source,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            128 * 1024,
            FileOptions.Asynchronous | FileOptions.SequentialScan
        );
        await using var output = new FileStream(
            destination,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            128 * 1024,
            FileOptions.Asynchronous | FileOptions.SequentialScan
        );
        await input.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
        await output.FlushAsync(cancellationToken).ConfigureAwait(false);
        if (!OperatingSystem.IsWindows())
        {
            File.SetUnixFileMode(destination, File.GetUnixFileMode(source));
        }
    }

    internal static void InstallDirectoryAtomically(
        string source,
        string destination
    ) => AtomicDirectory.Install(source, destination);
}
