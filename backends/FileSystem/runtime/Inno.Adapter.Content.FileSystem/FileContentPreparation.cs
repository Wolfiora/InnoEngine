using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Content;
using Inno.Core.IO;

namespace Inno.Adapter.Content.FileSystem;

/// <summary>
/// Coordinates complete cache validation, generation publication, and reader-safe retirement across processes.
/// </summary>
public static class FileContentPreparation
{
    /// <summary>
    /// Reuses only a completely verified generation or publishes a fully validated replacement.
    /// </summary>
    /// <param name="source">
    /// The borrowed immutable store retained by the caller until preparation completes.
    /// </param>
    /// <param name="options">
    /// Host-selected cache ownership and bounded lock wait.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation before ownership is returned; already committed content remains valid.
    /// </param>
    /// <returns>
    /// A caller-owned store pinning the selected generation, with separate post-publication cleanup diagnostics.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The source cannot produce the indexed content or candidate validation fails.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Another preparing process retains exclusive ownership beyond the host-selected timeout.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Preparation was canceled; no store ownership is returned.
    /// </exception>
    public static async ValueTask<FileContentStore> PrepareAsync(
        IRuntimeContentStore source,
        FileContentCacheOptions options,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();
        string cache = Path.Combine(options.applicationCacheRoot, "Content", source.descriptor.contentHash);
        PathBoundary.RequireUnlinkedPath(options.applicationCacheRoot, cache);
        PathBoundary.RequireUnlinkedPath(cache, Path.Combine(cache, "install.lock"));
        using FileLease writer = await FileLease.AcquireAsync(Path.Combine(cache, "install.lock"),
            options.lockTimeout, cancellationToken).ConfigureAwait(false);
        foreach (string entry in new[] { "generations", "staging", "leases", "current" })
            PathBoundary.RequireUnlinkedPath(cache, Path.Combine(cache, entry));
        Directory.CreateDirectory(Path.Combine(cache, "generations"));
        string? current = ReadCurrent(cache);
        bool valid = current is not null
            && File.Exists(PathBoundary.RequireUnlinkedPath(cache, Path.Combine(cache, "leases", current + ".lock")))
            && await Task.Run(() => ContentCacheValidator.Validate(
            Path.Combine(cache, "generations", current), source.index, cancellationToken), cancellationToken).ConfigureAwait(false);
        if (!valid)
            current = await ContentCachePublication.PublishAsync(cache, source, cancellationToken).ConfigureAwait(false);
        FileLease reader = await FileLease.AcquireSharedAsync(Path.Combine(cache, "leases", current + ".lock"),
            options.lockTimeout, cancellationToken).ConfigureAwait(false);
        try
        {
            var diagnostics = await ContentCacheRetirement.RetireAsync(cache, current!).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            return new FileContentStore(source.descriptor, source.index,
                new ContentCacheGeneration(Path.Combine(cache, "generations", current!), reader), diagnostics);
        }
        catch
        {
            reader.Dispose();
            throw;
        }
    }

    private static string? ReadCurrent(string cache)
    {
        string pointer = Path.Combine(cache, "current");
        try
        {
            if (new FileInfo(pointer).Length != 32)
                return null;
            string value = File.ReadAllText(pointer);
            return Guid.TryParseExact(value, "N", out _) ? value : null;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }
}
