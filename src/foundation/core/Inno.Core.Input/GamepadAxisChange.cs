namespace Inno.Core.Input;

/// <summary>
/// Captures a normalized axis sample without applying a consumer-specific dead zone.
/// </summary>
public sealed record GamepadAxisChange : GamepadInputChange
{
    /// <summary>
    /// Captures one finite controller axis value.
    /// </summary>
    /// <param name="deviceId">
    /// Nonzero connection identity.
    /// </param>
    /// <param name="axis">
    /// Standard axis whose range determines validation.
    /// </param>
    /// <param name="value">
    /// Stick value in [-1, 1], or trigger value in [0, 1].
    /// </param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// The axis is undefined, non-finite, or outside its normalized range.
    /// </exception>
    public GamepadAxisChange(
        ulong deviceId,
        GamepadAxis axis,
        float value
    ) : base(deviceId) {
        GamepadState.ValidateAxis(axis, value);
        this.axis = axis;
        this.value = value;
    }

    /// <summary>
    /// Gets the sampled axis.
    /// </summary>
    public GamepadAxis axis { get; }
    /// <summary>
    /// Gets its finite normalized value before dead-zone processing.
    /// </summary>
    public float value { get; }
}
