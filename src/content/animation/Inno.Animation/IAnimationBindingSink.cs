using System;

namespace Inno.Animation;

/// <summary>
/// Receives final backend-neutral samples after runtime blending has completed.
/// </summary>
public interface IAnimationBindingSink
{
    /// <summary>
    /// Applies one immutable frame of final animation samples.
    /// </summary>
    /// <param name="samples">
    /// Samples sorted by stable binding identifier.
    /// </param>
    void Apply(ReadOnlySpan<AnimationSample> samples);
}
