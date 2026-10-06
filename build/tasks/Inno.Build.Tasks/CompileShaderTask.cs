using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;
using Microsoft.Build.Framework;
using Inno.Core.IO;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.Tasks;

/// <summary>
/// Connects MSBuild to the same graph compiler used by the unified build workflow.
/// </summary>
public sealed class CompileShaderTask : BuildTask, ICancelableTask
{
    private readonly object m_gate = new();
    private CancellationTokenSource? m_cancellation;
    private bool m_canceled;

    /// <summary>
    /// Gets or sets the checkout whose native offline tools must match this build.
    /// </summary>
    [Required]
    public string EngineRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the managed and native tool configuration.
    /// </summary>
    public string Configuration { get; set; } = "Release";

    /// <summary>
    /// Gets or sets the authoring source directory.
    /// </summary>
    [Required]
    public string AssetRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source-local shader graph path.
    /// </summary>
    [Required]
    public string ShaderPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the graphics compiler target platform.
    /// </summary>
    [Required]
    public string Platform { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the graphics API identifier.
    /// </summary>
    [Required]
    public string Renderer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the compiled artifact destination.
    /// </summary>
    [Required]
    public string OutputFile { get; set; } = string.Empty;

    /// <summary>
    /// Compiles the requested graph artifact and reports compilation failures through MSBuild.
    /// </summary>
    /// <returns>
    /// True after writing the artifact; false when validation or compilation fails.
    /// </returns>
    public override bool Execute()
    {
        using CancellationTokenSource cancellation = new();
        lock (m_gate)
        {
            m_cancellation = cancellation;
            if (m_canceled)
                cancellation.Cancel();
        }
        try
        {
            BuildTaskHostRetirement.Inspect(EngineRoot);
            if (!Enum.TryParse(Platform, out BgfxShaderTargetPlatform platform)
                || !GraphicsApi.TryParse(Renderer, out GraphicsApi renderer))
                throw new ArgumentException("The shader platform or rendering API is invalid.");
            ToolRunner tools = PrepareTools(cancellation.Token);
            ShaderArtifactBuilder.CompileGraphicsProgram(AssetRoot, ShaderPath, platform, renderer, OutputFile, tools, cancellation.Token);
            return true;
        }
        catch (Exception failure)
        {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
        finally
        {
            lock (m_gate)
                m_cancellation = null;
        }
    }

    /// <summary>
    /// Cancels offline tool preparation and prevents publication of newly prepared tool products.
    /// </summary>
    public void Cancel()
    {
        lock (m_gate)
        {
            m_canceled = true;
            m_cancellation?.Cancel();
        }
    }

    private ToolRunner PrepareTools(CancellationToken cancellationToken)
    {
        NativeBuildContext context = new(EngineRoot, Configuration.ToLowerInvariant());
        string cacheKey = "Inno.Build.ShaderTools|" + context.engineRoot + "|" + context.configuration;
        IBuildEngine4 engine = BuildEngine as IBuildEngine4
            ?? throw new InvalidOperationException("Offline tool preparation requires MSBuild's build-lifetime task ownership.");
        string leasePath = Path.Combine(context.engineRoot, "artifacts", "build-tools", "tasks",
            "native", context.configuration + ".lock");
        using FileLease lease = FileLease.AcquireAsync(leasePath, Timeout.InfiniteTimeSpan, cancellationToken)
            .GetAwaiter().GetResult();
        // Only neutral file names cross task load contexts. MSBuild releases the record when this build ends.
        string[]? files = engine.GetRegisteredTaskObject(cacheKey, RegisteredTaskObjectLifetime.Build) as string[];
        if (files is null)
        {
            NativeBuildProduct product = BgfxToolsBuild.BuildAsync(context, cancellationToken).GetAwaiter().GetResult();
            files = product.files.ToArray();
            engine.RegisterTaskObject(cacheKey, files, RegisteredTaskObjectLifetime.Build, allowEarlyCollection: false);
            Log.LogMessage(MessageImportance.Normal,
                "INNO-SHADER-TOOLS prepared {0}; hashedFiles={1}; hashedBytes={2}; nativeProcesses={3}",
                product.fingerprint, context.statistics.hashedFiles, context.statistics.hashedBytes,
                context.statistics.nativeProcesses);
        }
        string extension = OperatingSystem.IsWindows() ? ".exe" : string.Empty;
        Dictionary<BgfxTool, string> executables = [];
        foreach (BgfxTool tool in Enum.GetValues<BgfxTool>())
        {
            string name = tool.ToString().ToLowerInvariant() + "-" + context.configuration + extension;
            executables.Add(tool, files.Single(path => Path.GetFileName(path) == name));
        }
        return new ToolRunner(executables);
    }
}
