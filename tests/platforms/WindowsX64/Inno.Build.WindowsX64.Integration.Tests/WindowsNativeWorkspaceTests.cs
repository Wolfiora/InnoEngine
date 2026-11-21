using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Build.WindowsX64;

namespace Inno.Build.WindowsX64.Integration.Tests;

public sealed class WindowsNativeWorkspaceTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoWorkspaceTests", Guid.NewGuid().ToString("N"));
    private readonly string m_bindingRoot = Path.Combine(Path.GetTempPath(), "InnoWorkspaceBindings", Guid.NewGuid().ToString("N"));
    private readonly NativeComponentDescriptor m_owner = new("fixture", "native/fixture.csproj", "build/fixture.csproj");

    public WindowsNativeWorkspaceTests()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "build"));
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), "fixture");
        File.WriteAllText(Path.Combine(m_root, "build", "fixture.csproj"), "<Project />");
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    public async Task WindowsCMakeReusesUnchangedObjectsAndPublishesChangedSourceBytes()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
        NativeToolchainSelection tools = await new WindowsX64NativeToolchainProvider(
            ToolchainEnvironment.ResolveExecutable("dotnet")
        ).ResolveAsync(
            new(m_root, "release"),
            (new WindowsBuildHostProvider().TryCapture()
             ?? throw new PlatformNotSupportedException("A Windows build host is required.")),
            "windows-x64",
            CancellationToken.None
        );
        string source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(source);
        File.WriteAllText(
            Path.Combine(source, "CMakeLists.txt"),
            "cmake_minimum_required(VERSION 3.20)\nproject(WorkspaceFixture C)\nadd_library(fixture SHARED api.c stable.c)\n"
        );
        string changed = Path.Combine(source, "api.c");
        File.WriteAllText(changed, "__declspec(dllexport) int value(void) { return 41; }\n");
        File.WriteAllText(Path.Combine(source, "stable.c"), "int stable(void) { return 7; }\n");
        DateTime originalTime = File.GetLastWriteTimeUtc(changed);
        string? workspace = null;
        string? stableObject = null;
        DateTime stableTime = default;
        int compilations = 0;
        async Task Produce(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellation
        ) {
            string current = scoped.GetNativeBuildRoot(m_owner);
            string physical = new DirectoryInfo(current).LinkTarget ?? current;
            if (workspace is not null)
            {
                Assert.Equal(workspace, physical);
            }
            workspace = physical;
            string build = await NativeCMakeExecutor.BuildAsync(scoped, m_owner, source, "fixture", [], cancellation);
            File.Copy(NativeCMakeExecutor.FindOutput(scoped, build, "fixture.dll"), Path.Combine(output, "fixture.dll"));
            string objectPath = Directory.EnumerateFiles(physical, "*.obj", SearchOption.AllDirectories)
                .Single(path => Path.GetFileName(path) is "stable.obj" or "stable.c.obj");
            if (compilations++ == 0)
            {
                stableObject = objectPath;
                stableTime = File.GetLastWriteTimeUtc(objectPath);
            }
            else
            {
                Assert.Equal(stableObject, objectPath);
                Assert.Equal(stableTime, File.GetLastWriteTimeUtc(objectPath));
            }
        }
        NativeBuildContext Context()
            => new NativeBuildContext(m_root, "release").WithToolchain(tools)
                .WithComponentOptions(m_owner, new(NativeLibraryKind.Shared));
        NativeBuildRecipe Recipe()
            => new(
                m_owner,
                "fixture",
                "windows-x64",
                new[] { source }.Concat(tools.inputPaths).Select(path => NativeBuildInput.FromPath(m_root, path)),
                tools.declarations.Concat(new[] { "shared", "fixture" })
            );
        using NativeBuildProduct first = await NativeArtifactPublisher.PublishAsync(Context(), Recipe(), Produce);
        Assert.Equal(41, Read(first));
        File.WriteAllText(changed, "__declspec(dllexport) int value(void) { return 42; }\n");
        File.SetLastWriteTimeUtc(changed, originalTime);
        using NativeBuildProduct second = await NativeArtifactPublisher.PublishAsync(Context(), Recipe(), Produce);
        Assert.NotEqual(first.fingerprint, second.fingerprint);
        Assert.Equal(42, Read(second));
        Assert.Equal(41, Read(first));
        NativeBuildContext hitContext = Context();
        using NativeBuildProduct hit = await NativeArtifactPublisher.PublishAsync(hitContext, Recipe(), Produce);
        Assert.Equal(second.fingerprint, hit.fingerprint);
        Assert.Equal(2, compilations);
        Assert.Equal(0, hitContext.statistics.nativeProcesses);
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
        if (Directory.Exists(m_bindingRoot))
        {
            Directory.Delete(m_bindingRoot, recursive: true);
        }
    }

    private static int Read(NativeBuildProduct product)
    {
        nint library = NativeLibrary.Load(product.files.Single());
        try
        {
            return Marshal.GetDelegateForFunctionPointer<ReadNativeValue>(NativeLibrary.GetExport(library, "value"))();
        }
        finally
        {
            NativeLibrary.Free(library);
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int ReadNativeValue();
}
