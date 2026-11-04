using System;

namespace Inno.UI;

/// <summary>
/// Owns one context-local image source. Dispose on the UI owner thread to invalidate that
/// source and retire its UI texture handles; shared Rendering assets remain owned by Rendering.
/// </summary>
/// <remarks>
/// The source is ephemeral and must not be serialized. Register a replacement before updating
/// document references, then dispose the old registration. Context retirement also retires all
/// its registrations; subsequent disposal is harmless on the owner thread.
/// </remarks>
public interface IUiImageRegistration : IDisposable
{
    /// <summary>
    /// Gets the opaque source URI accepted by document image elements and styles.
    /// </summary>
    string source { get; }
}
