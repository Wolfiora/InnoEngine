using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Windows.Host;
using Inno.Build.WindowsX64;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeBindingCMakeTests
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int ReadValue();

    [WindowsCompilerFact]
    public async Task WindowsCompilationUsesFrozenHeadersAndKeepsBindingDefinesOutOfUpstreamSources()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoBindingCMake", Guid.NewGuid().ToString("N"));
        string headerRoot = Path.Combine(Path.GetTempPath(), "InnoBindingHeader", Guid.NewGuid().ToString("N"));
        string header = Path.Combine(headerRoot, "api.h");
        string hidden = header + ".hidden";
        string cmakeHeader = header.Replace(Path.DirectorySeparatorChar, '/');
        string cmakeHidden = hidden.Replace(Path.DirectorySeparatorChar, '/');
        Directory.CreateDirectory(Path.Combine(root, "Source", "upstream"));
        Directory.CreateDirectory(headerRoot);
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
            Directory.CreateDirectory(Path.Combine(root, "build"));
            File.WriteAllText(Path.Combine(root, "build", "fixture.csproj"), "<Project />");
            File.WriteAllText(header, "#define INNO_HEADER_VALUE 41\n");
            using ToolchainWorkingDirectory sourceAlias = await new WindowsToolchainWorkspaceProvider().OpenAsync(root);
            string source = Path.Combine(sourceAlias.toolPath, "Source");
            File.WriteAllText(Path.Combine(source, "CMakeLists.txt"), """
                cmake_minimum_required(VERSION 3.20)
                project(BindingFixture C)
                add_subdirectory(upstream)
                """);
            File.WriteAllText(Path.Combine(source, "upstream", "CMakeLists.txt"), $$"""
                add_library(fixture SHARED original.c)
                add_custom_command(TARGET fixture PRE_BUILD
                    COMMAND "${CMAKE_COMMAND}" -E rename "{{cmakeHeader}}" "{{cmakeHidden}}")
                add_custom_command(TARGET fixture POST_BUILD
                    COMMAND "${CMAKE_COMMAND}" -E rename "{{cmakeHidden}}" "{{cmakeHeader}}")
                """);
            File.WriteAllText(Path.Combine(source, "upstream", "original.c"), """
                #ifdef INNO_BINDING_ONLY
                #error Binding definitions escaped into the upstream translation unit.
                #endif
                __declspec(dllexport) int upstream(void) { return 37; }
                """);
            string artifact = Path.Combine(root, "Generation");
            string native = Path.Combine(artifact, "Native");
            string managed = Path.Combine(artifact, "Generated", "Bindings.cs");
            Directory.CreateDirectory(native);
            Directory.CreateDirectory(Path.GetDirectoryName(managed)!);
            File.WriteAllText(managed, "// Neutral binding fixture.\n");
            string unit = Path.Combine(native, "binding.c");
            File.WriteAllText(unit, """
                #ifndef INNO_BINDING_ONLY
                #error The binding translation unit lost its required definitions.
                #endif
                #include "api.h"
                __declspec(dllexport) int value(void) { return INNO_HEADER_VALUE; }
                """);
            var compilation = new NativeBindingCompilation
            {
                roots = [new("generated", "", true, true), new("user", headerRoot, false, true)],
                files = [Input("generated", "binding.c", unit, true), Input("user", "api.h", header, false)],
                includeDirectories = [new("user", "", false, false)],
                defines = ["INNO_BINDING_ONLY=1"], compilerDefines = [], cxxStandard = 0, cStandard = 17,
                requiredExports = ["value", "upstream"], staticExportSymbols = [], staticExportAbi = "", staticExportNames = []
            };
            File.WriteAllBytes(Path.Combine(native, "compilation.json"), JsonSerializer.SerializeToUtf8Bytes(compilation));
            var owner = new NativeComponentDescriptor("fixture", "native/fixture.csproj", "build/fixture.csproj",
                bindingDefinition: "native/Bindings/bindings.props");
            var descriptor = new NativeBindingGenerationDescriptor
            {
                fingerprint = new string('a', 64), artifactDirectory = artifact, artifactOwnerDirectory = root,
                bindingsPath = managed, nativeDirectory = native
            };
            using var bindings = new NativeBindingGenerationResult(
                new Dictionary<string, NativeBindingGenerationDescriptor> { [owner.nativeProject] = descriptor }, []);
            var tools = await new WindowsX64NativeToolchainProvider(ToolchainEnvironment.ResolveExecutable("dotnet"))
                .ResolveAsync(new(root, "release"), BuildTestProducts.WindowsHost(), "windows-x64", CancellationToken.None);
            NativeBuildContext context = new NativeBuildContext(sourceAlias.toolPath, "release").WithToolchain(tools).WithBindings(bindings)
                .WithComponentOptions(owner, new(NativeLibraryKind.Shared));
            var recipe = new NativeBuildRecipe(owner, "fixture", "windows-x64",
                new[] { source }.Concat(tools.inputPaths).Select(path => NativeBuildInput.FromPath(root, path)), tools.declarations);
            using NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(context, recipe,
                async (
                    operation,
                    output,
                    cancellation
                ) => {
                    NativeCMakeSource inputs = await NativeCMakeExecutor.PrepareSourceAsync(operation, owner, cancellation);
                    string frozenHeader = inputs.ResolvePath(header);
                    Assert.NotEqual(header, frozenHeader);
                    Assert.Equal(File.ReadAllBytes(header), File.ReadAllBytes(frozenHeader));
                    string build;
                    try
                    {
                        build = await NativeCMakeExecutor.BuildAsync(operation, owner, source, "fixture", [], cancellation);
                    }
                    catch (InvalidOperationException exception)
                    {
                        string requirements = Path.Combine(operation.GetNativeBuildRoot(owner), "CMake", "BindingInputs.cmake");
                        throw new Xunit.Sdk.XunitException(exception + Environment.NewLine + File.ReadAllText(requirements));
                    }
                    File.Copy(NativeCMakeExecutor.FindOutput(operation, build, "fixture.dll"), Path.Combine(output, "fixture.dll"));
                });
            Assert.True(File.Exists(header));
            Assert.False(File.Exists(hidden));
            nint library = NativeLibrary.Load(Assert.Single(product.files));
            try
            {
                Assert.Equal(41, Marshal.GetDelegateForFunctionPointer<ReadValue>(NativeLibrary.GetExport(library, "value"))());
                Assert.Equal(37, Marshal.GetDelegateForFunctionPointer<ReadValue>(NativeLibrary.GetExport(library, "upstream"))());
            }
            finally
            {
                NativeLibrary.Free(library);
            }
        }
        finally
        {
            if (File.Exists(hidden))
                File.Move(hidden, header, overwrite: true);
            Directory.Delete(root, recursive: true);
            Directory.Delete(headerRoot, recursive: true);
        }
    }

    private static NativeBindingSourceFile Input(
        string rootId,
        string relative,
        string path,
        bool compile
    ) {
        byte[] content = File.ReadAllBytes(path);
        return new(rootId, relative, content.Length, Convert.ToHexStringLower(SHA256.HashData(content)), compile);
    }

    private sealed class WindowsCompilerFactAttribute : FactAttribute
    {
        public WindowsCompilerFactAttribute()
        {
            if (!OperatingSystem.IsWindows())
                Skip = "Requires the Windows MSVC toolchain and its path-length contract.";
        }
    }
}
