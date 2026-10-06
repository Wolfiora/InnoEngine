using System.Collections.Generic;
using System.Threading;

namespace Inno.Build.Toolchains;

internal sealed class NativeBuildInputState
{
    private readonly object m_gate = new();
    private readonly NativeInputReadCache m_initialInputs;
    private long m_hashedFiles;
    private long m_hashedBytes;
    private long m_nativeProcesses;

    internal NativeBuildInputState() => m_initialInputs = new(this);

    internal NativeBuildStatistics statistics => new()
    {
        hashedFiles = Interlocked.Read(ref m_hashedFiles),
        hashedBytes = Interlocked.Read(ref m_hashedBytes),
        nativeProcesses = Interlocked.Read(ref m_nativeProcesses)
    };

    internal NativeInputSnapshot CaptureInitial(
        IEnumerable<NativeBuildInput> inputs,
        CancellationToken cancellationToken
    ) {
        lock (m_gate)
            return NativeInputSnapshot.Capture(inputs, m_initialInputs, cancellationToken);
    }

    internal NativeInputSnapshot CaptureVerification(
        IEnumerable<NativeBuildInput> inputs,
        CancellationToken cancellationToken
    ) => NativeInputSnapshot.Capture(inputs, new NativeInputReadCache(this), cancellationToken);

    internal void RecordHash(long bytes)
    {
        Interlocked.Increment(ref m_hashedFiles);
        Interlocked.Add(ref m_hashedBytes, bytes);
    }

    internal byte[] GetFrozenHash(string path)
    {
        lock (m_gate)
            return m_initialInputs.GetFrozenHash(path);
    }

    internal void RecordProcess() => Interlocked.Increment(ref m_nativeProcesses);
}
