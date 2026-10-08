using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Inno.Build.Toolchains;
using Inno.Build.Distribution.Standard;
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
    public string Target { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the explicit native target of the offline compiler executed by this task.
    /// This selection is independent of the shader artifact target.
    /// </summary>
    [Required]
    public string ToolTarget { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the graphics API identifier.
    /// </summary>
    [Required]
    public string Renderer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the compiled file inside an owned Outputs subdirectory.
    /// Its parent artifact directory is validated and atomically published with a content manifest.
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
            var execution = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId(ToolTarget));
            BgfxShaderTargetProfile platform = StandardBuildDistribution.Create(execution).ResolveShaderTarget(new BuildTargetId(Target));
            if (!GraphicsApi.TryParse(Renderer, out GraphicsApi renderer))
                throw new ArgumentException("The shader platform or rendering API is invalid.");
            ToolRunner tools = PrepareTools(cancellation.Token, out string compilerFingerprint);
            CompileProduct(tools, compilerFingerprint, platform, renderer, cancellation.Token);
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

    private ToolRunner PrepareTools(
        CancellationToken cancellationToken,
        out string compilerFingerprint
    ) {
        NativeBuildContext context = new(EngineRoot, Configuration.ToLowerInvariant());
        var execution = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId(ToolTarget));
        string cacheKey = "Inno.Build.ShaderTools|" + context.engineRoot + "|" + context.configuration + "|" + ToolTarget;
        IBuildEngine4 engine = BuildEngine as IBuildEngine4
            ?? throw new InvalidOperationException("Offline tool preparation requires MSBuild's build-lifetime task ownership.");
        string leasePath = Path.Combine(context.engineRoot, "artifacts", "build-tools", "tasks",
            "native", ToolTarget + "-" + context.configuration + ".lock");
        using FileLease lease = FileLease.AcquireAsync(leasePath, Timeout.InfiniteTimeSpan, cancellationToken)
            .GetAwaiter().GetResult();
        // Only neutral file names cross task load contexts. MSBuild releases the record when this build ends.
        string[]? record = engine.GetRegisteredTaskObject(cacheKey, RegisteredTaskObjectLifetime.Build) as string[];
        if (record is not null && (record.Length < 3 || !BuildArtifactManifest.IsComplete(record[0], record[1], ["Outputs"])))
            throw new InvalidDataException("The build-owned offline tool product changed after preparation.");
        if (record is null)
        {
            var distribution = StandardBuildDistribution.Create(execution).build;
            context = context.WithToolchain(distribution.ResolveNativeToolchain(ToolTarget)
                .ResolveAsync(context, execution.host, ToolTarget, cancellationToken).AsTask().GetAwaiter().GetResult());
            NativeBuildProduct product = distribution.ResolveNativeProduct(ToolTarget, "shader-tools")
                .BuildAsync(context, cancellationToken).GetAwaiter().GetResult().Single();
            record = [product.directory, product.fingerprint, .. product.files];
            engine.RegisterTaskObject(cacheKey, record, RegisteredTaskObjectLifetime.Build, allowEarlyCollection: false);
            Log.LogMessage(MessageImportance.Normal,
                "INNO-SHADER-TOOLS prepared {0}; hashedFiles={1}; hashedBytes={2}; nativeProcesses={3}",
                product.fingerprint, context.statistics.hashedFiles, context.statistics.hashedBytes,
                context.statistics.nativeProcesses);
        }
        string extension = execution.host.system == "Windows" ? ".exe" : string.Empty;
        Dictionary<BgfxTool, string> executables = [];
        foreach (BgfxTool tool in Enum.GetValues<BgfxTool>())
        {
            string name = tool.ToString().ToLowerInvariant() + "-" + context.configuration + extension;
            executables.Add(tool, record.Skip(2).Single(path => Path.GetFileName(path) == name));
        }
        compilerFingerprint = record[1];
        return new ToolRunner(executables);
    }

    private void CompileProduct(
        ToolRunner tools,
        string compilerFingerprint,
        BgfxShaderTargetProfile platform,
        GraphicsApi renderer,
        CancellationToken cancellationToken
    ) {
        string output = Path.GetFullPath(OutputFile);
        string outputs = Path.GetDirectoryName(output)!;
        if (Path.GetFileName(outputs) != "Outputs")
            throw new ArgumentException("A shader product must be published inside its owned Outputs directory.", nameof(OutputFile));
        string directory = Path.GetDirectoryName(outputs)!;
        string runtime = Path.GetDirectoryName(typeof(CompileShaderTask).Assembly.Location)!;
        NativeBuildInput[] inputs = [new("shader-source", Path.GetFullPath(AssetRoot)),
            .. Directory.EnumerateFiles(runtime, "*.dll").Order(StringComparer.Ordinal)
                .Select(path => new NativeBuildInput("compiler-runtime/" + Path.GetFileName(path), path))];
        string[] declarations = [compilerFingerprint, platform.id, platform.Resolve(renderer).key, Renderer, Configuration, ShaderPath, Path.GetFileName(output)];
        using FileLease lease = FileLease.AcquireAsync(directory + ".lock", Timeout.InfiniteTimeSpan, cancellationToken)
            .GetAwaiter().GetResult();
        string fingerprint = NativeBuildFingerprint.Create(declarations, inputs, cancellationToken);
        if (BuildArtifactManifest.IsComplete(directory, fingerprint, ["Outputs"]))
        {
            Log.LogMessage(MessageImportance.Normal, "INNO-SHADER reused {0}: {1}", Renderer, fingerprint);
            return;
        }
        string candidate = directory + ".candidate-" + Guid.NewGuid().ToString("N");
        string candidateOutputs = Path.Combine(candidate, "Outputs");
        Directory.CreateDirectory(candidateOutputs);
        try
        {
            ShaderArtifactBuilder.CompileGraphicsProgram(AssetRoot, ShaderPath, platform, renderer,
                Path.Combine(candidateOutputs, Path.GetFileName(output)), tools, cancellationToken);
            if (NativeBuildFingerprint.Create(declarations, inputs, cancellationToken) != fingerprint)
                throw new InvalidDataException("Shader inputs changed during compilation; the candidate was not published.");
            BuildArtifactManifest.Write(candidate, fingerprint, ["Outputs"]);
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Install(candidate, directory);
            Log.LogMessage(MessageImportance.Normal, "INNO-SHADER published {0}: {1}", Renderer, fingerprint);
        }
        finally
        {
            if (Directory.Exists(candidate))
                Directory.Delete(candidate, recursive: true);
        }
    }
}
