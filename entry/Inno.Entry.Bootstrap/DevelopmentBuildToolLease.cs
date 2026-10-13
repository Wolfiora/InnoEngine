using System;
using System.IO;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Entry.Bootstrap;

internal sealed class DevelopmentBuildToolLease : IDisposable
{
    private readonly FileLease m_reading;

    internal DevelopmentBuildToolLease(
        string assemblyPath,
        CancellationToken cancellationToken
    ) {
        this.assemblyPath = assemblyPath;
        string publication = Path.GetDirectoryName(Path.GetDirectoryName(assemblyPath))!;
        m_reading = FileLease.AcquireSharedAsync(publication + ".read.lock",
            TimeSpan.FromSeconds(30), cancellationToken).GetAwaiter().GetResult();
    }

    internal string assemblyPath { get; }

    /// <summary>
    /// Releases the publication after the loaded build application has completed its command.
    /// </summary>
    public void Dispose() => m_reading.Dispose();
}
