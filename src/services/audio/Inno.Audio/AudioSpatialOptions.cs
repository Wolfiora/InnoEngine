using System;
using Inno.Core.Mathematics;

namespace Inno.Audio;

/// <summary>
/// Contains listener-relative positioning and attenuation parameters for one voice.
/// </summary>
public readonly record struct AudioSpatialOptions
{
    /// <summary>
    /// Creates spatial playback parameters.
    /// </summary>
    /// <param name="position">
    /// World-space source position.
    /// </param>
    /// <param name="direction">
    /// World-space source forward direction.
    /// </param>
    /// <param name="velocity">
    /// World-space source velocity used for Doppler calculation.
    /// </param>
    /// <param name="distanceModel">
    /// Distance attenuation model.
    /// </param>
    /// <param name="minDistance">
    /// Distance at which attenuation begins.
    /// </param>
    /// <param name="maxDistance">
    /// Distance beyond which attenuation is clamped.
    /// </param>
    /// <param name="rolloff">
    /// Attenuation strength for the selected distance model.
    /// </param>
    /// <param name="coneInnerAngle">
    /// Full-volume source cone angle in degrees.
    /// </param>
    /// <param name="coneOuterAngle">
    /// Outer source cone angle in degrees.
    /// </param>
    /// <param name="coneOuterGain">
    /// Gain outside the outer cone.
    /// </param>
    /// <param name="dopplerFactor">
    /// Doppler effect multiplier.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A vector or scalar is non-finite, a spatial bound is invalid, or the distance model is undefined.
    /// </exception>
    public AudioSpatialOptions(
        Vector3 position,
        Vector3 direction,
        Vector3 velocity,
        AudioDistanceModel distanceModel = AudioDistanceModel.Inverse,
        float minDistance = 1f,
        float maxDistance = 100f,
        float rolloff = 1f,
        float coneInnerAngle = 360f,
        float coneOuterAngle = 360f,
        float coneOuterGain = 1f,
        float dopplerFactor = 1f
    ) {
        AudioValueValidation.RequireFinite(position, nameof(position));
        AudioValueValidation.RequireFinite(direction, nameof(direction));
        AudioValueValidation.RequireFinite(velocity, nameof(velocity));
        if (!Enum.IsDefined(distanceModel))
        {
            throw new ArgumentOutOfRangeException(nameof(distanceModel));
        }
        if (!float.IsFinite(minDistance) || !float.IsFinite(maxDistance) || minDistance < 0f || maxDistance < minDistance)
        {
            throw new ArgumentOutOfRangeException(nameof(minDistance), "Spatial distance bounds are invalid.");
        }
        if (!float.IsFinite(rolloff) || rolloff < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(rolloff));
        }
        if (!float.IsFinite(coneInnerAngle) || !float.IsFinite(coneOuterAngle)
            || coneInnerAngle is < 0f or > 360f || coneOuterAngle < coneInnerAngle || coneOuterAngle > 360f
        ) {
            throw new ArgumentOutOfRangeException(nameof(coneOuterAngle), "Spatial cone angles are invalid.");
        }
        if (!float.IsFinite(coneOuterGain) || coneOuterGain is < 0f or > 1f)
        {
            throw new ArgumentOutOfRangeException(nameof(coneOuterGain));
        }
        if (!float.IsFinite(dopplerFactor) || dopplerFactor < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(dopplerFactor));
        }
        this.position = position;
        this.direction = direction;
        this.velocity = velocity;
        this.distanceModel = distanceModel;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
        this.rolloff = rolloff;
        this.coneInnerAngle = coneInnerAngle;
        this.coneOuterAngle = coneOuterAngle;
        this.coneOuterGain = coneOuterGain;
        this.dopplerFactor = dopplerFactor;
    }

    /// <summary>
    /// Gets the world-space source position.
    /// </summary>
    public Vector3 position { get; }

    /// <summary>
    /// Gets the world-space source forward direction.
    /// </summary>
    public Vector3 direction { get; }

    /// <summary>
    /// Gets the world-space source velocity.
    /// </summary>
    public Vector3 velocity { get; }

    /// <summary>
    /// Gets the distance attenuation model.
    /// </summary>
    public AudioDistanceModel distanceModel { get; }

    /// <summary>
    /// Gets the distance at which attenuation begins.
    /// </summary>
    public float minDistance { get; }

    /// <summary>
    /// Gets the distance beyond which attenuation is clamped.
    /// </summary>
    public float maxDistance { get; }

    /// <summary>
    /// Gets the distance attenuation strength.
    /// </summary>
    public float rolloff { get; }

    /// <summary>
    /// Gets the full-volume cone angle in degrees.
    /// </summary>
    public float coneInnerAngle { get; }

    /// <summary>
    /// Gets the outer cone angle in degrees.
    /// </summary>
    public float coneOuterAngle { get; }

    /// <summary>
    /// Gets the gain outside the outer cone.
    /// </summary>
    public float coneOuterGain { get; }

    /// <summary>
    /// Gets the Doppler effect multiplier.
    /// </summary>
    public float dopplerFactor { get; }
}
