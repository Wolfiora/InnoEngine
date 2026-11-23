using System;
using Inno.Core.Mathematics;

namespace Inno.Audio;

/// <summary>
/// Contains one listener transform used by backend spatialization.
/// </summary>
public readonly record struct AudioListenerState
{
    /// <summary>
    /// Creates a listener transform snapshot.
    /// </summary>
    /// <param name="position">
    /// World-space listener position.
    /// </param>
    /// <param name="direction">
    /// World-space listener forward direction.
    /// </param>
    /// <param name="up">
    /// World-space listener up direction.
    /// </param>
    /// <param name="velocity">
    /// World-space listener velocity.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Any position, direction, up, or velocity component is non-finite.
    /// </exception>
    public AudioListenerState(
        Vector3 position,
        Vector3 direction,
        Vector3 up,
        Vector3 velocity
    ) {
        AudioValueValidation.RequireFinite(position, nameof(position));
        AudioValueValidation.RequireFinite(direction, nameof(direction));
        AudioValueValidation.RequireFinite(up, nameof(up));
        AudioValueValidation.RequireFinite(velocity, nameof(velocity));
        this.position = position;
        this.direction = direction;
        this.up = up;
        this.velocity = velocity;
    }

    /// <summary>
    /// Gets the world-space listener position.
    /// </summary>
    public Vector3 position { get; }

    /// <summary>
    /// Gets the world-space listener forward direction.
    /// </summary>
    public Vector3 direction { get; }

    /// <summary>
    /// Gets the world-space listener up direction.
    /// </summary>
    public Vector3 up { get; }

    /// <summary>
    /// Gets the world-space listener velocity.
    /// </summary>
    public Vector3 velocity { get; }
}
