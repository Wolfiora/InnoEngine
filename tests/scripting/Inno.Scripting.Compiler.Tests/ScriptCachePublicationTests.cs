using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Scripting.Compiler.Tests;

/// <summary>
/// Verifies publication through the public compiler while independently observing its owned artifacts.
/// </summary>
public sealed class ScriptCachePublicationTests
{
    /// <summary>
    /// Publishes a complete generation after a temporary filesystem reader releases its staging borrow.
    /// </summary>
    /// <returns>The completed public compiler and artifact-equivalence checks.</returns>
    [WindowsDirectorySharingFact]
    public async Task ReusedAssembliesPublishAfterATemporaryStagingReaderReleasesThem()
    {
        using var fixture = new CompilerFixture();
        fixture.Write("Probe.cs", "public sealed class Probe { public int value { get; set; } }");
        ScriptCompilationResult baseline = fixture.Compile();
        Assert.True(baseline.success, string.Join(Environment.NewLine, baseline.diagnostics));
        string previous = Assert.IsType<string>(baseline.outputDirectory);
        Assert.NotEmpty(baseline.compiledAssemblyNames);
        Dictionary<string, byte[]> expected = Directory.EnumerateFiles(previous, "*.dll")
            .ToDictionary(static path => Path.GetFileName(path)!, File.ReadAllBytes, StringComparer.Ordinal);
        string root = Path.GetDirectoryName(previous)!;
        Assert.StartsWith(Path.GetFullPath(fixture.projectRoot) + Path.DirectorySeparatorChar,
            Path.GetFullPath(previous), StringComparison.OrdinalIgnoreCase);
        Directory.Delete(previous, recursive: true);
        HashSet<string> existing = Directory.EnumerateFiles(root, "*.dll", SearchOption.AllDirectories)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        using var observer = new StagingReader(root, existing,
            $"Reused cached {baseline.compiledAssemblyNames[^1]} assembly artifact.");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        Task<ScriptCompilationResult> compilation = fixture.compiler
            .CompileAuthoringGenerationAsync(observer, cancellation.Token).AsTask();
        Exception? compilationFailure = null;
        bool captured = false;
        try
        {
            await observer.captured.WaitAsync(TimeSpan.FromSeconds(20));
            captured = true;
            await Task.Delay(150);
            Assert.False(compilation.IsCompleted,
                compilation.Exception?.ToString() ?? "A staged assembly borrow must delay publication.");
        }
        finally
        {
            if (!captured)
                cancellation.Cancel();
            observer.Dispose();
            compilationFailure = await Record.ExceptionAsync(async () => { _ = await compilation; });
        }
        Assert.Null(compilationFailure);
        ScriptCompilationResult result = await compilation;
        Assert.True(result.success, string.Join(Environment.NewLine, result.diagnostics));
        Assert.Equal(previous, result.outputDirectory);
        Assert.Empty(result.compiledAssemblyNames);
        Assert.Equal(baseline.compiledAssemblyNames, result.reusedAssemblyNames);
        foreach ((string name, byte[] bytes) in expected)
            Assert.Equal(bytes, File.ReadAllBytes(Path.Combine(result.outputDirectory!, name)));
    }

    /// <summary>
    /// Cancels a blocked publication without changing another completed generation.
    /// </summary>
    /// <returns>The completed failure, previous-generation and explicit retry checks.</returns>
    [WindowsDirectorySharingFact]
    public Task CanceledPublicationPreservesThePreviousGenerationAndAllowsAnotherRequest()
        => Task.Run(() => VerifyRejectedPublication(cancel: true));

    /// <summary>
    /// Reports a persistent filesystem block without publishing incomplete or replacing previous artifacts.
    /// </summary>
    /// <returns>The completed failure, previous-generation and explicit retry checks.</returns>
    [WindowsDirectorySharingFact]
    public Task BlockedPublicationPreservesThePreviousGenerationAndAllowsAnotherRequest()
        => Task.Run(() => VerifyRejectedPublication(cancel: false));

    /// <summary>
    /// Preserves a rejected cache tree until its reader releases ownership and an explicit repair commits.
    /// </summary>
    /// <returns>The completed replacement failure, byte-preservation and repair checks.</returns>
    [WindowsDirectorySharingFact]
    public Task BlockedReplacementPreservesTheEntireExistingTreeAndAllowsRepair()
        => Task.Run(VerifyRejectedReplacement);

    private static void VerifyRejectedReplacement()
    {
        using var fixture = new CompilerFixture();
        fixture.Write("Probe.cs", "public sealed class Probe { public int value { get; set; } }");
        ScriptCompilationResult baseline = fixture.Compile();
        Assert.True(baseline.success, string.Join(Environment.NewLine, baseline.diagnostics));
        string publication = Assert.IsType<string>(baseline.outputDirectory);
        Assert.StartsWith(Path.GetFullPath(fixture.projectRoot) + Path.DirectorySeparatorChar,
            Path.GetFullPath(publication), StringComparison.OrdinalIgnoreCase);
        Dictionary<string, byte[]> original = Directory.EnumerateFiles(publication, "*", SearchOption.AllDirectories)
            .ToDictionary(path => Path.GetRelativePath(publication, path), File.ReadAllBytes, StringComparer.Ordinal);
        string assembly = Directory.EnumerateFiles(publication, "*.dll").First();
        File.WriteAllBytes(assembly, [0, 1, 2, 3]);
        Dictionary<string, byte[]> rejected = Directory.EnumerateFiles(publication, "*", SearchOption.AllDirectories)
            .ToDictionary(path => Path.GetRelativePath(publication, path), File.ReadAllBytes, StringComparer.Ordinal);
        using (var reader = new FileStream(assembly, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete))
        {
            Exception? failure = Record.Exception(() => fixture.Compile());
            Assert.True(failure is IOException or UnauthorizedAccessException, failure?.ToString());
            Assert.Equal(rejected.Keys.Order(StringComparer.Ordinal),
                Directory.EnumerateFiles(publication, "*", SearchOption.AllDirectories)
                    .Select(path => Path.GetRelativePath(publication, path)).Order(StringComparer.Ordinal));
            foreach ((string path, byte[] bytes) in rejected)
                Assert.Equal(bytes, File.ReadAllBytes(Path.Combine(publication, path)));
        }

        ScriptCompilationResult repaired = fixture.Compile();
        Assert.True(repaired.success, string.Join(Environment.NewLine, repaired.diagnostics));
        Assert.Equal(publication, repaired.outputDirectory);
        Assert.Empty(repaired.compiledAssemblyNames);
        Assert.Equal(original.Keys.Order(StringComparer.Ordinal),
            Directory.EnumerateFiles(publication, "*", SearchOption.AllDirectories)
                .Select(path => Path.GetRelativePath(publication, path)).Order(StringComparer.Ordinal));
        foreach ((string path, byte[] bytes) in original)
            Assert.Equal(bytes, File.ReadAllBytes(Path.Combine(publication, path)));
    }

    private static void VerifyRejectedPublication(bool cancel)
    {
        using var fixture = new CompilerFixture();
        fixture.Write("Probe.cs", "public sealed class Probe { public int value { get; set; } = 1; }");
        ScriptCompilationResult previous = fixture.Compile();
        Assert.True(previous.success, string.Join(Environment.NewLine, previous.diagnostics));
        string lastGood = Assert.IsType<string>(previous.outputDirectory);
        Dictionary<string, byte[]> saved = Directory.EnumerateFiles(lastGood, "*", SearchOption.AllDirectories)
            .ToDictionary(path => Path.GetRelativePath(lastGood, path), File.ReadAllBytes, StringComparer.Ordinal);
        fixture.Write("Probe.cs", "public sealed class Probe { public int value { get; set; } = 2; }");
        ScriptCompilationResult candidate = fixture.Compile();
        Assert.True(candidate.success, string.Join(Environment.NewLine, candidate.diagnostics));
        string publication = Assert.IsType<string>(candidate.outputDirectory);
        Assert.NotEqual(lastGood, publication);
        Assert.NotEmpty(candidate.compiledAssemblyNames);
        Assert.StartsWith(Path.GetFullPath(fixture.projectRoot) + Path.DirectorySeparatorChar,
            Path.GetFullPath(publication), StringComparison.OrdinalIgnoreCase);
        string root = Path.GetDirectoryName(publication)!;
        Directory.Delete(publication, recursive: true);
        HashSet<string> existing = Directory.EnumerateFiles(root, "*.dll", SearchOption.AllDirectories)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        using var observer = new StagingReader(root, existing,
            $"Reused cached {candidate.compiledAssemblyNames[^1]} assembly artifact.");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        Task<ScriptCompilationResult> compilation = fixture.compiler
            .CompileAuthoringGenerationAsync(observer, cancellation.Token).AsTask();
        try
        {
            observer.captured.WaitAsync(TimeSpan.FromSeconds(20)).GetAwaiter().GetResult();
            if (cancel)
                cancellation.Cancel();
            Exception? failure = Record.Exception(() => compilation.WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult());
            if (cancel)
                Assert.IsAssignableFrom<OperationCanceledException>(failure);
            else
                Assert.True(failure is IOException or UnauthorizedAccessException, failure?.ToString());
        }
        finally
        {
            observer.Dispose();
            cancellation.Cancel();
            _ = Record.Exception(() => compilation.GetAwaiter().GetResult());
        }

        Assert.False(Directory.Exists(publication));
        foreach ((string path, byte[] bytes) in saved)
            Assert.Equal(bytes, File.ReadAllBytes(Path.Combine(lastGood, path)));
        ScriptCompilationResult recovered = fixture.Compile();
        Assert.True(recovered.success, string.Join(Environment.NewLine, recovered.diagnostics));
        Assert.Equal(publication, recovered.outputDirectory);
        Assert.Empty(recovered.compiledAssemblyNames);
        foreach ((string path, byte[] bytes) in saved)
            Assert.Equal(bytes, File.ReadAllBytes(Path.Combine(lastGood, path)));
    }

    private sealed class StagingReader : IProgress<ScriptCompilationProgress>, IDisposable
    {
        private readonly object m_sync = new();
        private readonly string m_root;
        private readonly IReadOnlySet<string> m_existing;
        private readonly string m_stage;
        private readonly TaskCompletionSource<bool> m_captured = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private FileStream? m_reader;
        private bool m_retired;

        internal StagingReader(
            string root,
            IReadOnlySet<string> existing,
            string stage
        ) {
            m_root = root;
            m_existing = existing;
            m_stage = stage;
        }

        /// <inheritdoc />
        public void Report(ScriptCompilationProgress value)
        {
            lock (m_sync)
            {
                if (m_retired || m_reader is not null || value.stage != m_stage)
                    return;
                try
                {
                    string[] candidates = Directory.EnumerateFiles(m_root, "*.dll", SearchOption.AllDirectories)
                        .Where(path => !m_existing.Contains(path)).ToArray();
                    Assert.NotEmpty(candidates);
                    m_reader = new FileStream(candidates[0], FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
                    m_captured.TrySetResult(true);
                }
                catch (Exception failure)
                {
                    m_captured.TrySetException(failure);
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            lock (m_sync)
            {
                m_retired = true;
                m_reader?.Dispose();
                m_reader = null;
            }
        }

        internal Task captured => m_captured.Task;
    }

    private sealed class WindowsDirectorySharingFactAttribute : FactAttribute
    {
        /// <summary>Marks the Windows directory-sharing fixture as unexecuted on other hosts.</summary>
        public WindowsDirectorySharingFactAttribute()
        {
            if (!OperatingSystem.IsWindows())
                Skip = "This fixture requires Windows directory rename sharing semantics.";
        }
    }
}
