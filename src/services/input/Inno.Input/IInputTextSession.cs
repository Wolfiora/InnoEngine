using System;

using Inno.Core.Mathematics;

namespace Inno.Input;

/// <summary>
/// Exclusively owns system text input for the current runtime input route.
/// </summary>
/// <remarks>
/// Use and dispose the session on the input owner thread. Focus loss, route replacement and runtime retirement
/// invalidate it. Retain no session across a runtime or plugin generation; inactive disposal is harmless.
/// </remarks>
public interface IInputTextSession : IDisposable
{
    /// <summary>
    /// Gets whether the session still owns text input on its original route.
    /// </summary>
    bool isActive { get; }

    /// <summary>
    /// Updates the preferred input-method area without restarting composition.
    /// </summary>
    /// <param name="area">
    /// Finite positive bounds in the same logical coordinate space as the runtime input snapshot.
    /// The view owner must map rendered UI coordinates into this space before submitting them.
    /// </param>
    /// <param name="cursorOffset">
    /// Finite horizontal insertion offset from the area's left edge, in the same units.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The bounds or insertion offset are invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The session is inactive, the thread is wrong or the platform rejects the placement.
    /// </exception>
    void SetArea(
        Rect area,
        float cursorOffset
    );
}
