using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Rendering.Assets;
using Inno.Rendering;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Compiles common Shader IR stages with the BGFX shaderc toolchain.
/// </summary>
public sealed partial class BgfxShadercToolchain : IShaderCompilerToolchain
{
    private readonly BgfxShaderTargetProfile m_targetPlatform;
    private readonly ToolRunner m_tools;

    /// <summary>
    /// Creates a compiler for one explicit offline target platform.
    /// </summary>
    /// <param name="targetPlatform">
    /// Target platform whose shaderc profiles are required.
    /// </param>
    /// <param name="tools">
    /// Frozen host-selected executables, or null to use the application's explicit native deployment.
    /// </param>
    public BgfxShadercToolchain(
        BgfxShaderTargetProfile targetPlatform,
        ToolRunner? tools = null
    ) {
        ArgumentNullException.ThrowIfNull(targetPlatform);
        m_targetPlatform = targetPlatform;
        m_tools = tools ?? new ToolRunner();
    }

    /// <summary>
    /// Creates a target using this implementation's validated inputs.
    /// </summary>
    /// <param name="capabilities">
    /// The immutable graphics capabilities used to validate the operation.
    /// </param>
    /// <param name="optimize">
    /// Whether the compiler should optimize the generated runtime artifact.
    /// </param>
    /// <param name="debugInformation">
    /// Whether the compiler should retain target debug information.
    /// </param>
    /// <returns>
    /// The validated shader compile target that represents the completed operation.
    /// </returns>
    public ShaderCompileTarget CreateTarget(
        GraphicsCapabilities capabilities,
        bool optimize = true,
        bool debugInformation = false
    ) {
        BgfxShaderCompilerProfile profile = m_targetPlatform.Resolve(capabilities);
        return new ShaderCompileTarget(m_targetPlatform.GetKey(capabilities), capabilities, optimize, debugInformation);
    }

    private async ValueTask<BgfxShadercResult> RunCompilerAsync(
        string source,
        string? varying,
        ShaderStage stage,
        ShaderCompileTarget target,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        BgfxShaderCompilerProfile profile = m_targetPlatform.Resolve(target.capabilities);
        if (!string.Equals(m_targetPlatform.GetKey(target.capabilities), target.profileKey, StringComparison.Ordinal))
        {
            throw new ArgumentException(
                $"Shader target '{target.profileKey}' does not belong to this BGFX toolchain.",
                nameof(target));
        }

        string temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            "InnoEngine",
            "Shaderc",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryDirectory);
        try
        {
            string sourcePath = Path.Combine(temporaryDirectory, "stage.sc");
            string outputPath = Path.Combine(temporaryDirectory, "stage.bin");
            string bgfxShaderInclude = ResolveBgfxShaderInclude();
            File.Copy(
                bgfxShaderInclude,
                Path.Combine(temporaryDirectory, "bgfx_shader.sh"),
                overwrite: true);
            File.Copy(
                Path.Combine(
                    Path.GetDirectoryName(bgfxShaderInclude)
                        ?? throw new InvalidOperationException("BGFX shader include has no parent directory."),
                    "bgfx_compute.sh"),
                Path.Combine(temporaryDirectory, "bgfx_compute.sh"),
                overwrite: true);
            await File.WriteAllTextAsync(
                sourcePath,
                source,
                cancellationToken).ConfigureAwait(false);

            List<string> arguments =
            [
                "-f",
                sourcePath,
                "-o",
                outputPath,
                "--type",
                ToShadercStage(stage),
                "--platform",
                profile.shadercPlatform,
                "--profile",
                profile.GetStageProfile(stage),
                "-i",
                temporaryDirectory
            ];
            if (stage != ShaderStage.Compute && varying is not null)
            {
                string varyingPath = Path.Combine(temporaryDirectory, "varying.def.sc");
                await File.WriteAllTextAsync(
                    varyingPath,
                    varying,
                    cancellationToken).ConfigureAwait(false);
                arguments.Add("--varyingdef");
                arguments.Add(varyingPath);
            }

            if (profile.defines.Count != 0)
            {
                arguments.Add("--define");
                arguments.Add(string.Join(";", profile.defines));
            }

            if (target.optimize)
            {
                arguments.Add("-O");
                arguments.Add("3");
            }

            if (target.debugInformation)
                arguments.Add("--debug");
            arguments.Add("--keepcomments");

            ToolRunResult result = await m_tools.RunAsync(
                BgfxTool.Shaderc,
                arguments,
                temporaryDirectory,
                cancellationToken).ConfigureAwait(false);
            byte[]? bytes = result.succeeded && File.Exists(outputPath)
                ? await File.ReadAllBytesAsync(outputPath, cancellationToken).ConfigureAwait(false)
                : null;
            return new BgfxShadercResult(
                bytes,
                result.exitCode,
                result.standardOutput,
                result.standardError);
        }
        finally
        {
            if (Directory.Exists(temporaryDirectory))
                Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    private static string ToShadercStage(ShaderStage stage)
        => stage switch
        {
            ShaderStage.Vertex => "vertex",
            ShaderStage.Fragment => "fragment",
            ShaderStage.Compute => "compute",
            _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, "A single shader stage is required.")
        };

    private static string ResolveBgfxShaderInclude()
    {
        string owner = Path.GetDirectoryName(typeof(BgfxShadercToolchain).Assembly.Location) ?? AppContext.BaseDirectory;
        string deployed = Path.Combine(
            owner,
            "native",
            "bgfx",
            "includes",
            "bgfx_shader.sh");
        if (File.Exists(deployed))
            return deployed;

        string[] starts = [owner, AppContext.BaseDirectory, Directory.GetCurrentDirectory()];
        foreach (string start in starts)
        {
            for (DirectoryInfo? directory = new(start); directory is not null; directory = directory.Parent)
            {
                string candidate = Path.Combine(
                    directory.FullName,
                    "extern",
                    "bgfx",
                    "src",
                    "bgfx_shader.sh");
                if (File.Exists(candidate))
                    return candidate;
            }
        }

        throw new FileNotFoundException(
            "Unable to resolve the BGFX shader include 'bgfx_shader.sh' from the application or repository root.");
    }
}

internal sealed record BgfxShadercResult(
    byte[]? bytes,
    int exitCode,
    string standardOutput,
    string standardError
);
