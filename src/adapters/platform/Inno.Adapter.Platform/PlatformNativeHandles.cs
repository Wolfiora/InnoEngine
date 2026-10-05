using System;

namespace Inno.Adapter.Platform;

/// <summary>
/// Borrows surface handles from a live window for cooperating native adapters.
/// </summary>
/// <remarks>
/// Handles remain valid only while their window is alive. Consumers must not release them.
/// The handle identifier selects the ABI; this value is not a runtime or scripting contract.
/// </remarks>
/// <param name="windowHandle">
/// Native window handle, or a stable UTF-8 canvas selector for browser surfaces.
/// </param>
/// <param name="displayHandle">
/// Native display handle when required by the platform.
/// </param>
/// <param name="handleKind">
/// Platform handle kind.
/// </param>
/// <returns>
/// A borrowed adapter-level surface description.
/// </returns>
public readonly record struct PlatformNativeHandles(
    IntPtr windowHandle,
    IntPtr displayHandle = default,
    PlatformNativeHandleId handleKind = default
);
