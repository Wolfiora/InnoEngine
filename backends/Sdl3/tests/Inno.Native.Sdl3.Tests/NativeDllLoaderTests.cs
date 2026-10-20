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
    public void ExplicitDeploymentSupportsIndependentLibraryOwners()
    {
        string libraryPath = NativeDllLoader.FindNativeLibrary(C_LIBRARY_NAME);
        nint first = NativeLibrary.Load(libraryPath);
        try
        {
            nint second = NativeLibrary.Load(libraryPath);
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

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(".")]
    [InlineData("..")]
    [InlineData("native/SDL3")]
    [InlineData("native\\SDL3")]
    [InlineData("SDL*")]
    [InlineData("SDL?")]
    public void InvalidNamesFailBeforeDeploymentLookup(string libraryName)
    {
        ArgumentException failure = Assert.Throws<ArgumentException>(() => NativeDllLoader.FindNativeLibrary(libraryName));
        Assert.Equal(nameof(libraryName), failure.ParamName);
    }

    [Fact]
    public void NullNamesFailBeforeDeploymentLookup()
    {
        ArgumentNullException failure = Assert.Throws<ArgumentNullException>(() => NativeDllLoader.FindNativeLibrary(null!));
        Assert.Equal("libraryName", failure.ParamName);
    }
}
