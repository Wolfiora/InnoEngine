using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Newtonsoft.Json.Linq;

namespace Inno.Build.Bindings;

internal static class BindingInputPlanning
{
    internal static async ValueTask<string> ComputeAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        string dotnetHost,
        CancellationToken cancellation
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        cancellation.ThrowIfCancellationRequested();
        var declarations = new List<string> { "binding-source-inputs", request.targetId, request.outputMode.ToString() };
        var inputs = new List<NativeBuildInput>();
        var extensions = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (NativeComponentDescriptor component in request.components)
        {
            BindingGenerationConfiguration definition = BindingDefinitionReader.Read(context, component, request);
            var loaded = definition.Load(context);
            declarations.Add(NativeBindingGenerationIdentity.Compute(loaded.managed, loaded.bridge, definition,
                definition.GetOutputRoot(context), context, loaded.hostBridgeRoot, cancellation));
            // Definition identity describes only the selected mapping, never another target's config contents.
            declarations.Add(component.nativeProject + "|" + definition.configPath + "|" + definition.bridgePath
                + "|" + definition.hostBridgePath + "|" + definition.extensionProject);
            foreach (string config in new[] { definition.configPath, definition.bridgePath, definition.hostBridgePath }.OfType<string>())
                inputs.AddRange(ConfigurationInputs(config).Select(path => NativeBuildInput.FromPath(context.engineRoot, path)));
            if (definition.extensionProject is string extension)
            {
                if (!extensions.TryGetValue(extension, out string? identity))
                {
                    DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnetHost, extension, cancellation).ConfigureAwait(false);
                    ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(context, sdk, extension,
                        new Dictionary<string, string> { ["Configuration"] = "Release" }, cancellation).ConfigureAwait(false);
                    identity = snapshot.fingerprint;
                    extensions.Add(extension, identity);
                }
                declarations.Add("extension=" + extension + ":" + identity);
            }
            IEnumerable<string> plugins = loaded.managed.pluginAssemblies.Select(path =>
                    NativeBindingGenerationIdentity.Resolve(path, Path.GetDirectoryName(definition.configPath)!))
                .Concat(loaded.bridge is null ? [] : loaded.bridge.pluginAssemblies.Select(path =>
                    NativeBindingGenerationIdentity.Resolve(path, Path.GetDirectoryName(definition.bridgePath!)!)));
            inputs.AddRange(plugins.Select(path => NativeBuildInput.FromPath(context.engineRoot, path)));
        }
        Assembly[] generators = [typeof(NativeBindingGenerator).Assembly, typeof(BGCS.Facade.CsCodeGenerator).Assembly,
            typeof(BGCS.Cpp2C.Facade.Cpp2CCodeGenerator).Assembly, typeof(BGCS.CppAst.Parsing.CppParserOptions).Assembly,
            typeof(BGCS.Core.Caching.IncrementalGenerationCache).Assembly, typeof(BGCS.Intermediate.BindingModule).Assembly,
            typeof(BGCS.Language.Lexing.Lexer).Assembly, typeof(JObject).Assembly];
        inputs.AddRange(generators.Distinct().Select(assembly => new NativeBuildInput(
            "binding-implementation/" + Path.GetFileName(assembly.Location), assembly.Location)));
        return NativeBuildFingerprint.Create(context, declarations, inputs, cancellation);
    }

    private static IEnumerable<string> ConfigurationInputs(string path)
    {
        var visited = new HashSet<string>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        while (true)
        {
            path = Path.GetFullPath(path);
            if (!visited.Add(path))
                throw new InvalidDataException("Binding configuration input inheritance contains a cycle.");
            yield return path;
            JObject document = JObject.Parse(File.ReadAllText(path));
            string? reference = document["baseConfig"] is JObject parent ? parent.Value<string>("url") : null;
            if (string.IsNullOrWhiteSpace(reference))
                yield break;
            if (!reference.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException("Build input planning requires locally owned binding configuration inputs.");
            path = Path.GetFullPath(reference["file://".Length..], Path.GetDirectoryName(path)!);
        }
    }
}
