using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.Build.Toolchains;

/// <summary>
/// Identifies the checkout, configuration and frozen initial inputs owned by one native build operation.
/// Create a new context for each operation; changes during its lifetime fail stability verification.
/// </summary>
public sealed class NativeBuildContext
{
    private readonly NativeBuildInputState m_inputState;
    private INativeBindingGenerator? m_bindingGenerator;
    private NativeInputVerificationScope? m_verification;
    private NativeComponentDescriptor? m_component;
    private NativeComponentBuildOptions? m_componentOptions;
    private readonly string? m_buildOwner;
    private readonly string? m_targetId;
    private readonly string? m_fingerprint;
    private readonly string? m_toolDirectory;
    private IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> m_bindings =
        new ReadOnlyDictionary<string, NativeBindingGenerationDescriptor>(new Dictionary<string, NativeBindingGenerationDescriptor>());
    /// <summary>
    /// Creates a build context without consulting the tool assembly's checkout.
    /// </summary>
    /// <param name="engineRoot">
    /// The checkout containing InnoEngine.sln and the native source tree.
    /// </param>
    /// <param name="configuration">
    /// The normalized debug or release configuration.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The root is empty or the configuration is unsupported.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The selected checkout has no repository marker.
    /// </exception>
    public NativeBuildContext(
        string engineRoot,
        string configuration
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ToolchainEnvironment.ValidateConfiguration(configuration);
        this.engineRoot = Path.GetFullPath(engineRoot);
        if (!File.Exists(Path.Combine(this.engineRoot, ToolchainLayout.C_REPOSITORY_MARKER_FILE)))
            throw new DirectoryNotFoundException($"Engine checkout is unavailable at '{this.engineRoot}'.");
        this.configuration = configuration;
        m_inputState = new NativeBuildInputState();
    }

    private NativeBuildContext(
        NativeBuildContext origin,
        string buildOwner,
        string targetId,
        string fingerprint
    ) : this(origin.engineRoot, origin.configuration) {
        m_inputState = origin.m_inputState;
        sourcePreparation = origin.sourcePreparation;
        m_bindingGenerator = origin.m_bindingGenerator;
        m_verification = origin.m_verification;
        m_component = origin.m_component;
        m_componentOptions = origin.m_componentOptions;
        m_buildOwner = buildOwner;
        m_targetId = targetId;
        m_fingerprint = fingerprint;
        toolchain = origin.toolchain;
        m_bindings = origin.m_bindings;
    }

    private NativeBuildContext(
        NativeBuildContext origin,
        NativeToolchainSelection toolchain
    ) : this(origin.engineRoot, origin.configuration) {
        m_inputState = origin.m_inputState;
        sourcePreparation = origin.sourcePreparation;
        m_bindingGenerator = origin.m_bindingGenerator;
        m_verification = origin.m_verification;
        m_component = origin.m_component;
        m_componentOptions = origin.m_componentOptions;
        this.toolchain = toolchain;
        m_bindings = origin.m_bindings;
        m_buildOwner = origin.m_buildOwner;
        m_targetId = origin.m_targetId;
        m_fingerprint = origin.m_fingerprint;
        m_toolDirectory = origin.m_toolDirectory;
    }

    private NativeBuildContext(
        NativeBuildContext origin,
        NativeComponentDescriptor component,
        NativeComponentBuildOptions options
    ) : this(origin.engineRoot, origin.configuration) {
        m_inputState = origin.m_inputState;
        sourcePreparation = origin.sourcePreparation;
        m_bindingGenerator = origin.m_bindingGenerator;
        m_verification = origin.m_verification;
        m_component = component;
        m_componentOptions = options;
        m_buildOwner = origin.m_buildOwner;
        m_targetId = origin.m_targetId;
        m_fingerprint = origin.m_fingerprint;
        m_toolDirectory = origin.m_toolDirectory;
        toolchain = origin.toolchain;
        m_bindings = origin.m_bindings;
    }

    private NativeBuildContext(
        NativeBuildContext origin,
        string? toolDirectory
    ) : this(origin.engineRoot, origin.configuration) {
        m_inputState = origin.m_inputState;
        sourcePreparation = origin.sourcePreparation;
        m_bindingGenerator = origin.m_bindingGenerator;
        m_verification = origin.m_verification;
        m_component = origin.m_component;
        m_componentOptions = origin.m_componentOptions;
        m_buildOwner = origin.m_buildOwner;
        m_targetId = origin.m_targetId;
        m_fingerprint = origin.m_fingerprint;
        toolchain = origin.toolchain;
        m_toolDirectory = toolDirectory;
        m_bindings = origin.m_bindings;
    }

    /// <summary>
    /// Gets the absolute checkout used for every source, intermediate and output path.
    /// </summary>
    public string engineRoot { get; }

    /// <summary>
    /// Gets the normalized debug or release configuration prepared by this operation.
    /// </summary>
    public string configuration { get; }

    /// <summary>
    /// Gets the explicit frozen target compiler and SDK selection, or null before composition resolves it.
    /// Browser toolchains resolve their own workload-specific SDK independently.
    /// </summary>
    public NativeToolchainSelection? toolchain { get; }

    /// <summary>
    /// Resolves native intermediates under the owning toolchain project in this checkout.
    /// </summary>
    /// <param name="component">
    /// The explicit component descriptor whose recipe project owns native intermediates.
    /// </param>
    /// <returns>
    /// The absolute project-local obj/native path, without creating the directory. During publication,
    /// the path also contains the operation's target and input fingerprint. A Windows producer receives
    /// a temporary junction to the same physical directory so SDK tools can use bounded paths.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The component owner is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The selected checkout does not contain the owning toolchain project.
    /// </exception>
    public string GetNativeBuildRoot(NativeComponentDescriptor component)
    {
        ArgumentNullException.ThrowIfNull(component);
        string projectRoot = component.GetToolchainRoot(engineRoot);
        string projectName = Path.GetFileNameWithoutExtension(component.toolchainProject);
        if (!File.Exists(Path.Combine(projectRoot, projectName + ".csproj")))
            throw new InvalidOperationException($"Native build owner '{projectName}' is unavailable in '{engineRoot}'.");
        string root = Path.Combine(projectRoot, "obj", "native");
        if (m_buildOwner is null)
            return root;
        if (component.toolchainProject != m_buildOwner)
            throw new InvalidOperationException($"The native operation belongs to '{m_buildOwner}', not '{projectName}'.");
        return m_toolDirectory ?? Path.Combine(root, m_targetId!, m_fingerprint!);
    }

    /// <summary>
    /// Gets actual hashing and native execution work accumulated by this operation and its scoped contexts.
    /// </summary>
    public NativeBuildStatistics statistics => m_inputState.statistics;

    internal NativeBuildInputState inputState => m_inputState;

    internal NativeCMakeSource.Preparation? sourcePreparation { get; private set; }

    /// <summary>
    /// Records one complete binding batch executed by the borrowed provider.
    /// </summary>
    public void RecordBindingBatch() => m_inputState.RecordBindingBatch();

    /// <summary>
    /// Records one actual bridge or managed generator invocation.
    /// </summary>
    public void RecordBindingGeneration() => m_inputState.RecordBindingGeneration();

    /// <summary>
    /// Records one full artifact output read without treating it as an input snapshot hash.
    /// </summary>
    /// <param name="bytes">
    /// The actual number of bytes consumed by output integrity verification.
    /// </param>
    public void RecordOutputRead(long bytes) => m_inputState.RecordOutput(bytes);

    /// <summary>
    /// Records a managed SDK process owned by generator extension preparation.
    /// </summary>
    public void RecordManagedProcess() => m_inputState.RecordManagedProcess();

    internal NativeBuildContext WithIdentity(
        NativeComponentDescriptor owner,
        string targetId,
        string fingerprint
    ) => new(this, owner.toolchainProject, targetId, fingerprint);

    /// <summary>
    /// Attaches a provider's immutable tool selection while preserving this operation's input ownership.
    /// </summary>
    /// <param name="toolchain">
    /// The complete selection resolved for the requested target.
    /// </param>
    /// <returns>
    /// A scoped context sharing input snapshots and statistics with this operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The selection is null.
    /// </exception>
    public NativeBuildContext WithToolchain(NativeToolchainSelection toolchain)
    {
        ArgumentNullException.ThrowIfNull(toolchain);
        return new(this, toolchain);
    }

    /// <summary>
    /// Requires an explicit selection before a component can build or start a native tool.
    /// </summary>
    /// <returns>
    /// The already frozen tool selection without performing discovery.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Composition has not resolved a toolchain.
    /// </exception>
    public NativeToolchainSelection RequireToolchain() => toolchain
        ?? throw new InvalidOperationException("Native composition must provide an explicit target toolchain before building.");

    /// <summary>
    /// Scopes one component's explicit product configuration without changing the frozen SDK.
    /// </summary>
    /// <param name="component">
    /// The sole source and intermediate owner receiving the configuration.
    /// </param>
    /// <param name="options">
    /// The immutable linkage, arguments and configuration input closure.
    /// </param>
    /// <returns>
    /// A context sharing inputs, bindings and statistics with the current operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// An owner or configuration is null.
    /// </exception>
    public NativeBuildContext WithComponentOptions(
        NativeComponentDescriptor component,
        NativeComponentBuildOptions options
    ) {
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(options);
        return new(this, component, options);
    }

    /// <summary>
    /// Requires product configuration for the exact component before its recipe or tools execute.
    /// </summary>
    /// <param name="component">
    /// The component requesting its scoped configuration.
    /// </param>
    /// <returns>
    /// The immutable configuration; absent or foreign ownership fails explicitly.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// No options are assigned to this component owner.
    /// </exception>
    public NativeComponentBuildOptions RequireComponentOptions(NativeComponentDescriptor component)
    {
        ArgumentNullException.ThrowIfNull(component);
        return m_component?.nativeProject == component.nativeProject
            && m_component.toolchainProject == component.toolchainProject && m_componentOptions is not null
            ? m_componentOptions : throw new InvalidOperationException("The native component requires explicit product configuration.");
    }

    /// <summary>
    /// Creates a scoped operation with an immutable, explicitly prepared target binding closure.
    /// </summary>
    /// <param name="bindings">
    /// Completed generations indexed by their declared Native project locations.
    /// </param>
    /// <returns>
    /// A context sharing the existing tool selection, inputs and statistics.
    /// </returns>
    public NativeBuildContext WithBindings(IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> bindings)
    {
        ArgumentNullException.ThrowIfNull(bindings);
        var scoped = new NativeBuildContext(this, RequireToolchain());
        scoped.m_bindings = new ReadOnlyDictionary<string, NativeBindingGenerationDescriptor>(
            new Dictionary<string, NativeBindingGenerationDescriptor>(bindings, StringComparer.Ordinal));
        return scoped;
    }

    /// <summary>
    /// Requires the target binding generation prepared for an explicitly declared Native owner.
    /// </summary>
    /// <param name="component">
    /// The component whose facade and managed binding share one generation.
    /// </param>
    /// <returns>
    /// The immutable completed generation descriptor.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The owner has no prepared generation in this operation.
    /// </exception>
    public NativeBindingGenerationDescriptor RequireBindings(NativeComponentDescriptor component) =>
        m_bindings.TryGetValue(component.nativeProject, out NativeBindingGenerationDescriptor? generation)
            ? generation : throw new InvalidOperationException($"No target binding generation was prepared for '{component.nativeProject}'.");

    /// <summary>
    /// Borrows a generator while preserving this operation's input and tool ownership.
    /// </summary>
    /// <param name="generator">
    /// The shared generation implementation selected by composition.
    /// </param>
    /// <returns>
    /// A derived context; neither context owns or disposes the provider.
    /// </returns>
    public NativeBuildContext WithBindingGenerator(INativeBindingGenerator generator)
    {
        ArgumentNullException.ThrowIfNull(generator);
        var scoped = new NativeBuildContext(this, m_toolDirectory);
        scoped.m_bindingGenerator = generator;
        return scoped;
    }

    /// <summary>
    /// Requires generation to be configured explicitly before creating staging or launching tools.
    /// </summary>
    /// <returns>
    /// The borrowed provider without discovering or constructing an implementation.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Composition has not supplied a binding generator.
    /// </exception>
    public INativeBindingGenerator RequireBindingGenerator() => m_bindingGenerator
        ?? throw new InvalidOperationException("Native composition must provide a binding generator.");

    /// <summary>
    /// Starts a fresh verification phase whose reads can be shared only within this derived context.
    /// </summary>
    /// <param name="phase">
    /// The operation phase recorded in statistics, such as binding-locks or binding-generation.
    /// </param>
    /// <returns>
    /// A context with a new input inventory and hash cache; existing scopes are unchanged.
    /// </returns>
    /// <remarks>
    /// Create another phase after each independent ownership wait, generation or compilation.
    /// </remarks>
    public NativeBuildContext BeginInputVerification(string phase)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(phase);
        var scoped = new NativeBuildContext(this, m_toolDirectory);
        scoped.m_verification = new NativeInputVerificationScope(m_inputState, phase);
        return scoped;
    }

    internal NativeInputSnapshot CaptureInputs(
        IEnumerable<NativeBuildInput> inputs,
        System.Threading.CancellationToken cancellationToken
    ) => m_verification?.Capture(inputs, cancellationToken)
        ?? m_inputState.CaptureInitial(inputs, cancellationToken);

    internal NativeBuildContext WithToolDirectory(string toolDirectory) => new(this, toolDirectory);

    internal NativeBuildContext WithExecutionInputs(NativeInputSnapshot inputs)
    {
        var scoped = new NativeBuildContext(this, m_toolDirectory);
        scoped.sourcePreparation = new NativeCMakeSource.Preparation(inputs);
        return scoped;
    }
}
