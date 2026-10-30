using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Scripting.Compiler;

namespace Inno.Build;

/// <summary>
/// Evaluates the authoring project's selected SDK and maps its framework contract into scripting inputs.
/// </summary>
public sealed class SdkScriptCompilationEnvironment : IScriptCompilationEnvironmentProvider
{
    private readonly string m_dotnetHost;
    private readonly string m_projectRoot;

    /// <summary>
    /// Creates a host-owned input provider without selecting installed reference packs independently.
    /// </summary>
    /// <param name="dotnetHost">
    /// The .NET host executable selected by the composition root.
    /// </param>
    /// <param name="projectRoot">
    /// The authoring project whose ancestor SDK configuration governs compilation and IDE use.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A host or project location is empty.
    /// </exception>
    public SdkScriptCompilationEnvironment(
        string dotnetHost,
        string projectRoot
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        ArgumentException.ThrowIfNullOrWhiteSpace(projectRoot);
        m_dotnetHost = dotnetHost;
        m_projectRoot = Path.GetFullPath(projectRoot);
    }

    /// <inheritdoc />
    public ScriptCompilationInputs Capture(CancellationToken cancellationToken = default)
        => CaptureAsync(cancellationToken).GetAwaiter().GetResult();

    private async Task<ScriptCompilationInputs> CaptureAsync(CancellationToken cancellationToken)
    {
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveDirectoryAsync(m_dotnetHost, m_projectRoot, cancellationToken).ConfigureAwait(false);
        string requests = Path.Combine(m_projectRoot, "Library", "Scripting", "SdkInputs");
        using ProcessOwnedDirectory owner = await ProcessOwnedDirectory.CreateAsync(requests, cancellationToken).ConfigureAwait(false);
        string project = Path.Combine(owner.directory, "Framework.csproj");
        await File.WriteAllTextAsync(project, """
            <Project>
              <PropertyGroup>
                <ImportDirectoryBuildProps>false</ImportDirectoryBuildProps>
                <ImportDirectoryBuildTargets>false</ImportDirectoryBuildTargets>
              </PropertyGroup>
              <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <LangVersion>14.0</LangVersion>
                <ImplicitUsings>disable</ImplicitUsings>
                <EnableDefaultItems>false</EnableDefaultItems>
              </PropertyGroup>
              <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />
              <Target Name="InitializeScriptDefines">
                <PropertyGroup><DefineConstants></DefineConstants></PropertyGroup>
              </Target>
              <Target Name="CaptureScriptFramework" DependsOnTargets="ResolveReferences;InitializeScriptDefines;AddImplicitDefineConstants" />
            </Project>
            """, cancellationToken).ConfigureAwait(false);
        _ = await RunSdkAsync(sdk,
            [sdk.cliPath, "restore", project, "--disable-build-servers", "-m:1", "-nr:false", "-v:quiet"],
            owner.directory, cancellationToken).ConfigureAwait(false);
        string output = await RunSdkAsync(sdk,
            [sdk.cliPath, "msbuild", project, "-nologo", "-m:1", "-nr:false", "-t:CaptureScriptFramework",
                "-getItem:ReferencePath", "-getProperty:TargetFramework,LangVersion,DefineConstants,NETCoreSdkVersion"],
            owner.directory, cancellationToken).ConfigureAwait(false);
        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement properties = document.RootElement.GetProperty("Properties");
        if (properties.GetProperty("TargetFramework").GetString() != "net10.0"
            || properties.GetProperty("LangVersion").GetString() != "14.0"
            || properties.GetProperty("NETCoreSdkVersion").GetString() != sdk.sdkIdentity)
            throw new InvalidDataException("SDK scripting evaluation returned an inconsistent framework, language or SDK identity.");
        return new ScriptCompilationInputs(sdk.sdkIdentity,
            document.RootElement.GetProperty("Items").GetProperty("ReferencePath").EnumerateArray()
                .Select(static item => item.GetProperty("FullPath").GetString()!),
            properties.GetProperty("DefineConstants").GetString()!.Split(';', StringSplitOptions.RemoveEmptyEntries));
    }

    private static async Task<string> RunSdkAsync(
        DotNetSdkDescriptor sdk,
        IReadOnlyList<string> arguments,
        string directory,
        CancellationToken cancellationToken
    ) {
        using var output = new StringWriter();
        using var errors = new StringWriter();
        try
        {
            await ToolchainEnvironment.RunAsync(sdk.hostPath, arguments, directory, cancellationToken,
                DotNetSdkEnvironment.Create(sdk.hostPath), output, errors).ConfigureAwait(false);
            return output.ToString();
        }
        catch (InvalidOperationException failure)
        {
            throw new InvalidOperationException($"Script framework evaluation with SDK '{sdk.sdkIdentity}' failed.\n{output}{errors}", failure);
        }
    }
}
