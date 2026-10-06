using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BGCS.Configuration;
using BGCS.Core.IO;
using BGCS.Cpp2C.Configuration;
using BGCS.Cpp2C.Facade;
using BGCS.Facade;
using BGCS.Intermediate;
using BGCS.Intermediate.Bridges;
using Inno.Build.Toolchains;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.Tasks;

/// <summary>
/// Runs the BGCS library pipeline inside MSBuild without invoking a second generator process.
/// </summary>
public sealed class GenerateBindingsTask : BuildTask, ICancelableTask {
    private readonly object m_lifecycle = new();
    private readonly CancellationTokenSource m_cancellation = new();
    private bool m_completed;

    /// <summary>
    /// Gets or sets the checkout root used to give source inputs stable logical identities.
    /// </summary>
    [Required]
    public string EngineRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the composed managed binding definition.
    /// </summary>
    [Required]
    public string ConfigPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the managed output root containing the single binding source.
    /// </summary>
    [Required]
    public string OutputDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional C++ facade bridge definition evaluated before managed generation.
    /// </summary>
    public string BridgeConfigPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether generation is compared against current sources without replacing them.
    /// </summary>
    public bool CheckOnly { get; set; }

    /// <summary>
    /// Gets or sets the optional target output root whose children are immutable generation fingerprints.
    /// An empty value selects the checked-in host output layout.
    /// </summary>
    public string TargetOutputRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional request-owned descriptor destination for command-line toolchain consumers.
    /// </summary>
    public string DescriptorOutputPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an input identity frozen by the native build; a different current identity fails before generation.
    /// </summary>
    public string ExpectedFingerprint { get; set; } = string.Empty;

    /// <summary>
    /// Gets the generated managed source after a successful generation or comparison.
    /// </summary>
    [Output]
    public string GeneratedBindings { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the immutable input identity of the validated generation.
    /// </summary>
    [Output]
    public string GenerationFingerprint { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the complete C bridge root when the component has a facade bridge, otherwise an empty string.
    /// </summary>
    [Output]
    public string NativeBridgeDirectory { get; private set; } = string.Empty;

    /// <summary>
    /// Requests cancellation and prevents publication after the active generator has drained.
    /// </summary>
    public void Cancel() {
        lock (m_lifecycle) {
            if (!m_completed)
                m_cancellation.Cancel();
        }
    }

    /// <summary>
    /// Executes facade lowering and managed emission, or reports their structured diagnostics.
    /// </summary>
    /// <returns>
    /// True when generation succeeds and the output contract is satisfied; false after a reported failure.
    /// </returns>
    public override bool Execute() {
        try {
            BuildTaskHostRetirement.Inspect(EngineRoot);
            CancellationToken cancellation = m_cancellation.Token;
            cancellation.ThrowIfCancellationRequested();
            CsCodeGeneratorConfig config = new ConfigLoader().Load(Path.GetFullPath(ConfigPath));
            Cpp2CGeneratorConfig? bridgeConfig = string.IsNullOrWhiteSpace(BridgeConfigPath)
                ? null : Cpp2CGeneratorConfig.Load(Path.GetFullPath(BridgeConfigPath));
            if (!string.IsNullOrWhiteSpace(TargetOutputRoot)) {
                bool generated = GenerateTarget(config, bridgeConfig, cancellation);
                if (generated)
                    WriteDescriptor();
                return generated;
            }

            bool hostGenerated = GenerateHost(config, bridgeConfig, cancellation);
            if (hostGenerated)
                WriteDescriptor();
            return hostGenerated;
        }
        catch (Exception failure) {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
        finally {
            lock (m_lifecycle) {
                m_completed = true;
                m_cancellation.Dispose();
            }
        }
    }

    private bool GenerateHost(
        CsCodeGeneratorConfig config,
        Cpp2CGeneratorConfig? bridgeConfig,
        CancellationToken cancellation
    ) {
        string managedOutput = Path.GetFullPath(OutputDirectory);
        string? nativeOutput = bridgeConfig is null ? null
            : NativeBindingGenerationIdentity.Resolve(bridgeConfig.outputPath, bridgeConfig.configDirectory!);
        using OutputDirectorySetTransaction publication = new(nativeOutput is null
            ? [managedOutput] : [managedOutput, nativeOutput], cancellationToken: cancellation);
        string[] excluded = publication.stagingPaths.Values.ToArray();
        GenerationFingerprint = NativeBindingGenerationIdentity.Compute(
            config, bridgeConfig, ConfigPath, BridgeConfigPath, managedOutput, EngineRoot, cancellation, excluded);
        if (ExpectedFingerprint.Length > 0 && ExpectedFingerprint != GenerationFingerprint)
            throw new InvalidOperationException("Binding inputs changed after the native build selected its generation.");
        config.enableIncrementalCache = false;
        if (bridgeConfig is not null) {
            bridgeConfig.enableIncrementalCache = false;
            string nativeCandidate = publication.GetStagingPath(nativeOutput!);
            if (!GenerateBridge(bridgeConfig, nativeCandidate))
                return false;
            RewriteBridgeInputs(config, nativeOutput!, nativeCandidate, Path.GetDirectoryName(Path.GetFullPath(ConfigPath))!);
        }
        cancellation.ThrowIfCancellationRequested();
        string managedCandidate = publication.GetStagingPath(managedOutput);
        CsCodeGenerator generator = new(config);
        generator.GenerateConfigured(managedCandidate);
        if (!ReportResult(generator.lastResult))
            return false;
        ValidateSingleSource(managedCandidate);
        cancellation.ThrowIfCancellationRequested();
        if (CheckOnly) {
            if (!CompareOutput(managedCandidate, managedOutput)
                || nativeOutput is not null && !CompareFiles(publication.GetStagingPath(nativeOutput), nativeOutput))
                return false;
        }
        else {
            ValidateCurrentInputs(managedOutput, excluded);
            cancellation.ThrowIfCancellationRequested();
            publication.Commit();
        }
        GeneratedBindings = Path.Combine(managedOutput, "Bindings.cs");
        NativeBridgeDirectory = nativeOutput ?? string.Empty;
        return true;
    }

    private void ValidateCurrentInputs(
        string outputRoot,
        IReadOnlyList<string>? excludedDirectories = null
    ) {
        CsCodeGeneratorConfig managed = new ConfigLoader().Load(Path.GetFullPath(ConfigPath));
        Cpp2CGeneratorConfig? bridge = string.IsNullOrWhiteSpace(BridgeConfigPath) ? null
            : Cpp2CGeneratorConfig.Load(Path.GetFullPath(BridgeConfigPath));
        string current = NativeBindingGenerationIdentity.Compute(
            managed, bridge, ConfigPath, BridgeConfigPath, outputRoot, EngineRoot, m_cancellation.Token, excludedDirectories);
        if (current != GenerationFingerprint)
            throw new InvalidOperationException("Binding source or toolchain inputs changed during generation; the candidate was not published.");
    }

    private void WriteDescriptor() {
        if (!string.IsNullOrWhiteSpace(DescriptorOutputPath)) {
            new NativeBindingGenerationDescriptor {
                fingerprint = GenerationFingerprint,
                bindingsPath = GeneratedBindings,
                bridgeDirectory = NativeBridgeDirectory
            }.Write(DescriptorOutputPath);
        }
    }

    private bool GenerateTarget(
        CsCodeGeneratorConfig config,
        Cpp2CGeneratorConfig? bridgeConfig,
        CancellationToken cancellation
    ) {
        string root = Path.GetFullPath(TargetOutputRoot);
        GenerationFingerprint = NativeBindingGenerationIdentity.Compute(
            config, bridgeConfig, ConfigPath, BridgeConfigPath, root, EngineRoot, cancellation);
        if (ExpectedFingerprint.Length > 0 && ExpectedFingerprint != GenerationFingerprint)
            throw new InvalidOperationException("Binding inputs changed after the native build selected its generation.");
        cancellation.ThrowIfCancellationRequested();
        string destination = Path.Combine(root, GenerationFingerprint);
        using var publication = new OutputDirectoryTransaction(destination, cancellationToken: cancellation);
        string bindings = Path.Combine(destination, "Generated", "Bindings.cs");
        if (!CheckOnly && File.Exists(bindings)
            && BuildArtifactManifest.IsComplete(destination, GenerationFingerprint, ["Native", "Generated"])) {
            cancellation.ThrowIfCancellationRequested();
            SetTargetOutputs(destination, bridgeConfig is not null);
            return true;
        }
        if (bridgeConfig is not null) {
            // The coherent bundle owns caching for both outputs; candidate paths are request-local.
            bridgeConfig.enableIncrementalCache = false;
            string native = Path.Combine(publication.stagingPath, "Native");
            string declaredRoot = NativeBindingGenerationIdentity.Resolve(
                bridgeConfig.outputPath, bridgeConfig.configDirectory!);
            if (!GenerateBridge(bridgeConfig, native))
                return false;
            RewriteBridgeInputs(config, declaredRoot, native, Path.GetDirectoryName(Path.GetFullPath(ConfigPath))!);
        }
        cancellation.ThrowIfCancellationRequested();
        // The bundle owns caching; staging paths must not create nonce-keyed managed cache entries.
        config.enableIncrementalCache = false;
        string managed = Path.Combine(publication.stagingPath, "Generated");
        CsCodeGenerator generator = new(config);
        generator.GenerateConfigured(managed);
        if (!ReportResult(generator.lastResult))
            return false;
        ValidateSingleSource(managed);
        cancellation.ThrowIfCancellationRequested();
        if (CheckOnly) {
            bool sameNative = bridgeConfig is null || CompareFiles(
                Path.Combine(publication.stagingPath, "Native"), Path.Combine(destination, "Native"));
            if (!sameNative || !CompareOutput(managed, Path.Combine(destination, "Generated")))
                return false;
        }
        else {
            ValidateCurrentInputs(root);
            BuildArtifactManifest.Write(publication.stagingPath, GenerationFingerprint, ["Native", "Generated"]);
            cancellation.ThrowIfCancellationRequested();
            publication.Commit();
        }
        SetTargetOutputs(destination, bridgeConfig is not null);
        return true;
    }

    private bool GenerateBridge(
        Cpp2CGeneratorConfig config,
        string? output
    ) {
        Cpp2CCodeGenerator generator = new(config);
        generator.GenerateConfigured(output);
        return ReportResult(generator.lastResult);
    }

    private void SetTargetOutputs(
        string destination,
        bool hasBridge
    ) {
        GeneratedBindings = Path.Combine(destination, "Generated", "Bindings.cs");
        NativeBridgeDirectory = hasBridge ? Path.Combine(destination, "Native") : string.Empty;
    }

    private static void ValidateSingleSource(string directory) {
        if (!File.Exists(Path.Combine(directory, "Bindings.cs"))
            || Directory.EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories).Count() != 1)
            throw new InvalidDataException("The native component must generate exactly one managed Bindings.cs file.");
    }

    private static void RewriteBridgeInputs(
        CsCodeGeneratorConfig config,
        string declaredRoot,
        string candidateRoot,
        string configDirectory
    ) {
        config.entryFiles = config.entryFiles.Select(Replace).ToList();
        config.allowedHeaders = config.allowedHeaders.Select(Replace).ToList();
        config.includeFolders = config.includeFolders.Select(Replace).ToList();

        string Replace(string path) {
            string full = NativeBindingGenerationIdentity.Resolve(path, configDirectory);
            return NativeBindingGenerationIdentity.IsWithin(full, declaredRoot)
                || string.Equals(full, Path.TrimEndingDirectorySeparator(declaredRoot),
                    OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
                ? Path.Combine(candidateRoot, Path.GetRelativePath(declaredRoot, full))
                : path;
        }
    }

    private bool CompareFiles(
        string expectedDirectory,
        string currentDirectory
    ) {
        if (!Directory.Exists(currentDirectory)) {
            Log.LogError($"Generated native bridge is missing: {currentDirectory}");
            return false;
        }
        string[] expected = Directory.GetFiles(expectedDirectory, "*", SearchOption.AllDirectories);
        string[] current = Directory.GetFiles(currentDirectory, "*", SearchOption.AllDirectories);
        if (expected.Length != current.Length) {
            Log.LogError($"Generated native bridge is out of date: {currentDirectory}");
            return false;
        }
        foreach (string path in expected) {
            string other = Path.Combine(currentDirectory, Path.GetRelativePath(expectedDirectory, path));
            if (!File.Exists(other) || !File.ReadAllBytes(path).AsSpan().SequenceEqual(File.ReadAllBytes(other))) {
                Log.LogError($"Generated native bridge is out of date: {other}");
                return false;
            }
        }
        return true;
    }

    private bool ReportResult<TModule>(BindingGenerationResult<TModule>? result) where TModule : class {
        if (result is null) {
            Log.LogError("The binding generator returned no generation result.");
            return false;
        }
        foreach (BindingDiagnostic diagnostic in result.diagnostics) {
            string message = $"{diagnostic.code}: {diagnostic.message}";
            if (diagnostic.severity == BindingDiagnosticSeverity.Error)
                Log.LogError(message);
            else if (diagnostic.severity == BindingDiagnosticSeverity.Warning)
                Log.LogWarning(message);
        }
        return result.success;
    }

    private bool CompareOutput(
        string expectedDirectory,
        string currentDirectory
    ) {
        Dictionary<string, string> expected = ReadSources(expectedDirectory);
        Dictionary<string, string> current = Directory.Exists(currentDirectory) ? ReadSources(currentDirectory) : [];
        foreach (string relativePath in expected.Keys.Union(current.Keys, StringComparer.Ordinal).Order(StringComparer.Ordinal)) {
            if (!expected.TryGetValue(relativePath, out string? expectedSource) ||
                !current.TryGetValue(relativePath, out string? currentSource) || expectedSource != currentSource) {
                Log.LogError($"Generated binding source is out of date: {Path.Combine(currentDirectory, relativePath)}");
                return false;
            }
        }
        return true;
    }

    private static Dictionary<string, string> ReadSources(string directory) => Directory.EnumerateFiles(
        directory, "*.cs", SearchOption.AllDirectories).ToDictionary(
        path => Path.GetRelativePath(directory, path),
        static path => File.ReadAllText(path).Replace("\r\n", "\n", StringComparison.Ordinal),
        StringComparer.Ordinal);
}
