using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
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

    [Fact]
    [Trait("ProcessIsolation", "Required")]
    [SupportedOSPlatform("windows")]
    public void ExplicitDeploymentLoadsSdlNearTheWindowsPathLimit()
    {
        if (OperatingSystem.IsWindows())
            LoadSdlFromDeepOutputDirectory(252, extendedRoot: false);
    }

    [Fact]
    [Trait("ProcessIsolation", "Required")]
    [SupportedOSPlatform("windows")]
    public void ExplicitDeploymentLoadsSdlBeyondTheWindowsPathLimit()
    {
        if (OperatingSystem.IsWindows())
            LoadSdlFromDeepOutputDirectory(300, extendedRoot: false);
    }

    [Fact]
    [Trait("ProcessIsolation", "Required")]
    [SupportedOSPlatform("windows")]
    public void ExplicitDeploymentLoadsSdlWithAnExtendedWindowsRoot()
    {
        if (OperatingSystem.IsWindows())
            LoadSdlFromDeepOutputDirectory(300, extendedRoot: true);
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

    [SupportedOSPlatform("windows")]
    private static void LoadSdlFromDeepOutputDirectory(
        int pathLength,
        bool extendedRoot
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoSdlDeployment", Guid.NewGuid().ToString("N"));
        string source = NativeDllLoader.FindNativeLibrary(C_LIBRARY_NAME);
        string fileName = Path.GetFileName(source);
        string directory = Path.Combine(root, new string('a', pathLength - root.Length - fileName.Length - 2));
        Directory.CreateDirectory(directory);
        try
        {
            string payload = Path.Combine(directory, fileName);
            File.Copy(source, payload);
            Assert.Equal(pathLength, payload.Length);
            var deployment = new NativeDeploymentIndex("windows-x64",
                new Dictionary<string, string> { [C_LIBRARY_NAME] = fileName }, new Dictionary<string, string>());
            string selectedRoot = extendedRoot ? @"\\?\" + directory : directory;
            string library = NativeDeploymentIndex.Decode(deployment.Encode()).ResolveLibrary(selectedRoot, C_LIBRARY_NAME);
            nint module = NativeLibrary.Load(library, typeof(SDL).Assembly, null);
            try
            {
                Assert.NotEqual(0, NativeLibrary.GetExport(module, "SDL_GetVersion"));
            }
            finally
            {
                NativeLibrary.Free(module);
            }
            File.Delete(payload);
            Assert.Throws<FileNotFoundException>(() => deployment.ResolveLibrary(selectedRoot, C_LIBRARY_NAME));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
