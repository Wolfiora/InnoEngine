using System;
using Inno.Core.Input;

namespace Inno.Editor.Core;

/// <summary>
/// Supplies immutable keyboard conventions selected by the Editor product composition.
/// </summary>
public sealed class EditorKeyboardPolicy
{
    /// <summary>
    /// Defines the primary shortcut modifier and the display name of the Super modifier.
    /// </summary>
    /// <param name="primaryModifier">
    /// One Control, Super or Alt modifier used by symbolic primary shortcuts.
    /// </param>
    /// <param name="superModifierLabel">
    /// The nonempty product-specific name displayed for the Super modifier.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The modifier is not one supported modifier or the label is empty.
    /// </exception>
    public EditorKeyboardPolicy(
        KeyModifier primaryModifier,
        string superModifierLabel
    ) {
        if (primaryModifier is not (KeyModifier.Control or KeyModifier.Super or KeyModifier.Alt))
            throw new ArgumentException("A primary shortcut requires one Control, Super or Alt modifier.", nameof(primaryModifier));
        ArgumentException.ThrowIfNullOrWhiteSpace(superModifierLabel);
        this.primaryModifier = primaryModifier;
        this.superModifierLabel = superModifierLabel;
    }

    /// <summary>
    /// Gets the modifier used when resolving symbolic primary shortcuts.
    /// </summary>
    public KeyModifier primaryModifier { get; }

    /// <summary>
    /// Gets the product-selected display name of the Super modifier.
    /// </summary>
    public string superModifierLabel { get; }
}
