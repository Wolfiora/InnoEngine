namespace Inno.Animation;

/// <summary>
/// Selects how values are sampled between adjacent keyframes.
/// </summary>
public enum AnimationInterpolation
{
    /// <summary>
    /// Holds the previous keyframe value until the next keyframe.
    /// </summary>
    Step,
    /// <summary>
    /// Blends linearly between adjacent keyframes.
    /// </summary>
    Linear
}
