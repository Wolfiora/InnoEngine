using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Converts artist texture sources into validated KTX containers with BGFX texturec.
/// </summary>
public sealed class BgfxTextureTargetCompiler : ITextureTargetCompiler
{
    private readonly ToolRunner m_tools;

    /// <summary>
    /// Creates a texture compiler using frozen host tools or the application's explicit native deployment.
    /// </summary>
    /// <param name="tools">
    /// The selected tool runner, or null for application-deployed tools.
    /// </param>
    public BgfxTextureTargetCompiler(ToolRunner? tools = null) => m_tools = tools ?? new ToolRunner();

    /// <summary>
    /// Compiles the supplied source into a validated runtime artifact.
    /// </summary>
    /// <param name="source">
    /// The borrowed encoded image stream; only this toolchain materializes a short-lived compiler input.
    /// </param>
    /// <param name="colorSpace">
    /// The color transfer convention preserved in the compiled texture artifact.
    /// </param>
    /// <param name="cancellationToken">
    /// The token that cancels the operation before it commits.
    /// </param>
    /// <returns>
    /// An asynchronous operation that completes after all requested work has finished.
    /// </returns>
    public async ValueTask<byte[]> CompileKtxAsync(
        Stream source,
        TextureColorSpace colorSpace,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(source);
        if (!source.CanRead)
            throw new ArgumentException("Texture compilation requires readable encoded input.", nameof(source));
        cancellationToken.ThrowIfCancellationRequested();
        string temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            "InnoEngine",
            "Texturec",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryDirectory);
        try
        {
            string sourcePath = Path.Combine(temporaryDirectory, "source.image");
            await using (FileStream inputFile = new(sourcePath, FileMode.CreateNew, FileAccess.Write,
                             FileShare.None, 128 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan))
            {
                await source.CopyToAsync(inputFile, cancellationToken).ConfigureAwait(false);
            }
            string outputPath = Path.Combine(temporaryDirectory, "texture.ktx");
            var arguments = new List<string>
            {
                "-f", Path.GetFullPath(sourcePath),
                "-o", outputPath,
                "-t", "RGBA8",
                "--mips",
                "--validate"
            };
            if (colorSpace == TextureColorSpace.Linear)
                arguments.Add("--linear");

            ToolRunResult result = await m_tools.RunAsync(
                BgfxTool.Texturec,
                arguments,
                temporaryDirectory,
                cancellationToken).ConfigureAwait(false);
            if (!result.succeeded || !File.Exists(outputPath))
            {
                string diagnostics = string.Join(
                    Environment.NewLine,
                    new[] { result.standardOutput, result.standardError });
                throw new InvalidOperationException(
                    $"texturec rejected the encoded input with exit code {result.exitCode}: {diagnostics}");
            }

            byte[] artifact = await File.ReadAllBytesAsync(outputPath, cancellationToken)
                .ConfigureAwait(false);
            if (artifact.Length == 0)
                throw new InvalidOperationException("texturec produced an empty artifact.");
            return artifact;
        }
        finally
        {
            if (Directory.Exists(temporaryDirectory))
                Directory.Delete(temporaryDirectory, recursive: true);
        }
    }
}
