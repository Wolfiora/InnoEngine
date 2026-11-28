using System;

namespace Inno.Core.Coroutines;

/// <summary>
/// Waits until the predicate returns <see langword="true"/>.
/// </summary>
/// <param name="predicate">
/// The predicate used to initialize this instance.
/// </param>
public sealed class WaitUntil(Func<bool> predicate) : YieldInstruction
{
    private readonly Func<double, ulong, bool> m_wait = (
        _,
        _
    ) => !predicate();

    /// <summary>
    /// Gets the predicate evaluated each tick.
    /// </summary>
    public Func<bool> predicate { get; } = predicate ?? throw new ArgumentNullException(nameof(predicate));

    /// <inheritdoc/>
    protected internal override Func<double, ulong, bool> CreateWaiter() => m_wait;
}
