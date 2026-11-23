using System;

namespace Inno.Editor.Rendering;

/// <summary>
/// Declares navigation operations supported by one viewport interaction controller.
/// </summary>
[Flags]
public enum EditorViewportNavigationCapabilities
{
    /// <summary>
    /// Disables host-owned navigation.
    /// </summary>
    None = 0,
    /// <summary>
    /// Allows translating the view parallel to its image plane.
    /// </summary>
    Pan = 1 << 0,
    /// <summary>
    /// Allows changing orthographic size or perspective focus distance.
    /// </summary>
    Zoom = 1 << 1,
    /// <summary>
    /// Allows the complete plane-oriented pan and zoom interaction model.
    /// </summary>
    Planar = Pan | Zoom,
    /// <summary>
    /// Allows pivot-oriented orbit and dolly.
    /// </summary>
    Orbit = 1 << 2,
    /// <summary>
    /// Allows free-look movement.
    /// </summary>
    Fly = 1 << 3,
    /// <summary>
    /// Allows framing a provider-supplied or host-derived selection bound.
    /// </summary>
    FrameSelection = 1 << 4
}
