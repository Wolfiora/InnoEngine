using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using BGCS;
using BGCS.Configuration;
using BGCS.Core;
using BGCS.Core.Targets;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

/// <summary>
/// Captures one platform's official BGCS target and managed projection choices.
/// </summary>
public sealed class NativeBindingProfile
{
    private readonly Action<BindingHostBuilder>? m_configureGenerator;

    /// <summary>
    /// Creates a target profile without discovering an SDK or executing a generator.
    /// </summary>
    /// <param name="targetId">
    /// The engine target identity that may consume this profile.
    /// </param>
    /// <param name="target">
    /// The official BGCS native target selection.
    /// </param>
    /// <param name="runtime">
    /// The official BGCS managed runtime selection.
    /// </param>
    /// <param name="inputPaths">
    /// Checkout-relative implementation inputs owned by this profile.
    /// </param>
    /// <param name="configureGenerator">
    /// An immutable platform-owned registration action for official BGCS extensions, or null for the standard SDK.
    /// The action must not capture mutable state; its implementation and extension assemblies participate in input identity.
    /// </param>
    /// <param name="importMode">
    /// The platform's official import mechanism, or null to preserve the component choice.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An identity is empty, uses implicit host selection, or a path escapes the checkout.
    /// </exception>
    public NativeBindingProfile(
        string targetId,
        TargetConfiguration target,
        ManagedRuntimeId? runtime = null,
        IEnumerable<string>? inputPaths = null,
        Action<BindingHostBuilder>? configureGenerator = null,
        BindingImportMode? importMode = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        if (targetId == "host")
            throw new ArgumentException("A binding profile requires an explicit engine target.", nameof(targetId));
        ArgumentNullException.ThrowIfNull(target);
        if (target.id == "host" || string.IsNullOrWhiteSpace(target.id))
            throw new ArgumentException("Binding profiles require an explicit native ABI.", nameof(target));
        this.targetId = targetId;
        this.target = target;
        if (importMode is { } selectedImport && !Enum.IsDefined(selectedImport))
            throw new ArgumentOutOfRangeException(nameof(importMode));
        this.runtime = runtime;
        this.importMode = importMode;
        this.inputPaths = Array.AsReadOnly((inputPaths ?? []).Select(ValidatePath).ToArray());
        m_configureGenerator = configureGenerator;
    }

    /// <summary>
    /// Gets the engine target authorized to consume this profile.
    /// </summary>
    public string targetId { get; }
    /// <summary>
    /// Gets the immutable official BGCS target configuration.
    /// </summary>
    public TargetConfiguration target { get; }
    /// <summary>
    /// Gets the managed projection runtime, or null for the component's portable default.
    /// </summary>
    public ManagedRuntimeId? runtime { get; }
    /// <summary>
    /// Gets the explicit platform import mechanism, or null for the component's mechanism.
    /// </summary>
    public BindingImportMode? importMode { get; }
    /// <summary>
    /// Gets the profile's exact checkout implementation inputs.
    /// </summary>
    public IReadOnlyList<string> inputPaths { get; }

    internal void Configure(BindingHostBuilder builder) => m_configureGenerator?.Invoke(builder);

    internal IEnumerable<System.Reflection.Assembly> GetExtensionAssemblies() =>
        m_configureGenerator?.GetInvocationList().Select(static action => action.Method.Module.Assembly) ?? [];

    internal GenerationRequest Apply(
        GenerationRequest request,
        NativeCompilerInputs? compiler,
        NativeComponentBuildOptions? options,
        string engineRoot
    ) {
        TargetConfiguration selected = target;
        if (compiler is not null)
            selected = selected with
            {
                settings = selected.settings with
                {
                    compilerPath = request.configuration.source.language == BGCS.Core.Inputs.SourceLanguage.Cpp
                        ? compiler.cppCompiler : compiler.cCompiler,
                    sysRoot = compiler.sysRoot,
                    systemIncludeDirectories = compiler.systemIncludeDirectories.ToImmutableArray(),
                    cxxSystemIncludeDirectories = compiler.cppSystemIncludeDirectories.ToImmutableArray(),
                    defines = [.. selected.settings.defines, .. compiler.defines],
                    arguments = [.. selected.settings.arguments, .. compiler.arguments]
                }
            };
        BindingConfiguration config = request.configuration;
        return request.WithConfiguration(config with
        {
            targets = [selected],
            api = config.api with
            {
                runtime = runtime ?? config.api.runtime,
                importMode = importMode ?? config.api.importMode
            },
            library = config.library with { nativeName = options?.bindingLibraryName ?? config.library.nativeName },
            source = config.source with
            {
                includeDirectories = [.. config.source.includeDirectories,
                    .. (options?.bindingIncludeDirectories ?? []).Select(path => Path.GetFullPath(path, engineRoot))]
            }
        });
    }

    private static string ValidatePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || Path.IsPathFullyQualified(path) || path.Contains(':')
            || path.Replace('\\', '/').Split('/').Any(static part => part is "" or "." or ".."))
            throw new ArgumentException("Binding profile inputs must remain within the checkout.", nameof(path));
        return path.Replace('\\', '/');
    }
}
