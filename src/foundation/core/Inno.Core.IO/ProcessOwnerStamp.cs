using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Inno.Core.IO;

/// <summary>
/// Identifies one process lifetime without retaining a process handle or confusing recycled identifiers.
/// </summary>
public readonly record struct ProcessOwnerStamp
{
    /// <summary>
    /// Freezes the identifier and UTC start timestamp observed from the same process.
    /// </summary>
    /// <param name="processId">
    /// The positive operating-system identifier.
    /// </param>
    /// <param name="startedUtcTicks">
    /// The positive UTC start timestamp in DateTime ticks.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The identifier or timestamp is outside its valid range.
    /// </exception>
    public ProcessOwnerStamp(
        int processId,
        long startedUtcTicks
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(processId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(startedUtcTicks);
        if (startedUtcTicks > DateTime.MaxValue.Ticks)
            throw new ArgumentOutOfRangeException(nameof(startedUtcTicks));
        this.processId = processId;
        this.startedUtcTicks = startedUtcTicks;
    }

    /// <summary>
    /// Gets the operating-system identifier; zero denotes an uninitialized stamp.
    /// </summary>
    public int processId { get; }

    /// <summary>
    /// Gets the UTC start ticks; zero denotes an uninitialized stamp.
    /// </summary>
    public long startedUtcTicks { get; }

    /// <summary>
    /// Captures the current process lifetime and releases its temporary inspection handle.
    /// </summary>
    /// <returns>
    /// The current process identifier and UTC start timestamp.
    /// </returns>
    /// <exception cref="Win32Exception">
    /// The operating system cannot provide the current process start timestamp.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">
    /// The execution environment does not expose operating-system process lifetimes.
    /// </exception>
    public static ProcessOwnerStamp CaptureCurrent()
    {
        using Process process = Process.GetCurrentProcess();
        return new(process.Id, process.StartTime.ToUniversalTime().Ticks);
    }

    /// <summary>
    /// Checks the exact lifetime conservatively without holding an operating-system handle afterwards.
    /// </summary>
    /// <returns>
    /// Active for the matching live process, Exited for a missing or recycled process,
    /// or Unknown for uninitialized stamps and inaccessible process information.
    /// </returns>
    public ProcessOwnerState Query()
    {
        if (processId <= 0 || startedUtcTicks <= 0)
            return ProcessOwnerState.Unknown;
        try
        {
            using Process process = Process.GetProcessById(processId);
            if (process.HasExited || process.StartTime.ToUniversalTime().Ticks != startedUtcTicks)
                return ProcessOwnerState.Exited;
            return ProcessOwnerState.Active;
        }
        catch (ArgumentException) { return ProcessOwnerState.Exited; }
        catch (InvalidOperationException) { return ProcessOwnerState.Unknown; }
        catch (Win32Exception) { return ProcessOwnerState.Unknown; }
        catch (NotSupportedException) { return ProcessOwnerState.Unknown; }
    }
}
