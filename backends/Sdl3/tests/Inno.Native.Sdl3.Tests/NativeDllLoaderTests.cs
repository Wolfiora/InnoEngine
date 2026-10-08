using System;
using System.Runtime.InteropServices;
using Inno.Native.LibraryLoading;
using Xunit;

namespace Inno.Native.Sdl3.Tests;

public sealed class NativeDllLoaderTests
{
#if DEBUG
    private const string C_LIBRARY_NAME = "SDL3-debug";
#else
    private const string C_LIBRARY_NAME = "SDL3-release";
#endif

    [Fact]
    public void ExplicitOwnerCanLoadTheSameLibraryMoreThanOnce()
    {
        nint first = NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME, typeof(SDL).Assembly);
        try
        {
            nint second = NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME, typeof(SDL).Assembly);
            try
            {
                Assert.NotEqual(0, NativeLibrary.GetExport(first, "SDL_GetVersion"));
                Assert.Equal(first, second);
            }
            finally
            {
                NativeLibrary.Free(second);
            }
        }
        finally
        {
            NativeLibrary.Free(first);
        }
    }

    [Fact]
    public void InvalidInputsFailBeforeResolverPublication()
    {
        Assert.Throws<ArgumentNullException>(() => NativeDllLoader.LoadNativeDll(C_LIBRARY_NAME, null!));
        Assert.Throws<ArgumentException>(() => NativeDllLoader.LoadNativeDll(" ", typeof(SDL).Assembly));
    }
}
