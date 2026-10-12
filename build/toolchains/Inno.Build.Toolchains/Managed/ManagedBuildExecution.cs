using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Owns a reusable, exclusively leased source workspace containing verified copies of an operation's inputs.
/// </summary>
public sealed class ManagedBuildExecution : IDisposable
{
    private readonly FileLease m_ownership;
    private readonly IReadOnlyList<NativeInputSnapshot.Entry> m_inputs;
    private readonly BuildCachePublisher m_cache;
    private readonly string m_workspace;
    private readonly string m_currentKey;
    private int m_disposed;

    private ManagedBuildExecution(
        FileLease ownership,
        string engineRoot,
        string project,
        IReadOnlyList<NativeInputSnapshot.Entry> inputs,
        BuildCachePublisher cache,
        string workspace,
        string currentKey
    ) {
        m_ownership = ownership;
        this.engineRoot = engineRoot;
        this.project = project;
        m_inputs = inputs;
        m_cache = cache;
        m_workspace = workspace;
        m_currentKey = currentKey;
    }

    /// <summary>
    /// Gets the fixed source root used by compilation and source extraction until disposal.
    /// SDKs and restored packages retain their explicitly selected external locations.
    /// </summary>
    public string engineRoot { get; }

    /// <summary>
    /// Gets the entry project within this execution's copied source root.
    /// </summary>
    public string project { get; }

    /// <summary>
    /// Checks that owned source copies still match the bytes accepted before compilation.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels complete source verification before the consumer publishes its output.
    /// </param>
    /// <returns>
    /// Completion only when every owned input remains unchanged.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// A tool changed a fixed input during execution.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// This execution no longer owns the workspace.
    /// </exception>
    public ValueTask VerifyAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(m_disposed != 0, this);
        foreach (NativeInputSnapshot.Entry input in m_inputs)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using FileStream stream = File.OpenRead(input.physicalPath);
            if (!SHA256.HashData(stream).AsSpan().SequenceEqual(input.hash))
                throw new InvalidOperationException($"Fixed managed input '{input.logicalPath}' changed during execution.");
        }
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Releases exclusive workspace ownership after every compiler and source consumer has finished.
    /// Repeated disposal has no effect; reusable owned files remain available for the next operation.
    /// </summary>
    /// <exception cref="IOException">
    /// Completed workspace ownership metadata cannot be recorded; the execution lease is still released.
    /// </exception>
    public void Dispose()
    {
        if (Interlocked.Exchange(ref m_disposed, 1) == 0)
        {
            try
            {
                using FileLease activity = m_cache.AcquireUseAsync().AsTask().GetAwaiter().GetResult();
                m_cache.Record(m_workspace, m_workspace + ".lock", [], m_currentKey);
            }
            finally
            {
                m_ownership.Dispose();
            }
        }
    }

    internal static async ValueTask<ManagedBuildExecution> CreateAsync(
        NativeBuildContext context,
        DotNetSdkDescriptor sdk,
        string project,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellation
    ) {
        string entry = Path.GetRelativePath(context.engineRoot, project);
        if (Path.IsPathRooted(entry) || entry == ".." || entry.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            throw new ArgumentException("A managed execution entry must belong to its declared source root.", nameof(project));
        string identity = NativeBuildFingerprint.Create(new[] { "managed-workspace", context.engineRoot, project,
            sdk.hostPath, sdk.sdkIdentity, sdk.cliPath }.Concat(properties.OrderBy(static pair => pair.Key, StringComparer.Ordinal)
                .Select(static pair => pair.Key + "=" + pair.Value)), []);
        string workspace = Path.Combine(context.engineRoot, "artifacts", "managed", "workspaces", identity);
        var cache = new BuildCachePublisher(Path.Combine(context.engineRoot, "artifacts", "managed", "workspaces"));
        using FileLease cacheUse = await cache.AcquireUseAsync(cancellation).ConfigureAwait(false);
        FileLease ownership = await FileLease.AcquireAsync(workspace + ".lock", Timeout.InfiniteTimeSpan, cancellation).ConfigureAwait(false);
        try
        {
            string sourceRoot = Path.Combine(workspace, "Sources");
            string metadata = Path.Combine(workspace, "inputs.json");
            NativeInputSnapshot captured = context.inputState.CaptureRecordedInputs(context.engineRoot);
            var selected = captured.entries.Where(static input => input.logicalPath.StartsWith("engine/", StringComparison.Ordinal))
                .Where(input => IsSource(context.engineRoot, input.logicalPath["engine/".Length..])).ToArray();
            var paths = new HashSet<string>(selected.Select(static input => input.logicalPath["engine/".Length..]), StringComparer.Ordinal);
            if (File.Exists(metadata))
            {
                string[] previous = JsonSerializer.Deserialize<string[]>(File.ReadAllText(metadata))
                    ?? throw new InvalidDataException("A managed workspace has no owned input inventory.");
                foreach (string removed in previous.Where(path => !paths.Contains(path)))
                {
                    string file = PathBoundary.RequireUnlinkedPath(sourceRoot, PathBoundary.Resolve(sourceRoot, removed));
                    if (File.Exists(file))
                        File.Delete(file);
                }
            }
            var fixedInputs = new List<NativeInputSnapshot.Entry>();
            foreach (NativeInputSnapshot.Entry input in selected)
            {
                string relative = input.logicalPath["engine/".Length..];
                string destination = PathBoundary.Resolve(sourceRoot, relative);
                await NativeInputMaterializer.CopyAsync(context, input.physicalPath, destination, cancellation).ConfigureAwait(false);
                fixedInputs.Add(new(input.logicalPath, destination, input.hash));
            }
            string marker = Path.Combine(sourceRoot, "InnoEngine.sln");
            if (!File.Exists(marker))
                File.WriteAllText(marker, "Managed execution source owner.", new UTF8Encoding(false));
            AtomicFile.WriteAllBytes(metadata, JsonSerializer.SerializeToUtf8Bytes(paths.Order(StringComparer.Ordinal).ToArray()));
            return new(ownership, sourceRoot, PathBoundary.Resolve(sourceRoot, entry), fixedInputs.AsReadOnly(),
                cache, workspace, project + "/" + context.configuration);
        }
        catch
        {
            ownership.Dispose();
            throw;
        }
    }

    private static bool IsSource(
        string root,
        string relative
    ) {
        string directory = root;
        foreach (string segment in relative.Split('/'))
        {
            if (segment == ".git" || segment == "artifacts" && directory == root)
                return false;
            if (segment is "bin" or "obj" && Directory.EnumerateFiles(directory, "*.csproj").Any())
                return false;
            directory = Path.Combine(directory, segment);
        }
        return true;
    }
}
