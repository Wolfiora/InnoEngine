using System;
using System.Collections.Generic;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Animation;

/// <summary>
/// Represents one imported backend-neutral collection of tracks and event markers.
/// </summary>
[StableTypeID("da0df43b-f6e4-4c84-b45f-d58aa4030e6f")]
public sealed class AnimationClipAsset : AssetObject
{
    /// <summary>
    /// Gets or sets the positive clip duration in seconds.
    /// </summary>
    [SerializableProperty]
    public float duration { get; set; }

    /// <summary>
    /// Gets or sets the ordered backend-neutral value tracks.
    /// </summary>
    [SerializableProperty]
    public AnimationTrack[] tracks { get; set; } = [];

    /// <summary>
    /// Gets or sets ordered stable event markers.
    /// </summary>
    [SerializableProperty]
    public AnimationEventMarker[] events { get; set; } = [];

    /// <summary>
    /// Validates the complete clip contract before import or playback.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when duration, tracks, bindings, values, or event markers are invalid.
    /// </exception>
    public void Validate()
    {
        if (!float.IsFinite(duration) || duration <= 0f)
        {
            throw new InvalidOperationException("An animation clip requires a positive finite duration.");
        }
        if (tracks is null || events is null)
        {
            throw new InvalidOperationException("Animation clip collections cannot be null.");
        }
        var bindings = new HashSet<string>(StringComparer.Ordinal);
        for (int index = 0; index < tracks.Length; index++)
        {
            AnimationTrack track = tracks[index]
                                   ?? throw new InvalidOperationException("Animation clip tracks cannot contain null entries.");
            track.Validate(duration);
            if (!bindings.Add(track.bindingId))
            {
                throw new InvalidOperationException($"Animation binding '{track.bindingId}' is duplicated.");
            }
        }
        float previous = -1f;
        for (int index = 0; index < events.Length; index++)
        {
            AnimationEventMarker marker = events[index]
                                          ?? throw new InvalidOperationException("Animation clip events cannot contain null entries.");
            if (!float.IsFinite(marker.time) || marker.time < previous || marker.time > duration)
            {
                throw new InvalidOperationException("Animation clip event markers must be ordered within its duration.");
            }
            if (string.IsNullOrWhiteSpace(marker.eventId))
            {
                throw new InvalidOperationException("Animation event markers require stable event identifiers.");
            }
            marker.payload ??= [];
            previous = marker.time;
        }
    }
}
