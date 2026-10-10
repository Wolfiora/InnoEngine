using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Describes copied recipe inputs owned by one cold CMake producer.
/// External SDK locations remain assigned to the frozen toolchain.
/// </summary>
public sealed class NativeCMakeSource
{
    private readonly string m_engineRoot;
    private readonly HashSet<string> m_paths;

    private NativeCMakeSource(
        string engineRoot,
        string root,
        IReadOnlyList<NativeInputSnapshot.Entry> entries
    ) {
        m_engineRoot = Path.TrimEndingDirectorySeparator(engineRoot);
        this.root = root;
        m_paths = new(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        foreach (NativeInputSnapshot.Entry entry in entries)
        {
            string path = Path.GetFullPath(entry.physicalPath);
            while (!m_paths.Comparer.Equals(path, m_engineRoot) && m_paths.Add(path))
                path = Path.GetDirectoryName(path)!;
        }
        m_paths.Add(m_engineRoot);
    }

    /// <summary>
    /// Gets the isolated source root; this is neither the checkout nor an output installation directory.
    /// </summary>
    public string root { get; }

    /// <summary>
    /// Resolves a declared checkout file or directory to its copied counterpart.
    /// </summary>
    /// <param name="path">
    /// An absolute recipe input path. External SDK paths retain their original location.
    /// </param>
    /// <returns>
    /// The copied checkout input, or the unchanged external SDK location.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The path is empty or relative.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A checkout path was not declared by the recipe.
    /// </exception>
    public string ResolvePath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!Path.IsPathFullyQualified(path))
            throw new ArgumentException("A CMake input path must be absolute.", nameof(path));
        string absolute = Path.GetFullPath(path);
        if (!IsCheckoutPath(m_engineRoot, absolute))
            return absolute;
        return m_paths.Contains(absolute)
            ? PathBoundary.Resolve(root, Path.GetRelativePath(m_engineRoot, absolute))
            : throw new InvalidOperationException("A CMake source was not declared by the native recipe: " + absolute);
    }

    /// <summary>
    /// Resolves declared input paths in one CMake definition without redirecting output or SDK locations.
    /// </summary>
    /// <param name="argument">
    /// An ordered CMake argument; definition values may contain semicolon-separated paths.
    /// </param>
    /// <returns>
    /// The definition with known input paths redirected, or the original nondefinition argument.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The argument is null.
    /// </exception>
    public string ResolveDefinition(string argument)
    {
        ArgumentNullException.ThrowIfNull(argument);
        int separator = argument.IndexOf('=');
        if (!argument.StartsWith("-D", StringComparison.Ordinal) || separator < 0)
            return argument;
        string[] values = argument[(separator + 1)..].Split(';');
        for (int index = 0; index < values.Length; index++)
        {
            string value = values[index];
            if (Path.IsPathFullyQualified(value) && m_paths.Contains(Path.GetFullPath(value)))
                values[index] = ResolvePath(value);
        }
        return argument[..(separator + 1)] + string.Join(';', values);
    }

    private static async Task<NativeCMakeSource> CreateAsync(
        NativeBuildContext context,
        NativeComponentDescriptor component,
        NativeInputSnapshot snapshot,
        CancellationToken cancellationToken
    ) {
        StringComparer comparer = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        NativeInputSnapshot.Entry[] entries = snapshot.entries
            .Where(entry => IsCheckoutPath(context.engineRoot, entry.physicalPath))
            .GroupBy(static entry => entry.physicalPath, comparer)
            .Select(group => new NativeInputSnapshot.Entry(
                "engine/" + Path.GetRelativePath(context.engineRoot, group.Key).Replace('\\', '/'),
                group.Key, group.First().hash))
            .OrderBy(static entry => entry.logicalPath, StringComparer.Ordinal).ToArray();
        if (entries.Length == 0)
            throw new InvalidOperationException("The native recipe declares no checkout source inputs.");
        string root = Path.Combine(context.GetNativeBuildRoot(component), "Sources");
        if (Directory.Exists(root) && IsComplete(context, root, entries, cancellationToken))
            return new(context.engineRoot, root, entries);
        string candidate = root + ".staging-" + Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(candidate);
        try
        {
            foreach (NativeInputSnapshot.Entry entry in entries)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string destination = PathBoundary.Resolve(candidate, entry.logicalPath["engine/".Length..]);
                await NativeInputMaterializer.CopyAsync(context, entry.physicalPath,
                    destination, cancellationToken).ConfigureAwait(false);
            }
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Install(candidate, root);
            return new(context.engineRoot, root, entries);
        }
        finally
        {
            if (Directory.Exists(candidate))
                Directory.Delete(candidate, recursive: true);
        }
    }

    private static bool IsCheckoutPath(
        string engineRoot,
        string path
    ) {
        string relative = Path.GetRelativePath(engineRoot, path);
        return relative != ".." && !relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal)
            && !Path.IsPathRooted(relative);
    }

    private static bool IsComplete(
        NativeBuildContext context,
        string root,
        IReadOnlyList<NativeInputSnapshot.Entry> expected,
        CancellationToken cancellationToken
    ) {
        string[] files = PathBoundary.EnumerateFiles(root).ToArray();
        if (files.Length != expected.Count)
            return false;
        NativeInputSnapshot actual = context.inputState.CaptureVerification(files.Select(path =>
            new NativeBuildInput("engine/" + Path.GetRelativePath(root, path).Replace('\\', '/'), path)), cancellationToken);
        for (int index = 0; index < expected.Count; index++)
        {
            if (actual.entries[index].logicalPath != expected[index].logicalPath
                || !actual.entries[index].hash.AsSpan().SequenceEqual(expected[index].hash))
                return false;
        }
        return true;
    }
    internal sealed class Preparation
    {
        private readonly object m_gate = new();
        private readonly NativeInputSnapshot m_inputs;
        private Task<NativeCMakeSource>? m_task;

        internal Preparation(NativeInputSnapshot inputs) => m_inputs = inputs;

        internal Task<NativeCMakeSource> GetAsync(
            NativeBuildContext context,
            NativeComponentDescriptor component,
            CancellationToken cancellationToken
        ) {
            lock (m_gate)
                return m_task ??= CreateAsync(context, component, m_inputs, cancellationToken);
        }
    }
}
