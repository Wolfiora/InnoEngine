using System;
using Inno.Core.Serialization;

namespace Inno.Animation;

/// <summary>
/// Stores one timestamped value inside an animation track.
/// </summary>
public struct AnimationKeyframe
{
    /// <summary>
    /// Creates one animation keyframe.
    /// </summary>
    /// <param name="time">
    /// Non-negative clip-local time in seconds.
    /// </param>
    /// <param name="value">
    /// Value sampled at the keyframe time.
    /// </param>
    public AnimationKeyframe(
        float time,
        AnimationValue value
    ) {
        if (!float.IsFinite(time) || time < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(time));
        }
        this.time = time;
        this.value = value;
    }

    /// <summary>
    /// Gets or sets clip-local time in seconds.
    /// </summary>
    [SerializableProperty]
    public float time { get; set; }

    /// <summary>
    /// Gets or sets the sampled value.
    /// </summary>
    [SerializableProperty]
    public AnimationValue value { get; set; }
}
