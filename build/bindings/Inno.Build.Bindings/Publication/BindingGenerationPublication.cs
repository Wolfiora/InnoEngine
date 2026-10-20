using System;
using System.Collections.Generic;
using System.Threading;
using Inno.Core.IO;
using System.IO;
using System.Linq;

namespace Inno.Build.Bindings;

internal sealed class BindingGenerationPublication : IDisposable
{
    private readonly ProcessOwnedDirectory m_owner;
    private readonly Dictionary<string, string> m_candidates = new(StringComparer.Ordinal);
    private readonly bool m_target;
    private readonly string m_workingRoot;
    private bool m_preserveRecovery;

    private BindingGenerationPublication(
        string workingRoot,
        string destination,
        string? native,
        bool target,
        CancellationToken cancellation
    ) {
        managedDestination = destination;
        nativeDestination = native;
        m_owner = ProcessOwnedDirectory.CreateAsync(workingRoot, cancellation).AsTask().GetAwaiter().GetResult();
        m_workingRoot = workingRoot;
        m_target = target;
        m_candidates.Add(destination, Path.Combine(m_owner.directory, "Managed"));
        if (native is not null)
            m_candidates.Add(native, Path.Combine(m_owner.directory, "Native"));
    }

    internal string managedDestination { get; }
    internal string? nativeDestination { get; }
    internal string bgcsCandidate => Path.Combine(m_owner.directory, "Bgcs");
    internal string targetCandidate => m_candidates[managedDestination];
    internal IEnumerable<string> stagingPaths => m_candidates.Values;

    internal static BindingGenerationPublication CreateTarget(
        string workingRoot,
        string destination,
        CancellationToken cancellation
    ) => new(workingRoot, destination, null, true, cancellation);

    internal static BindingGenerationPublication CreateHost(
        string workingRoot,
        string managed,
        string? native,
        CancellationToken cancellation
    ) => new(workingRoot, managed, native, false, cancellation);

    internal string GetHostCandidate(string destination) => m_candidates[destination];

    internal void Commit()
    {
        if (m_target)
        {
            try
            {
                AtomicDirectory.Install(targetCandidate, managedDestination);
            }
            catch (DirectoryCleanupException committedCleanup)
            {
                Console.Error.WriteLine(committedCleanup.Message);
            }
            return;
        }
        // Generation locks protect the complete host output set. Keep every previous tree
        // in this claimed transaction until all candidate renames succeed.
        var previous = new Dictionary<string, string>(StringComparer.Ordinal);
        var installed = new List<string>();
        try
        {
            foreach (var candidate in m_candidates)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(candidate.Key)!);
                if (Directory.Exists(candidate.Key))
                {
                    string backup = Path.Combine(m_owner.directory, "Previous" + previous.Count);
                    Directory.Move(candidate.Key, backup);
                    previous.Add(candidate.Key, backup);
                }
                Directory.Move(candidate.Value, candidate.Key);
                installed.Add(candidate.Key);
            }
        }
        catch (Exception commitFailure)
        {
            var failures = new List<Exception> { commitFailure };
            foreach (string destination in m_candidates.Keys.Reverse())
            {
                try
                {
                    if (installed.Contains(destination))
                        Directory.Move(destination, m_candidates[destination]);
                    if (previous.TryGetValue(destination, out string? backup))
                        Directory.Move(backup, destination);
                }
                catch (Exception rollbackFailure)
                {
                    failures.Add(rollbackFailure);
                }
            }
            if (failures.Count > 1)
            {
                m_preserveRecovery = true;
                new Inno.Build.Toolchains.BuildCachePublisher(m_workingRoot, Inno.Build.Toolchains.BuildCacheArtifactRole.ImmutableArtifact)
                    .Record(m_owner.directory, m_owner.directory + ".work.lock", [], "binding-recovery:" + Path.GetFileName(m_owner.directory), null);
                throw new AggregateException("Binding publication failed; owned recovery trees could not be fully restored.", failures);
            }
            throw;
        }
    }

    /// <summary>
    /// Releases unpublished staging directories without replacing completed output trees.
    /// </summary>
    public void Dispose()
    {
        if (!m_preserveRecovery)
            m_owner.Dispose();
    }
}
