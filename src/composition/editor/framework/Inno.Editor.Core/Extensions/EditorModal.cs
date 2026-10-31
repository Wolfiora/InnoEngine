using System.Numerics;

using Inno.Scripting.Api;

namespace Inno.Editor.Core;

/// <summary>
/// Defines non-dockable modal editor content.
/// </summary>
public abstract class EditorModal
{
    /// <summary>
    /// Gets whether the modal should currently be visible.
    /// </summary>
    public abstract bool isVisible { get; }

    /// <summary>
    /// Gets whether the modal prevents interaction with regular editor views.
    /// </summary>
    public virtual bool blocksInteraction => true;

    /// <summary>
    /// Gets whether the modal window can be moved inside the main viewport.
    /// </summary>
    public virtual bool canMove => false;

    /// <summary>
    /// Gets whether the modal window can be resized by the user.
    /// </summary>
    public virtual bool canResize => false;

    /// <summary>
    /// Gets whether the modal window owns scrolling when its content overflows.
    /// Disable this when a child region owns all scrolling.
    /// </summary>
    public virtual bool allowScrolling => true;

    /// <summary>
    /// Gets the initial modal size in unscaled editor units, or <see cref="Vector2.Zero"/>
    /// when the runtime should size the modal from its content.
    /// </summary>
    public virtual Vector2 initialSize => Vector2.Zero;

    /// <summary>
    /// Gets the minimum modal size in unscaled editor units, or <see cref="Vector2.Zero"/>
    /// when no explicit minimum is required.
    /// </summary>
    public virtual Vector2 minimumSize => Vector2.Zero;

    /// <summary>
    /// Draws the modal body inside the runtime-managed centered window.
    /// </summary>
    /// <param name="context">
    /// The shared editor context containing current project and frame state.
    /// </param>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when <paramref name="context"/> is <see langword="null"/>.
    /// </exception>
    [ScriptingApiIgnore]
    public void Draw(EditorContext context)
    {
        System.ArgumentNullException.ThrowIfNull(context);
        OnDraw(context);
    }

    /// <summary>
    /// Captures presentation state before submitting content. Content-sized windows prepare before opening;
    /// explicitly sized resizable windows prepare after current input establishes their content bounds.
    /// </summary>
    /// <param name="context">
    /// The owner-thread editor context for this frame.
    /// </param>
    /// <param name="availableContentSize">
    /// The bounded content area in current presentation coordinates, already scaled.
    /// </param>
    /// <returns>
    /// The required content size, or null for an explicitly sized window.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// The context is null.
    /// </exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// The available extent is not finite and positive.
    /// </exception>
    [ScriptingApiIgnore]
    public Vector2? Prepare(
        EditorContext context,
        Vector2 availableContentSize
    ) {
        System.ArgumentNullException.ThrowIfNull(context);
        if (!float.IsFinite(availableContentSize.X) || !float.IsFinite(availableContentSize.Y) ||
            availableContentSize.X <= 0f || availableContentSize.Y <= 0f)
            throw new System.ArgumentOutOfRangeException(nameof(availableContentSize));
        return OnPrepare(context, availableContentSize);
    }

    /// <summary>
    /// Draws the modal body inside the runtime-managed centered window.
    /// </summary>
    /// <param name="context">
    /// The shared editor context containing current project and frame state.
    /// </param>
    protected abstract void OnDraw(EditorContext context);

    /// <summary>
    /// Captures the state consumed by OnDraw and measures it without drawing or performing domain operations.
    /// Content-sized modals must return a finite, nonnegative size. Fixed-size modals may return null.
    /// </summary>
    /// <param name="context">
    /// The current owner-thread context.
    /// </param>
    /// <param name="availableContentSize">
    /// The bounded, already scaled content area.
    /// </param>
    /// <returns>
    /// The content extent required by the captured state, or null for explicit sizing.
    /// </returns>
    protected virtual Vector2? OnPrepare(
        EditorContext context,
        Vector2 availableContentSize
    ) => null;
}
