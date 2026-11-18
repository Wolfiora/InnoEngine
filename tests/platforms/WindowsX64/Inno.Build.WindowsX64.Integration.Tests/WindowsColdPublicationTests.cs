using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Build.WindowsX64;
using Inno.Core.IO;

namespace Inno.Build.WindowsX64.Integration.Tests;

public sealed class WindowsColdPublicationTests : IDisposable
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int ReadNativeValue();

    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativePublication", Guid.NewGuid().ToString("N"));

    [Fact]
    [SupportedOSPlatform("windows")]
    public async Task WindowsColdCMakeBuildCompilesCopiedInputsEvenWhenOriginalBytesAreTemporarilyChanged()
    {
        if (!OperatingSystem.IsWindows())
            return;
        NativeBuildContext unresolved = CreateContext();
        NativeToolchainSelection tools = await new WindowsX64NativeToolchainProvider(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet")).ResolveAsync(
            unresolved, (new WindowsBuildHostProvider().TryCapture() ?? throw new PlatformNotSupportedException("A Windows build host is required.")), "windows-x64", CancellationToken.None);
        NativeBuildContext context = unresolved.WithToolchain(tools).WithComponentOptions(
            new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"),
            new NativeComponentBuildOptions(NativeLibraryKind.Shared));
        string source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(source);
        File.WriteAllText(Path.Combine(source, "CMakeLists.txt"),
            "cmake_minimum_required(VERSION 3.20)\nproject(ColdFixture C)\n"
            + "configure_file(\"${INNO_FIXTURE_INPUT}\" \"${CMAKE_CURRENT_BINARY_DIR}/answer.h\" COPYONLY)\n"
            + "add_library(fixture SHARED api.c)\n"
            + "target_include_directories(fixture PRIVATE \"${CMAKE_CURRENT_BINARY_DIR}\")\n");
        File.WriteAllText(Path.Combine(source, "api.c"), "#include \"answer.h\"\n__declspec(dllexport) int value(void) { return ANSWER; }\n");
        string header = Path.Combine(source, "answer.h");
        File.WriteAllText(header, "#define ANSWER 42\n");
        DateTime timestamp = File.GetLastWriteTimeUtc(header);

        using NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "windows-x64", [source], []), async (
                @scoped,
                output,
                cancellation
            ) => {
                var owner = new NativeComponentDescriptor("fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj");
                NativeCMakeSource copied = await NativeCMakeExecutor.PrepareSourceAsync(scoped, owner, cancellation);
                Assert.Same(copied, await NativeCMakeExecutor.PrepareSourceAsync(scoped.WithToolchain(tools), owner, cancellation));
                Assert.NotEqual(source, copied.ResolvePath(source));
                Assert.Throws<InvalidOperationException>(() => copied.ResolvePath(Path.Combine(source, "undeclared.h")));
                Assert.Equal("-DCMAKE_INSTALL_PREFIX=" + output, copied.ResolveDefinition("-DCMAKE_INSTALL_PREFIX=" + output));
                File.WriteAllText(header, "#define ANSWER 17\n");
                File.SetLastWriteTimeUtc(header, timestamp);
                try
                {
                    Assert.Equal("#define ANSWER 42\n", File.ReadAllText(copied.ResolvePath(header)));
                    string directory = await NativeCMakeExecutor.BuildAsync(scoped, owner, source,
                        "fixture", ["-DINNO_FIXTURE_INPUT=" + header], cancellation);
                    File.Copy(NativeCMakeExecutor.FindOutput(scoped, directory, "fixture.dll"), Path.Combine(output, "fixture.dll"));
                }
                finally
                {
                    File.WriteAllText(header, "#define ANSWER 42\n");
                    File.SetLastWriteTimeUtc(header, timestamp);
                }
            });

        Assert.Single(product.files);
        Assert.True(new FileInfo(product.files[0]).Length > 0);
        nint library = NativeLibrary.Load(product.files[0]);
        try
        {
            ReadNativeValue read = Marshal.GetDelegateForFunctionPointer<ReadNativeValue>(NativeLibrary.GetExport(library, "value"));
            Assert.Equal(42, read());
        }
        finally
        {
            NativeLibrary.Free(library);
        }
        Assert.True(context.statistics.materializedBytes > 0);
        Assert.Empty(Directory.EnumerateDirectories(Path.Combine(m_root, "artifacts"), "*.staging-*", SearchOption.AllDirectories));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private static NativeBuildRecipe CreateRecipe(
        NativeBuildContext context,
        NativeComponentDescriptor owner,
        string component,
        string targetId,
        IReadOnlyList<string> inputPaths,
        IReadOnlyList<string> declarations
    ) => new(owner, component, targetId,
        inputPaths.Concat(context.toolchain?.inputPaths ?? [])
            .Select(path => NativeBuildInput.FromPath(context.engineRoot, path)),
        declarations.Concat(context.toolchain?.declarations ?? []));

    private NativeBuildContext CreateContext()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        string projectName = "Inno.Build.Toolchains";
        string owner = Path.Combine(m_root, "build", "toolchains", projectName);
        Directory.CreateDirectory(owner);
        File.WriteAllText(Path.Combine(owner, projectName + ".csproj"), "<Project />");
        return new(m_root, "release");
    }
}
