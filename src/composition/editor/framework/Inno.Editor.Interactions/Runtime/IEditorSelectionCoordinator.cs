using System;
using Inno.Core.Identity;
using Inno.Scripting.Api;

namespace Inno.Editor.Interactions;

/// <summary>
/// Coordinates host-owned selection and the Core identity lifetime of temporary presentations.
/// </summary>
public interface IEditorSelectionCoordinator
{
    /// <summary>
    /// Registers the Core identity domain used by a temporary scene presentation.
    /// </summary>
    /// <param name="allocator">
    /// The borrowed allocator owned by that presentation's runtime session.
    /// </param>
    /// <returns>
    /// A routing lease released after restoring the preceding selection.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The allocator is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The domain is already active or the caller is not the owner thread.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The coordinator has retired.
    /// </exception>
    [ScriptingApiIgnore]
    IDisposable RegisterIdentityDomain(IdentityAllocator allocator);

    /// <summary>
    /// Gets the currently selected target, or <see langword="null"/> when selection is empty.
    /// </summary>
    object? selectedTarget { get; }

    /// <summary>
    /// Replaces the current target after closing presentations owned by another target.
    /// </summary>
    /// <param name="target">
    /// The new target, or <see langword="null"/> to clear selection.
    /// </param>
    void SetSelection(object? target);
}
