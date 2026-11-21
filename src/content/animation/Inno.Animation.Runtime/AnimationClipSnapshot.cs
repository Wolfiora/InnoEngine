using System.Linq;

namespace Inno.Animation.Runtime;

internal sealed class AnimationClipSnapshot
{
    private AnimationClipSnapshot(AnimationClipAsset clip)
    {
        this.duration = clip.duration;
        this.tracks = clip.tracks.Select(static track => new AnimationTrack(
            track.bindingId,
            track.interpolation,
            track.keyframes
        )).ToArray();
        this.events = clip.events.Select(static marker => new AnimationEventMarker
        {
            time = marker.time,
            eventId = marker.eventId,
            payload = marker.payload.ToArray()
        }).ToArray();
    }

    internal float duration { get; }

    internal AnimationTrack[] tracks { get; }

    internal AnimationEventMarker[] events { get; }

    internal static AnimationClipSnapshot Capture(AnimationClipAsset clip) => new(clip);
}
