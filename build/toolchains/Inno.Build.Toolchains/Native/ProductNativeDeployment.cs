using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Materializes an explicitly built host-native closure into one application's deployment directory.
/// </summary>
public static class ProductNativeDeployment
{
    /// <summary>
    /// Validates the application's native tree and atomically replaces it when its exact contents differ.
    /// </summary>
    /// <param name="products">
    /// Validated runtime and optional authoring products returned by the declared product plan.
    /// All products must use the same target; each component can appear once.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels ownership acquisition or staging before the atomic installation starts.
    /// </param>
    /// <returns>
    /// Completion after the complete native tree has been validated under exclusive publication ownership.
    /// Identical deployments retain their files, including libraries loaded by a running application.
    /// </returns>
    /// <param name="plan">
    /// The explicit component closure and deployment mapping used to produce these products.
    /// </param>
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
        ProductNativeBuildPlan plan,
        string applicationDirectory,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(products);
        ArgumentNullException.ThrowIfNull(plan);
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
        ValidateProducts(closure, cancellationToken);
        IReadOnlyDictionary<string, string> files = plan.CreateDeploymentFiles(closure);
        if (HasSameFiles(destination, files, cancellationToken))
        {
            ValidateProducts(closure, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return;
        }
        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(staging);
        try
        {
            foreach ((string relative, string file) in files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string output = Path.Combine(staging, relative);
                Directory.CreateDirectory(Path.GetDirectoryName(output)!);
                await using (FileStream source = File.OpenRead(file))
                await using (FileStream target = new(output, FileMode.CreateNew))
                    await source.CopyToAsync(target, cancellationToken).ConfigureAwait(false);
                if (!OperatingSystem.IsWindows())
                    File.SetUnixFileMode(output, File.GetUnixFileMode(file));
            }
            ValidateProducts(closure, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Install(staging, destination);
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }

    private static void ValidateProducts(
        IReadOnlyList<NativeBuildProduct> products,
        CancellationToken cancellationToken
    ) {
        foreach (NativeBuildProduct product in products)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!BuildArtifactManifest.IsComplete(product.directory, product.fingerprint, ["Outputs"]))
                throw new InvalidDataException($"Native product '{product.component}' changed after publication.");
        }
    }

    private static bool HasSameFiles(
        string directory,
        IReadOnlyDictionary<string, string> files,
        CancellationToken cancellationToken
    ) {
        if (!Directory.Exists(directory))
            return false;
        int count = 0;
        foreach (string deployed in PathBoundary.EnumerateFiles(directory))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!files.TryGetValue(Path.GetRelativePath(directory, deployed), out string? source)
                || new FileInfo(deployed).Length != new FileInfo(source).Length)
                return false;
            using FileStream deployedStream = File.OpenRead(deployed);
            using FileStream sourceStream = File.OpenRead(source);
            if (!SHA256.HashData(deployedStream).AsSpan().SequenceEqual(SHA256.HashData(sourceStream)))
                return false;
            if (!OperatingSystem.IsWindows() && File.GetUnixFileMode(deployed) != File.GetUnixFileMode(source))
                return false;
            count++;
        }
        cancellationToken.ThrowIfCancellationRequested();
        return count == files.Count;
    }
}
