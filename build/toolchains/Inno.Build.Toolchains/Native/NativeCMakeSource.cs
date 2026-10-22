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
    private readonly Dictionary<string, string> m_paths;
    private readonly NativeBindingInputPaths m_bindings;

    private NativeCMakeSource(
        string engineRoot,
        string root,
        IReadOnlyList<NativeInputSnapshot.Entry> entries,
        NativeBindingInputPaths bindings,
        PhysicalPathResolver resolver
    ) {
        m_engineRoot = Path.TrimEndingDirectorySeparator(engineRoot);
        m_bindings = bindings;
        this.root = root;
        m_paths = new(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        foreach (NativeInputSnapshot.Entry entry in entries)
        {
            string path = Path.GetFullPath(entry.physicalPath);
            while (GetSourceRelative(m_engineRoot, bindings, resolver, path) is string relative
                && m_paths.TryAdd(path, relative))
            {
                // Keep the source spelling for verification and also accept its declared physical counterpart.
                if (!IsCheckoutPath(m_engineRoot, path) && bindings.ResolveRelative(path) is null)
                    m_paths.TryAdd(resolver.Resolve(path), relative);
                if (relative == ".")
                    break;
                path = Path.GetDirectoryName(path)!;
            }
        }
        m_paths.TryAdd(m_engineRoot, ".");
    }

    /// <summary>
    /// Gets the isolated source root; this is neither the checkout nor an output installation directory.
    /// </summary>
    public string root { get; }

    /// <summary>
    /// Resolves a declared checkout or selected binding input to its copied counterpart.
    /// </summary>
    /// <param name="path">
    /// An absolute recipe input path. External SDK paths retain their original location.
    /// </param>
    /// <returns>
    /// The copied checkout or binding input, or the unchanged external SDK location.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The path is empty or relative.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A checkout or selected binding path was not declared by the recipe.
    /// </exception>
    public string ResolvePath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!Path.IsPathFullyQualified(path))
            throw new ArgumentException("A CMake input path must be absolute.", nameof(path));
        string absolute = Path.GetFullPath(path);
        if (m_paths.TryGetValue(absolute, out string? relative))
            return PathBoundary.Resolve(root, relative);
        if (!IsCheckoutPath(m_engineRoot, absolute) && m_bindings.ResolveRelative(absolute) is null)
            return absolute;
        throw new InvalidOperationException("A CMake source was not declared by the native recipe: " + absolute);
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
            if (Path.IsPathFullyQualified(value) && m_paths.ContainsKey(Path.GetFullPath(value)))
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
        var bindings = new NativeBindingInputPaths(context);
        var resolver = new PhysicalPathResolver(comparer);
        NativeInputSnapshot.Entry[] declared = snapshot.entries
            .Where(entry => GetSourceRelative(context.engineRoot, bindings, resolver, entry.physicalPath) is not null)
            .GroupBy(static entry => entry.physicalPath, comparer)
            .Select(group => new NativeInputSnapshot.Entry(
                "engine/" + GetSourceRelative(context.engineRoot, bindings, resolver, group.Key)!.Replace('\\', '/'),
                group.Key, group.First().hash))
            .OrderBy(static entry => entry.logicalPath, StringComparer.Ordinal).ToArray();
        NativeInputSnapshot.Entry[] entries = declared.GroupBy(static entry => entry.logicalPath, comparer).Select(group =>
        {
            NativeInputSnapshot.Entry first = group.First();
            string physical = resolver.Resolve(first.physicalPath);
            if (group.Any(entry => resolver.Resolve(entry.physicalPath) != physical || !entry.hash.AsSpan().SequenceEqual(first.hash)))
                throw new InvalidOperationException("Native source aliases disagree about one copied workspace location.");
            return first;
        }).ToArray();
        if (entries.Length == 0)
            throw new InvalidOperationException("The native recipe declares no checkout or selected binding inputs.");
        string root = Path.Combine(context.GetNativeBuildRoot(component), "Sources");
        if (Directory.Exists(root) && IsComplete(context, root, entries, cancellationToken))
            return new(context.engineRoot, root, declared, bindings, resolver);
        Directory.CreateDirectory(root);
        var expected = new HashSet<string>(entries.Select(static entry => entry.logicalPath["engine/".Length..]), comparer);
        foreach (string file in PathBoundary.EnumerateFiles(root))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string relative = Path.GetRelativePath(root, file).Replace('\\', '/');
            if (!expected.Contains(relative))
                File.Delete(file);
        }
        foreach (NativeInputSnapshot.Entry entry in entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string destination = PathBoundary.Resolve(root, entry.logicalPath["engine/".Length..]);
            await NativeInputMaterializer.CopyAsync(context, entry.physicalPath, destination, cancellationToken).ConfigureAwait(false);
        }
        cancellationToken.ThrowIfCancellationRequested();
        return new(context.engineRoot, root, declared, bindings, resolver);
    }

    private static string? GetSourceRelative(
        string engineRoot,
        NativeBindingInputPaths bindings,
        PhysicalPathResolver resolver,
        string path
    ) {
        string? binding = bindings.ResolveRelative(path);
        if (binding is not null)
            return binding;
        if (IsCheckoutPath(engineRoot, path))
            return Path.GetRelativePath(engineRoot, path);
        string physical = resolver.Resolve(path);
        return bindings.ResolveRelative(physical)
            ?? (IsCheckoutPath(engineRoot, physical) ? Path.GetRelativePath(engineRoot, physical) : null);
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
