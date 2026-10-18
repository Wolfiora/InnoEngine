using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Inno.Build.Toolchains;
using BGCS.Configuration;
using BGCS.Core.Caching;
using BGCS.Core.Extensibility;
using BGCS.Core.IO;
using BGCS.Cpp2C.Configuration;
using BGCS.Cpp2C.Facade;
using BGCS.CppAst.Parsing;
using BGCS.CppAst.Targeting;
using BGCS.Facade;
using Newtonsoft.Json;

namespace Inno.Build.Bindings;

internal static class NativeBindingGenerationIdentity
{
    internal static string Compute(
        CsCodeGeneratorConfig managed,
        Cpp2CGeneratorConfig? bridge,
        BindingGenerationConfiguration definition,
        string outputRoot,
        NativeBuildContext context,
        string? hostBridgeRoot,
        CancellationToken cancellationToken,
        IReadOnlyList<string>? excludedDirectories = null
    ) {
        string configPath = definition.configPath;
        string bridgeConfigPath = definition.bridgePath ?? string.Empty;
        string managedBase = Path.GetDirectoryName(Path.GetFullPath(configPath))!;
        string bridgeBase = bridge is null ? managedBase : Path.GetDirectoryName(Path.GetFullPath(bridgeConfigPath))!;
        string? bridgeOutput = bridge is null ? null : Resolve(bridge.outputPath, bridgeBase!);
        IEnumerable<string> entries = managed.entryFiles.Select(path => Resolve(path, managedBase))
            .Where(path => (bridgeOutput is null || !IsWithin(path, bridgeOutput))
                && (hostBridgeRoot is null || !IsWithin(path, hostBridgeRoot)));
        IEnumerable<string> includes = managed.includeFolders.Concat(managed.systemIncludeFolders)
            .Select(path => Resolve(path, managedBase))
            .Concat(managed.resolvedTarget.toolchain.systemIncludeFolders)
            .Concat(managed.resolvedTarget.toolchain.cxxSystemIncludeFolders);
        string lowering = string.Empty;
        if (bridge is not null)
        {
            entries = entries.Concat(bridge.entryFiles.Select(path => Resolve(path, bridgeBase!)));
            includes = includes.Concat(bridge.includeFolders.Concat(bridge.systemIncludeFolders)
                .Select(path => Resolve(path, bridgeBase!)))
                .Concat(bridge.resolvedTarget.toolchain.systemIncludeFolders)
                .Concat(bridge.resolvedTarget.toolchain.cxxSystemIncludeFolders);
            object[] lowerings = [..bridge.lowerings.typeLowerings, ..bridge.lowerings.callableLowerings,
                ..bridge.lowerings.artifactContributors];
            if (lowerings.Any(static service => service is not ICacheFingerprintProvider))
                throw new InvalidOperationException("Target binding generation requires fingerprinted lowering services.");
            lowering = string.Join("\n", lowerings.Cast<ICacheFingerprintProvider>()
                .Select(static service => service.GetCacheFingerprint()).Order(StringComparer.Ordinal));
        }
        string[] excluded = (bridgeOutput is null ? new[] { outputRoot } : [outputRoot, bridgeOutput])
            .Concat(hostBridgeRoot is null ? [] : new[] { hostBridgeRoot }).Concat(excludedDirectories ?? []).ToArray();
        cancellationToken.ThrowIfCancellationRequested();
        IReadOnlyList<string> inputs = IncrementalGenerationCache.DiscoverInputs(entries, includes, excluded);
        string? managedCompiler = CppToolchainDiscovery.FindCompiler(managed.parserKind,
            ConfigurationPath.Resolve(managed.compilerPath ?? managed.resolvedTarget.toolchain.compilerPath,
                managedBase, allowCommandName: true));
        string? bridgeCompiler = bridge is null ? null : CppToolchainDiscovery.FindCompiler(CppParserKind.Cpp,
            ConfigurationPath.Resolve(bridge.compilerPath ?? bridge.resolvedTarget.toolchain.compilerPath,
                bridgeBase, allowCommandName: true));
        string implementation = typeof(NativeBindingGenerationIdentity).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(static metadata => metadata.Key == "Inno.BindingImplementation").Value
            ?? throw new InvalidOperationException("The binding task has no compiled implementation identity.");
        string fingerprint = string.Join("\n",
            implementation,
            typeof(CsCodeGenerator).Assembly.ManifestModule.ModuleVersionId,
            typeof(Cpp2CCodeGenerator).Assembly.ManifestModule.ModuleVersionId,
            typeof(CppParserOptions).Assembly.ManifestModule.ModuleVersionId,
            typeof(IncrementalGenerationCache).Assembly.ManifestModule.ModuleVersionId,
            typeof(BGCS.Intermediate.BindingModule).Assembly.ManifestModule.ModuleVersionId,
            typeof(BGCS.Language.Lexing.Lexer).Assembly.ManifestModule.ModuleVersionId,
            managed.Serialize(),
            JsonConvert.SerializeObject(managed.resolvedTarget),
            CppToolchainDiscovery.GetCompilerFingerprint(managed.parserKind,
                ConfigurationPath.Resolve(managed.compilerPath ?? managed.resolvedTarget.toolchain.compilerPath,
                    managedBase, allowCommandName: true)),
            managed.plugins.GetCacheFingerprint(),
            bridge is null ? "" : bridge.Serialize(),
            bridge is null ? "" : JsonConvert.SerializeObject(bridge.resolvedTarget),
            bridge is null ? "" : CppToolchainDiscovery.GetCompilerFingerprint(CppParserKind.Cpp,
                ConfigurationPath.Resolve(bridge.compilerPath ?? bridge.resolvedTarget.toolchain.compilerPath,
                    bridgeBase!, allowCommandName: true)),
            bridge?.plugins.GetCacheFingerprint() ?? "",
            lowering);
        return NativeBuildFingerprint.Create(
            context,
            [fingerprint, definition.component.nativeProject, definition.request.targetId,
                definition.request.outputMode.ToString(), Path.GetRelativePath(context.engineRoot, definition.definition),
                definition.extensionProject ?? string.Empty, hostBridgeRoot ?? string.Empty],
            inputs.Concat(new[] { configPath, bridgeConfigPath, managedCompiler, bridgeCompiler }
                .OfType<string>().Where(static path => path.Length != 0))
                .Select(path => NativeBuildInput.FromPath(context.engineRoot, path)),
            cancellationToken);
    }

    internal static string Resolve(
        string path,
        string baseDirectory
    ) => ConfigurationPath.Resolve(path, baseDirectory, allowCommandName: false)!;

    internal static bool IsWithin(
        string path,
        string directory
    ) => Path.GetFullPath(path).StartsWith(
        Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory)) + Path.DirectorySeparatorChar,
        OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
}
