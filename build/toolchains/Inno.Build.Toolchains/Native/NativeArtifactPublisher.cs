using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Publishes complete native products by source and toolchain identity under process-shared ownership.
/// </summary>
public static class NativeArtifactPublisher
{
    /// <summary>
    /// Reuses a validated product or builds an isolated candidate and publishes it after input stability checks.
    /// </summary>
    /// <param name="context">
    /// Checkout and configuration selected by build composition.
    /// </param>
    /// <param name="owner">
    /// Toolchain assembly whose project owns the target-specific intermediates.
    /// </param>
    /// <param name="component">
    /// Stable path segment identifying the native component.
    /// </param>
    /// <param name="targetId">
    /// Stable path segment identifying the target ABI.
    /// </param>
    /// <param name="inputPaths">
    /// Complete source directories and explicit tool files, resolved from the checkout when relative.
    /// Build outputs and Git metadata are excluded from directory traversal.
    /// </param>
    /// <param name="declarations">
    /// Selected SDK, compiler arguments, generation identities and other non-file inputs.
    /// </param>
    /// <param name="build">
    /// Producer receiving an identity-scoped context, private output directory and cancellation token.
    /// It must await all work and validate required outputs before returning.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels ownership acquisition or candidate preparation before publication.
    /// </param>
    /// <returns>
    /// A complete native product with an exact, read-only output closure.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// A component or target identifier is not a single path segment.
    /// </exception>
    /// <exception cref="IOException">
    /// An input, candidate or artifact cannot be read or published.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Inputs change during preparation or the producer returns no complete output.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Ownership acquisition or preparation is canceled; previous products remain installed.
    /// </exception>
    public static async Task<NativeBuildProduct> PublishAsync(
        NativeBuildContext context,
        Assembly owner,
        string component,
        string targetId,
        IReadOnlyList<string> inputPaths,
        IReadOnlyList<string> declarations,
        Func<NativeBuildContext, string, CancellationToken, Task> build,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(inputPaths);
        ArgumentNullException.ThrowIfNull(declarations);
        ArgumentNullException.ThrowIfNull(build);
        ValidateSegment(component);
        ValidateSegment(targetId);
        cancellationToken.ThrowIfCancellationRequested();
        string[] paths = inputPaths.Concat(context.hostToolchain?.inputPaths ?? [])
            .Select(path => Path.GetFullPath(path, context.engineRoot)).ToArray();
        string[] identities = declarations.Concat(context.hostToolchain?.declarations ?? []).Concat([
            component, targetId, context.configuration,
            owner.ManifestModule.ModuleVersionId.ToString(),
            typeof(NativeArtifactPublisher).Assembly.ManifestModule.ModuleVersionId.ToString()]).ToArray();
        string fingerprint = Fingerprint(paths, identities);
        NativeBuildContext scoped = context.WithIdentity(owner, targetId, fingerprint);
        string intermediate = scoped.GetNativeBuildRoot(owner);
        string destination = Path.Combine(context.engineRoot, "artifacts", "native", component, targetId, fingerprint);
        using FileLease ownership = await FileLease.AcquireAsync(intermediate + ".lock",
            Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        if (Fingerprint(paths, identities) != fingerprint)
            throw new InvalidOperationException($"Native inputs for '{component}' changed while waiting for ownership.");
        if (BuildArtifactManifest.IsComplete(destination, fingerprint, ["Outputs"]))
            return new(component, targetId, fingerprint, destination);

        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        string output = Path.Combine(staging, "Outputs");
        Directory.CreateDirectory(output);
        try
        {
            using (ToolchainWorkingDirectory workspace = await ToolchainWorkingDirectory.OpenAsync(
                intermediate, cancellationToken).ConfigureAwait(false))
                await build(scoped.WithToolDirectory(workspace.toolPath), output, cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            if (!PathBoundary.EnumerateFiles(output).Any())
                throw new InvalidOperationException($"Native component '{component}' produced no output files.");
            if (Fingerprint(paths, identities) != fingerprint)
                throw new InvalidOperationException($"Native inputs for '{component}' changed during preparation; the candidate was not published.");
            BuildArtifactManifest.Write(staging, fingerprint, ["Outputs"]);
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Install(staging, destination);
            return new(component, targetId, fingerprint, destination);
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }

    private static string Fingerprint(
        IReadOnlyList<string> paths,
        IReadOnlyList<string> identities
    ) => NativeBuildFingerprint.Create(identities, paths.SelectMany(EnumerateInputs));

    private static IEnumerable<string> EnumerateInputs(string path)
    {
        if (File.Exists(path))
        {
            yield return path;
            yield break;
        }
        if (!Directory.Exists(path))
            throw new FileNotFoundException("A declared native build input is unavailable.", path);
        StringComparer comparer = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        foreach (string file in EnumerateDirectory(path, ResolveDirectory(path), new HashSet<string>(comparer)))
            yield return file;
    }

    private static IEnumerable<string> EnumerateDirectory(
        string logicalPath,
        string physicalPath,
        HashSet<string> ancestors
    ) {
        if (!ancestors.Add(physicalPath))
            throw new IOException($"A declared native input contains a directory link cycle at '{logicalPath}'.");
        try
        {
            foreach (string file in Directory.EnumerateFiles(physicalPath))
                yield return Path.Combine(logicalPath, Path.GetFileName(file));
            foreach (string child in Directory.EnumerateDirectories(physicalPath))
            {
                string name = Path.GetFileName(child);
                if (name is ".git" or ".build" or "bin" or "obj")
                    continue;
                foreach (string file in EnumerateDirectory(Path.Combine(logicalPath, name), ResolveDirectory(child), ancestors))
                    yield return file;
            }
        }
        finally
        {
            ancestors.Remove(physicalPath);
        }
    }

    private static string ResolveDirectory(string path)
    {
        var directory = new DirectoryInfo(path);
        return Path.TrimEndingDirectorySeparator(directory.ResolveLinkTarget(returnFinalTarget: true)?.FullName ?? directory.FullName);
    }

    private static void ValidateSegment(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value is "." or ".." || value.Any(character => !char.IsAsciiLetterOrDigit(character)
            && character is not ('-' or '_' or '.')))
            throw new ArgumentException("Native component and target identities must be single path segments.", nameof(value));
    }
}
