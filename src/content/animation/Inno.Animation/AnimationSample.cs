using System;

namespace Inno.Animation;

/// <summary>
/// Stores one final blended value emitted for a stable binding identifier.
/// </summary>
public readonly record struct AnimationSample
{
    /// <summary>
    /// Creates one final animation sample.
    /// </summary>
    /// <param name="target">
    /// The weak destination or explicit sampling-only policy.
    /// </param>
    /// <param name="binding">
    /// Stable target binding.
    /// </param>
    /// <param name="value">
    /// Final blended value.
    /// </param>
    public AnimationSample(
        AnimationTarget target,
        AnimationBindingID binding,
        AnimationValue value
    ) {
        if (!target.isValid)
        {
            throw new ArgumentException("A valid animation destination is required.", nameof(target));
        }
        if (!binding.isValid)
        {
            throw new ArgumentException("A valid animation binding is required.", nameof(binding));
        }
        this.binding = binding;
        this.target = target;
        this.value = value;
    }

    /// <summary>
    /// Gets the stable target binding.
    /// </summary>
    public AnimationBindingID binding { get; }

    /// <summary>
    /// Gets the generation-qualified destination for this sample.
    /// </summary>
    public AnimationTarget target { get; }

    /// <summary>
    /// Gets the final blended value.
    /// </summary>
    public AnimationValue value { get; }
}
