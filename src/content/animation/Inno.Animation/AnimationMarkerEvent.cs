using System;
using Inno.Core.Events;

namespace Inno.Animation;

/// <summary>
/// Publishes a stable marker reached by one animation playback on the main runtime thread.
/// </summary>
public sealed class AnimationMarkerEvent : Event
{
    /// <summary>
    /// Creates one immutable animation marker event.
    /// </summary>
    /// <param name="playback">
    /// Playback that crossed the marker.
    /// </param>
    /// <param name="eventID">
    /// Stable marker protocol identifier.
    /// </param>
    /// <param name="payload">
    /// Opaque reload-safe marker payload bytes.
    /// </param>
    public AnimationMarkerEvent(
        AnimationPlaybackHandle playback,
        string eventID,
        ReadOnlyMemory<byte> payload
    ) {
        if (!playback.isValid)
        {
            throw new ArgumentException("A valid animation playback handle is required.", nameof(playback));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(eventID);
        this.playback = playback;
        this.eventID = eventID;
        this.payload = payload.ToArray();
    }

    /// <summary>
    /// Gets the playback that crossed the marker.
    /// </summary>
    public AnimationPlaybackHandle playback { get; }

    /// <summary>
    /// Gets the stable marker protocol identifier.
    /// </summary>
    public string eventID { get; }

    /// <summary>
    /// Gets an immutable copy of the marker payload.
    /// </summary>
    public ReadOnlyMemory<byte> payload { get; }
}
