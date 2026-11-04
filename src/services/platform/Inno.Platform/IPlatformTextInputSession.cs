using System;

using Inno.Core.Mathematics;

namespace Inno.Platform;

/// <summary>
/// Exclusively owns system text input for one focused platform window.
/// </summary>
/// <remarks>
/// All operations use the window owner thread. Focus loss, hiding or window retirement invalidates the session.
/// Disposing an invalidated session is harmless and cannot stop a subsequently acquired session.
/// </remarks>
public interface IPlatformTextInputSession : IDisposable
{
    /// <summary>
    /// Gets whether this session still owns system text input.
    /// </summary>
    bool isActive { get; }

    /// <summary>
    /// Updates input-method placement without restarting the composition.
    /// </summary>
    /// <param name="area">
    /// Finite positive editable bounds in window client logical coordinates.
    /// </param>
    /// <param name="cursorOffset">
    /// Horizontal caret offset from the area's left edge, in the same coordinate units.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The area or cursor offset is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The session is inactive, the thread is wrong or the platform rejects the placement.
    /// </exception>
    void SetArea(
        Rect area,
        float cursorOffset
    );
}
