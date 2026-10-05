using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains.Host;

/// <summary>
/// Materializes an explicitly built host-native closure into one application's deployment directory.
/// </summary>
public static class HostNativeDeployment
{
    /// <summary>
    /// Atomically replaces the application's native tree using the supplied products alone.
    /// </summary>
    /// <param name="products">
    /// Validated runtime and optional authoring products returned by HostNativeBuild.
    /// All products must use the same target; each component can appear once.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels ownership acquisition or staging before the atomic installation starts.
    /// </param>
    /// <returns>
    /// Completion after the complete native tree has been installed under exclusive publication ownership.
    /// </returns>
    /// <param name="applicationDirectory">
    /// The managed output directory receiving the complete native tree.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The product closure is empty, contains duplicate components or mixes targets.
    /// </exception>
    /// <exception cref="IOException">
    /// A product cannot be validated, copied or installed; the prior native tree remains available.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled before installation; the previous native tree is unchanged.
    /// </exception>
    public static async ValueTask InstallAsync(
        IReadOnlyList<NativeBuildProduct> products,
        string applicationDirectory,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(products);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationDirectory);
        NativeBuildProduct[] closure = products.ToArray();
        if (closure.Length == 0 || closure.Any(static product => product is null)
            || closure.Select(static product => product.targetId).Distinct().Count() != 1
            || closure.Select(static product => product.component).Distinct().Count() != closure.Length)
            throw new ArgumentException("Native deployment requires a nonempty closure of unique components for one target.", nameof(products));
        string destination = Path.Combine(Path.GetFullPath(applicationDirectory), "native");
        using FileLease ownership = await FileLease.AcquireAsync(destination + ".lock",
            Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(staging);
        try
        {
            foreach (NativeBuildProduct product in closure)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!BuildArtifactManifest.IsComplete(product.directory, product.fingerprint, ["Outputs"]))
                    throw new InvalidDataException($"Native product '{product.component}' changed after publication.");
                foreach (string file in product.files)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string relative = Path.GetRelativePath(Path.Combine(product.directory, "Outputs"), file);
                    // Import libraries belong to the build closure, not to an application deployment.
                    if (relative.Split(Path.DirectorySeparatorChar)[0] == "Link")
                        continue;
                    string output = product.component == "bgfx-tools"
                        ? relative.StartsWith("includes" + Path.DirectorySeparatorChar, StringComparison.Ordinal)
                            ? Path.Combine(staging, "bgfx", relative)
                            : Path.Combine(staging, "bgfx", product.targetId, "tools", relative)
                        : Path.Combine(staging, product.component, product.targetId, relative);
                    Directory.CreateDirectory(Path.GetDirectoryName(output)!);
                    await using (FileStream source = File.OpenRead(file))
                    await using (FileStream target = new(output, FileMode.CreateNew))
                        await source.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
                    if (!OperatingSystem.IsWindows())
                        File.SetUnixFileMode(output, File.GetUnixFileMode(file));
                }
                if (!BuildArtifactManifest.IsComplete(product.directory, product.fingerprint, ["Outputs"]))
                    throw new InvalidDataException($"Native product '{product.component}' changed during deployment.");
            }
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Install(staging, destination);
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }
}
