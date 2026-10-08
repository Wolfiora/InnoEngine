using System;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Freezes product-selected keyboard conventions and horizontal wheel interpretation.
/// </summary>
public sealed class ImGuiInteractionOptions
{
    /// <summary>
    /// Captures interaction conventions without inferring them from the current operating system.
    /// </summary>
    /// <param name="commandKeyBehavior">
    /// Whether ImGui should use Command-based editing and selection conventions.
    /// </param>
    /// <param name="horizontalWheelDirection">
    /// Either one or minus one, applied to the platform event's horizontal wheel value.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The wheel direction is not one or minus one.
    /// </exception>
    public ImGuiInteractionOptions(
        bool commandKeyBehavior,
        int horizontalWheelDirection
    ) {
        if (horizontalWheelDirection is not (1 or -1))
            throw new ArgumentOutOfRangeException(nameof(horizontalWheelDirection), "Wheel direction must be one or minus one.");
        this.commandKeyBehavior = commandKeyBehavior;
        this.horizontalWheelDirection = horizontalWheelDirection;
    }

    /// <summary>
    /// Gets the explicitly chosen editing and selection conventions.
    /// </summary>
    public bool commandKeyBehavior { get; }

    /// <summary>
    /// Gets the horizontal platform wheel multiplier.
    /// </summary>
    public int horizontalWheelDirection { get; }
}
