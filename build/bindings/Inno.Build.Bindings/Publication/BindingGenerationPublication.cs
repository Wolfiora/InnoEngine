using System;
using System.Collections.Generic;
using System.Threading;
using BGCS.Core.IO;

namespace Inno.Build.Bindings;

internal sealed class BindingGenerationPublication : IDisposable
{
    private readonly OutputDirectoryTransaction? m_target;
    private readonly OutputDirectorySetTransaction? m_host;

    private BindingGenerationPublication(
        string destination,
        string? native,
        bool target,
        CancellationToken cancellation
    ) {
        managedDestination = destination;
        nativeDestination = native;
        if (target)
            m_target = new(destination, cancellationToken: cancellation);
        else
            m_host = new(native is null ? [destination] : [destination, native], cancellationToken: cancellation);
    }

    internal string managedDestination { get; }
    internal string? nativeDestination { get; }
    internal string targetCandidate => m_target!.stagingPath;
    internal IEnumerable<string> stagingPaths => m_target is null ? m_host!.stagingPaths.Values : [m_target.stagingPath];

    internal static BindingGenerationPublication CreateTarget(
        string destination,
        CancellationToken cancellation
    ) => new(destination, null, true, cancellation);

    internal static BindingGenerationPublication CreateHost(
        string managed,
        string? native,
        CancellationToken cancellation
    ) => new(managed, native, false, cancellation);

    internal string GetHostCandidate(string destination) => m_host!.GetStagingPath(destination);

    internal void Commit()
    {
        if (m_target is not null)
            m_target.Commit();
        else
            m_host!.Commit();
    }

    /// <summary>
    /// Releases unpublished staging directories without replacing completed output trees.
    /// </summary>
    public void Dispose()
    {
        m_target?.Dispose();
        m_host?.Dispose();
    }
}
