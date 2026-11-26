using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Testing.Source;

namespace Inno.Build.ProductProfiles.Integration.Tests;

public sealed class ApiReferenceBuildTests
{
    [Theory]
    [InlineData("Bgfx", "Inno.Adapter.Rendering.Bgfx", "CompileCompositionShaderGraphs", "InnoBgfxShaderApis", true)]
    [InlineData("Bgfx", "Inno.Adapter.Rendering.Bgfx", "CompileCompositionShaderGraphs", "InnoBgfxShaderApis", false)]
    [InlineData("ImGui", "Inno.Adapter.Presentation.ImGui.Bgfx", "CompileImGuiShaderGraphs", "InnoImGuiShaderApis", true)]
    [InlineData("ImGui", "Inno.Adapter.Presentation.ImGui.Bgfx", "CompileImGuiShaderGraphs", "InnoImGuiShaderApis", false)]
    public async Task ShaderPreparationDependsOnBuildPurpose(
        string backend,
        string project,
        string target,
        string apiProperty,
        bool referenceBuild
    ) {
        using ProcessOwnedDirectory outputs = await ProcessOwnedDirectory.CreateAsync(
            Path.Combine(Path.GetTempPath(), "InnoApiReferenceTests")
        );
        List<string> arguments =
        [
            Path.Combine("backends", backend, "runtime", project, project + ".csproj"),
            "-t:" + target,
            "-p:InnoNativeTarget=api-contract-fixture",
            "-p:" + apiProperty + "=",
            "-p:ArtifactsPath=" + outputs.directory
        ];
        if (referenceBuild)
        {
            arguments.Add("-p:InnoBuildPurpose=ApiReferences");
            await RunAsync(arguments);
            return;
        }
        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() => RunAsync(arguments));
        Assert.Contains("explicit shader platform", failure.Message);
    }

    [Theory]
    [InlineData("ValidateApiReferenceOutputOwner", "explicit ArtifactsPath")]
    [InlineData("RejectApiReferenceProductPublication", "cannot publish a runnable product")]
    public async Task ReferenceBuildRejectsInvalidOutput(
        string target,
        string diagnostic
    ) {
        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() => RunAsync(
            ["src/foundation/core/Inno.Core.IO/Inno.Core.IO.csproj", "-t:" + target, "-p:InnoBuildPurpose=ApiReferences"]
        ));
        Assert.Contains(diagnostic, failure.Message);
    }

    private static async Task RunAsync(IReadOnlyList<string> arguments)
    {
        string root = TestRepository.GetRoot(typeof(ApiReferenceBuildTests).Assembly);
        string sdk = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        await ToolchainEnvironment.CaptureOutputAsync(
            sdk,
            ["msbuild", "-nologo", "-nr:false", "-m:1", ..arguments],
            root,
            DotNetSdkEnvironment.Create(sdk),
            cancellation.Token
        );
    }
}
