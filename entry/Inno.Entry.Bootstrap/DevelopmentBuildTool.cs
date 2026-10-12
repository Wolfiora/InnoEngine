using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Entry.Bootstrap;

internal static class DevelopmentBuildTool
{
    internal static string? Open(
        string root,
        string configuration,
        string dotnet,
        CancellationToken cancellation
    ) {
        Receipt? receipt = Read(root, configuration);
        if (receipt is null || !File.Exists(receipt.assembly))
            return null;
        if (!SameFiles(receipt.files, ReadFiles(Path.GetDirectoryName(receipt.assembly)!, cancellation)))
            return null;
        string? current = Probe(receipt.assembly, root, configuration, dotnet, cancellation);
        return current == receipt.inputs ? receipt.assembly : null;
    }

    internal static void Record(
        string root,
        string configuration,
        string dotnet,
        string assembly,
        CancellationToken cancellation
    ) {
        string inputs = Probe(assembly, root, configuration, dotnet, cancellation)
            ?? throw new InvalidDataException("The prepared Build tool cannot verify its evaluated input closure.");
        var receipt = new Receipt(assembly, inputs, ReadFiles(Path.GetDirectoryName(assembly)!, cancellation));
        AtomicFile.WriteAllBytes(Path.Combine(root, "artifacts", "entry", "build", configuration, "ready.json"),
            JsonSerializer.SerializeToUtf8Bytes(receipt));
    }

    private static Receipt? Read(
        string root,
        string configuration
    ) {
        string path = Path.Combine(root, "artifacts", "entry", "build", configuration, "ready.json");
        if (!File.Exists(path))
            return null;
        try
        {
            Receipt? receipt = JsonSerializer.Deserialize<Receipt>(File.ReadAllBytes(path));
            if (receipt is null || receipt.files is null || receipt.files.Count == 0 || string.IsNullOrEmpty(receipt.inputs)
                || !Path.IsPathFullyQualified(receipt.assembly))
                return null;
            PathBoundary.RequireUnlinkedPath(root, receipt.assembly);
            return receipt;
        }
        catch (JsonException) { return null; }
    }

    private static string? Probe(
        string assembly,
        string root,
        string configuration,
        string dotnet,
        CancellationToken cancellation
    ) {
        (string? identity, WeakReference monitor) = ReadIdentity(assembly, root, configuration, dotnet, cancellation);
        for (int attempt = 0; attempt < 8 && monitor.IsAlive; attempt++)
        {
            Thread.Sleep(10);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        if (monitor.IsAlive)
            throw new InvalidOperationException("Build input inspection did not retire its load context; restart the entry.");
        return identity;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (string? identity, WeakReference monitor) ReadIdentity(
        string assembly,
        string root,
        string configuration,
        string dotnet,
        CancellationToken cancellation
    ) {
        var load = new InspectionContext(assembly);
        var monitor = new WeakReference(load);
        try
        {
            Type type = load.LoadFromAssemblyPath(assembly).GetType("Inno.Build.Cli.BuildApplication", throwOnError: true)!;
            MethodInfo? method = type.GetMethod("ReadInputIdentityAsync", BindingFlags.Public | BindingFlags.Static);
            if (method is null)
                return (null, monitor);
            var inspect = method.CreateDelegate<Func<string, string, string, CancellationToken, Task<string>>>();
            return (inspect(root, configuration, dotnet, cancellation).GetAwaiter().GetResult(), monitor);
        }
        catch (Exception failure) when (failure is IOException or InvalidOperationException or ArgumentException)
        {
            Console.WriteLine($"Build preparation is required: {failure.Message}");
            return (null, monitor);
        }
        finally
        {
            load.Unload();
        }
    }

    private static Dictionary<string, string> ReadFiles(
        string directory,
        CancellationToken cancellation
    ) {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string file in PathBoundary.EnumerateFiles(directory).Order(StringComparer.Ordinal))
        {
            cancellation.ThrowIfCancellationRequested();
            if (file.EndsWith(".lock", StringComparison.Ordinal))
                continue;
            using FileStream input = File.OpenRead(file);
            result.Add(Path.GetRelativePath(directory, file), Convert.ToHexStringLower(SHA256.HashData(input)));
        }
        return result;
    }

    private static bool SameFiles(
        IReadOnlyDictionary<string, string> first,
        IReadOnlyDictionary<string, string> second
    ) => first.Count == second.Count && second.All(pair => first.TryGetValue(pair.Key, out string? hash) && hash == pair.Value);

    private sealed record Receipt(
        string assembly,
        string inputs,
        Dictionary<string, string> files
    );

    private sealed class InspectionContext(string assembly) : AssemblyLoadContext(isCollectible: true)
    {
        private readonly AssemblyDependencyResolver m_resolver = new(assembly);

        /// <summary>
        /// Resolves only dependencies selected by this prepared tool's deployment.
        /// </summary>
        /// <param name="name">
        /// The requested assembly identity.
        /// </param>
        /// <returns>
        /// The deployment assembly, or null for a shared runtime dependency.
        /// </returns>
        protected override Assembly? Load(AssemblyName name)
        {
            string? path = m_resolver.ResolveAssemblyToPath(name);
            return path is null ? null : LoadFromAssemblyPath(path);
        }
    }
}
