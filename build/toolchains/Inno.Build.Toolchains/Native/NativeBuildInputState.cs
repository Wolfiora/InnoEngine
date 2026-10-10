using System;
using System.Linq;
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
    private long m_bindingBatches;
    private long m_bindingGenerations;
    private long m_materializedBytes;
    private long m_outputFiles;
    private long m_outputBytes;
    private long m_managedProcesses;
    private readonly Dictionary<string, (long files, long bytes, long reused)> m_phases = new(StringComparer.Ordinal);

    internal NativeBuildInputState() => m_initialInputs = new(this);

    internal NativeBuildStatistics statistics => new()
    {
        hashedFiles = Interlocked.Read(ref m_hashedFiles),
        hashedBytes = Interlocked.Read(ref m_hashedBytes),
        nativeProcesses = Interlocked.Read(ref m_nativeProcesses),
        bindingBatches = Interlocked.Read(ref m_bindingBatches),
        bindingGenerations = Interlocked.Read(ref m_bindingGenerations),
        materializedBytes = Interlocked.Read(ref m_materializedBytes),
        outputFiles = Interlocked.Read(ref m_outputFiles),
        outputBytes = Interlocked.Read(ref m_outputBytes),
        managedProcesses = Interlocked.Read(ref m_managedProcesses),
        phases = CapturePhases()
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
    ) => NativeInputSnapshot.Capture(inputs, new NativeInputReadCache(this, "verification"), cancellationToken);

    internal void RecordHash(
        long bytes,
        string phase
    ) {
        Interlocked.Increment(ref m_hashedFiles);
        Interlocked.Add(ref m_hashedBytes, bytes);
        lock (m_gate)
        {
            m_phases.TryGetValue(phase, out var value);
            m_phases[phase] = (value.files + 1, value.bytes + bytes, value.reused);
        }
    }

    internal byte[] GetFrozenHash(string path)
    {
        lock (m_gate)
            return m_initialInputs.GetFrozenHash(path);
    }

    internal void RecordReuse(string phase)
    {
        lock (m_gate)
        {
            m_phases.TryGetValue(phase, out var value);
            m_phases[phase] = (value.files, value.bytes, value.reused + 1);
        }
    }

    internal void RecordBindingBatch() => Interlocked.Increment(ref m_bindingBatches);
    internal void RecordBindingGeneration() => Interlocked.Increment(ref m_bindingGenerations);
    internal void RecordMaterialization(long bytes) => Interlocked.Add(ref m_materializedBytes, bytes);

    internal void RecordOutput(long bytes)
    {
        Interlocked.Increment(ref m_outputFiles);
        Interlocked.Add(ref m_outputBytes, bytes);
    }

    internal void RecordManagedProcess() => Interlocked.Increment(ref m_managedProcesses);

    private IReadOnlyList<NativeBuildPhaseStatistics> CapturePhases()
    {
        lock (m_gate)
            return Array.AsReadOnly(m_phases.OrderBy(static pair => pair.Key, StringComparer.Ordinal)
                .Select(static pair => new NativeBuildPhaseStatistics
                {
                    phase = pair.Key, files = pair.Value.files, bytes = pair.Value.bytes, reusedReads = pair.Value.reused
                }).ToArray());
    }

    internal void RecordProcess() => Interlocked.Increment(ref m_nativeProcesses);
}
