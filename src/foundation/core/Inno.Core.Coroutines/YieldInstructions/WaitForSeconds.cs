using System;

namespace Inno.Core.Coroutines;

/// <summary>
/// Waits for the specified number of scaled seconds.
/// </summary>
/// <param name="seconds">
/// The seconds used to initialize this instance.
/// </param>
public sealed class WaitForSeconds(float seconds) : YieldInstruction
{
    private readonly Func<double, ulong, bool> m_wait = (
        elapsedSeconds,
        _
    ) => elapsedSeconds < seconds;

    /// <summary>
    /// Gets the configured wait time in seconds.
    /// </summary>
    public float seconds { get; } = seconds;

    /// <inheritdoc/>
    protected internal override Func<double, ulong, bool> CreateWaiter() => m_wait;
}
