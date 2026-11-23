using System;
using System.Collections.Generic;

namespace Inno.Editor.Interactions;

/// <summary>
/// Collects dynamic menu item placements.
/// </summary>
public sealed class EditorMenuBuilder
{
    private readonly List<EditorMenuPlacement> m_items = [];
    private readonly List<EditorMenuGroupPlacement> m_groups = [];

    /// <summary>
    /// Declares presentation for an intermediate menu group without inventing a no-op action.
    /// </summary>
    /// <param name="path">
    /// The slash-delimited path of the group.
    /// </param>
    /// <param name="order">
    /// The stable ordering value among sibling groups and items.
    /// </param>
    /// <param name="separatorBefore">
    /// Whether a separator precedes this group at its own level.
    /// </param>
    public void AddGroup(
        string path,
        int order = 0,
        bool separatorBefore = false
    ) {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("A menu group path is required.", nameof(path));
        }
        m_groups.Add(new EditorMenuGroupPlacement(path, order, separatorBefore));
    }

    /// <summary>
    /// Adds a dynamic action placement to the menu currently being constructed.
    /// </summary>
    /// <param name="path">
    /// The slash-delimited path used to create parent menus and the leaf label.
    /// </param>
    /// <param name="actionID">
    /// The stable action identifier invoked by the leaf.
    /// </param>
    /// <param name="order">
    /// The stable ordering value among sibling placements.
    /// </param>
    /// <param name="separatorBefore">
    /// Whether a visual separator should precede the leaf placement.
    /// </param>
    /// <param name="argument">
    /// An optional argument captured for this menu generation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="path"/> or <paramref name="actionID"/> is empty.
    /// </exception>
    public void Add(
        string path,
        string actionID,
        int order = 0,
        bool separatorBefore = false,
        object? argument = null
    ) {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("A menu path is required.", nameof(path));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(actionID);
        m_items.Add(new EditorMenuPlacement(path, actionID, order, separatorBefore, argument));
    }

    internal IReadOnlyList<EditorMenuPlacement> items => m_items;
    internal IReadOnlyList<EditorMenuGroupPlacement> groups => m_groups;
}

internal sealed record EditorMenuGroupPlacement(
    string path,
    int order,
    bool separatorBefore
);
