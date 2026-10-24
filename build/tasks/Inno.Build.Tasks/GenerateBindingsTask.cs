using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Build.Framework;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
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
    /// Gets or sets the stable component identity declared by the sole binding definition.
    /// </summary>
    [Required]
    public string ComponentId { get; set; } = string.Empty;
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
    /// Gets or sets the explicitly selected managed SDK host used to compose the target toolchain.
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
    /// Gets whether the selected generated context requires runtime-import initialization.
    /// </summary>
    [Output]
    public bool UsesRuntimeImports { get; private set; }
    /// <summary>
    /// Gets the complete native artifacts and compilation requirements selected by this operation.
    /// </summary>
    [Output]
    public string NativeDirectory { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the protected immutable artifact root, or an empty value for host-source generation.
    /// </summary>
    [Output]
    public string ArtifactDirectory { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the publisher root coordinating the selected immutable artifact, or empty for host sources.
    /// </summary>
    [Output]
    public string ArtifactOwnerDirectory { get; private set; } = string.Empty;

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
            var owner = new NativeComponentDescriptor(ComponentId, project, project,
                bindingDefinition: definition);
            var context = new NativeBuildContext(root, "release");
            BuildCompositionContext captured = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId(TargetId));
            var execution = new BuildCompositionContext(DotnetHost, captured.applicationDirectory,
                captured.host, captured.toolsTarget);
            BuildDistribution distribution = StandardBuildDistribution.Create(execution).build;
            INativeBindingGenerator generator = distribution.ResolveBindingGenerator(TargetId);
            context = context.WithBindingGenerator(generator);
            NativeToolchainSelection toolchain = distribution.ResolveNativeToolchain(TargetId)
                .ResolveAsync(context, execution.host, TargetId, cancellation).AsTask().GetAwaiter().GetResult();
            context = context.WithToolchain(toolchain);
            Dictionary<string, string>? expected = ExpectedFingerprint.Length == 0 ? null
                : new(StringComparer.Ordinal) { [owner.nativeProject] = ExpectedFingerprint };
            NativeComponentBuildOptions[] configured = distribution.platforms
                .Where(platform => platform.descriptor.id.value == TargetId)
                .SelectMany(static platform => platform.nativeProducts.Values)
                .SelectMany(static product => product.plan.steps)
                .Where(step => step.component.nativeProject == owner.nativeProject)
                .Select(static step => step.options).ToArray();
            NativeComponentBuildOptions? selectedOptions = configured.FirstOrDefault();
            if (selectedOptions is not null && configured.Any(options => options.libraryKind != selectedOptions.libraryKind
                || options.bindingLibraryName != selectedOptions.bindingLibraryName
                || !options.bindingIncludeDirectories.SequenceEqual(selectedOptions.bindingIncludeDirectories, StringComparer.Ordinal)))
                throw new InvalidOperationException("The registered products declare ambiguous standalone component binding inputs.");
            Dictionary<string, NativeComponentBuildOptions>? componentOptions = selectedOptions is null ? null
                : new(StringComparer.Ordinal) { [owner.nativeProject] = selectedOptions };
            var request = new NativeBindingGenerationRequest([owner], TargetId, mode, CheckOnly, expected, componentOptions);
            using NativeBindingGenerationResult closure = generator.GenerateAsync(context, request, cancellation)
                .AsTask().GetAwaiter().GetResult();
            NativeBindingGenerationDescriptor result = closure[owner.nativeProject];
            if (DescriptorOutputPath.Length != 0)
                result.Write(DescriptorOutputPath);
            GeneratedBindings = result.bindingsPath;
            GenerationFingerprint = result.fingerprint;
            UsesRuntimeImports = result.usesRuntimeImports;
            NativeDirectory = result.nativeDirectory;
            ArtifactDirectory = result.artifactDirectory;
            ArtifactOwnerDirectory = result.artifactOwnerDirectory;
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
