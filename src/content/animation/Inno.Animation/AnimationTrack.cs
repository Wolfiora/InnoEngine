using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Serialization;

namespace Inno.Animation;

/// <summary>
/// Stores one ordered value track targeting a stable binding.
/// </summary>
public sealed class AnimationTrack : ISerializable
{
    /// <summary>
    /// Creates an empty track for structured deserialization.
    /// </summary>
    public AnimationTrack() {}

    /// <summary>
    /// Creates one validated animation track.
    /// </summary>
    /// <param name="bindingID">
    /// Stable target binding identifier.
    /// </param>
    /// <param name="interpolation">
    /// Sampling rule between adjacent keyframes.
    /// </param>
    /// <param name="keyframes">
    /// Keyframes ordered by non-decreasing clip-local time.
    /// </param>
    public AnimationTrack(
        string bindingID,
        AnimationInterpolation interpolation,
        IEnumerable<AnimationKeyframe> keyframes
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(bindingID);
        ArgumentNullException.ThrowIfNull(keyframes);
        this.bindingId = bindingID;
        this.interpolation = interpolation;
        this.keyframes = keyframes.ToArray();
        Validate(float.PositiveInfinity);
    }

    /// <summary>
    /// Gets or sets the stable target binding identifier.
    /// </summary>
    [SerializableProperty]
    public string bindingId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sampling rule between adjacent keyframes.
    /// </summary>
    [SerializableProperty]
    public AnimationInterpolation interpolation { get; set; }

    /// <summary>
    /// Gets or sets ordered keyframes.
    /// </summary>
    [SerializableProperty]
    public AnimationKeyframe[] keyframes { get; set; } = [];

    internal void Validate(float duration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bindingId);
        if (keyframes is null || keyframes.Length == 0)
        {
            throw new InvalidOperationException($"Animation track '{bindingId}' requires at least one keyframe.");
        }
        AnimationValueKind kind = keyframes[0].value.kind;
        float previous = -1f;
        for (int index = 0; index < keyframes.Length; index++)
        {
            AnimationKeyframe keyframe = keyframes[index];
            if (!float.IsFinite(keyframe.time) || keyframe.time < previous || keyframe.time > duration)
            {
                throw new InvalidOperationException($"Animation track '{bindingId}' contains invalid keyframe times.");
            }
            if (keyframe.value.kind != kind)
            {
                throw new InvalidOperationException($"Animation track '{bindingId}' mixes incompatible value kinds.");
            }
            previous = keyframe.time;
        }
    }
}
