using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.Tasks;

/// <summary>
/// Maps an explicit MSBuild request to the shared binding application without owning generation policy.
/// </summary>
public sealed class GenerateBindingsTask : BuildTask, ICancelableTask
{
    private readonly object m_lifecycle = new();
    private readonly CancellationTokenSource m_cancellation = new();
    private bool m_completed;

    /// <summary>
    /// Gets or sets the absolute checkout owning the component definition.
    /// </summary>
    [Required]
    public string EngineRoot { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the absolute Native owner project, which must belong to this checkout.
    /// </summary>
    [Required]
    public string ComponentProject { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the sole literal definition consumed by both the project and direct generation.
    /// </summary>
    [Required]
    public string BindingDefinition { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the explicit configuration identity selected from the definition.
    /// </summary>
    [Required]
    public string TargetId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets HostSource or TargetArtifacts without deriving publication from a path.
    /// </summary>
    [Required]
    public string OutputMode { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the explicitly selected SDK host for declared generator extensions.
    /// </summary>
    [Required]
    public string DotnetHost { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets comparison without publication.
    /// </summary>
    public bool CheckOnly { get; set; }
    /// <summary>
    /// Gets or sets an optional request-owned descriptor destination written only after success.
    /// </summary>
    public string DescriptorOutputPath { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets an optional expected identity; stale requests fail before generation.
    /// </summary>
    public string ExpectedFingerprint { get; set; } = string.Empty;
    /// <summary>
    /// Gets the single source selected after complete publication or successful comparison.
    /// </summary>
    [Output]
    public string GeneratedBindings { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the complete validated input identity.
    /// </summary>
    [Output]
    public string GenerationFingerprint { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the complete C bridge directory, or an empty value for a direct C API.
    /// </summary>
    [Output]
    public string NativeBridgeDirectory { get; private set; } = string.Empty;

    /// <summary>
    /// Cancels owned work before retiring the task cancellation source.
    /// </summary>
    public void Cancel()
    {
        lock (m_lifecycle)
            if (!m_completed)
                m_cancellation.Cancel();
    }

    /// <summary>
    /// Executes one explicit component request and publishes task outputs only after success.
    /// </summary>
    /// <returns>
    /// True after generation or comparison; false after a logged failure and complete cleanup.
    /// </returns>
    public override bool Execute()
    {
        try
        {
            CancellationToken cancellation = m_cancellation.Token;
            cancellation.ThrowIfCancellationRequested();
            if (!Enum.TryParse(OutputMode, out NativeBindingOutputMode mode) || !Enum.IsDefined(mode))
                throw new ArgumentException("Binding output mode must be explicitly declared.", nameof(OutputMode));
            string root = Path.GetFullPath(EngineRoot);
            string project = Path.GetRelativePath(root, Path.GetFullPath(ComponentProject)).Replace('\\', '/');
            string definition = Path.GetRelativePath(root, Path.GetFullPath(BindingDefinition)).Replace('\\', '/');
            var owner = new NativeComponentDescriptor(Path.GetFileNameWithoutExtension(project), project, project,
                bindingDefinition: definition);
            var context = new NativeBuildContext(root, "release");
            INativeBindingGenerator generator = new NativeBindingGenerator(DotnetHost);
            context = context.WithBindingGenerator(generator);
            if (mode == NativeBindingOutputMode.TargetArtifacts)
            {
                BuildCompositionContext captured = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId(TargetId));
                var execution = new BuildCompositionContext(DotnetHost, captured.applicationDirectory,
                    captured.host, captured.toolsTarget, generator);
                BuildDistribution distribution = StandardBuildDistribution.Create(execution).build;
                if (distribution.TryResolveNativeToolchain(TargetId, out INativeToolchainProvider? provider))
                {
                    NativeToolchainSelection toolchain = provider.ResolveAsync(context, execution.host, TargetId, cancellation)
                        .AsTask().GetAwaiter().GetResult();
                    context = context.WithToolchain(toolchain);
                }
            }
            Dictionary<string, string>? expected = ExpectedFingerprint.Length == 0 ? null
                : new(StringComparer.Ordinal) { [owner.nativeProject] = ExpectedFingerprint };
            var request = new NativeBindingGenerationRequest([owner], TargetId, mode, CheckOnly, expected);
            NativeBindingGenerationDescriptor result = generator.GenerateAsync(context, request, cancellation)
                .AsTask().GetAwaiter().GetResult()[owner.nativeProject];
            if (DescriptorOutputPath.Length != 0)
                result.Write(DescriptorOutputPath);
            GeneratedBindings = result.bindingsPath;
            GenerationFingerprint = result.fingerprint;
            NativeBridgeDirectory = result.bridgeDirectory;
            Log.LogMessage(MessageImportance.High,
                "INNO-BINDINGS-PREPARE project={0} fingerprint={1} batches={2} generations={3}",
                project, result.fingerprint, context.statistics.bindingBatches, context.statistics.bindingGenerations);
            return true;
        }
        catch (Exception failure)
        {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
        finally
        {
            lock (m_lifecycle)
            {
                m_completed = true;
                m_cancellation.Dispose();
            }
        }
    }
}
