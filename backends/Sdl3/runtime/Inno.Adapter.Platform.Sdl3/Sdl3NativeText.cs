using System.Runtime.InteropServices;
using Inno.Native.Sdl3;

namespace Inno.Adapter.Platform.Sdl3;

internal static unsafe class Sdl3NativeText
{
    // SDL owns these strings; copy them before another call may replace their storage.
    internal static string? ReadError() => Marshal.PtrToStringUTF8((nint)SDL.Raw.GetError());

    internal static string? ReadWindowTitle(SDLWindow* window) => Marshal.PtrToStringUTF8((nint)SDL.Raw.GetWindowTitle(window));
}
