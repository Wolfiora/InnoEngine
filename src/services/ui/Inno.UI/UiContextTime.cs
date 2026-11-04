namespace Inno.UI;

/// <summary>
/// Captures a context clock without advancing it or retaining backend state.
/// </summary>
/// <param name="seconds">
/// The elapsed nonnegative context time in seconds.
/// </param>
/// <param name="paused">
/// Whether normal Update leaves this clock unchanged.
/// </param>
public readonly record struct UiContextTime(
    double seconds,
    bool paused
);
