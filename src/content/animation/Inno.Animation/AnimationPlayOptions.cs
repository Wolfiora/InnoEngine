namespace Inno.Animation;

/// <summary>
/// Stores immutable options used to begin one animation playback.
/// </summary>
public struct AnimationPlayOptions
{
    /// <summary>
    /// Gets the default playback options.
    /// </summary>
    public static AnimationPlayOptions defaultValue
        => new()
    {
        speed = 1f,
        weight = 1f,
        layer = 0,
        clock = AnimationClock.Scaled
    };

    /// <summary>
    /// Gets or sets the non-negative playback speed multiplier.
    /// </summary>
    public float speed { get; set; }

    /// <summary>
    /// Gets or sets the finite blend weight in the inclusive zero-to-one range.
    /// </summary>
    public float weight { get; set; }

    /// <summary>
    /// Gets or sets the deterministic blend layer.
    /// </summary>
    public int layer { get; set; }

    /// <summary>
    /// Gets or sets whether the clip repeats after its duration.
    /// </summary>
    public bool loop { get; set; }

    /// <summary>
    /// Gets or sets the session clock used by the playback.
    /// </summary>
    public AnimationClock clock { get; set; }
}
