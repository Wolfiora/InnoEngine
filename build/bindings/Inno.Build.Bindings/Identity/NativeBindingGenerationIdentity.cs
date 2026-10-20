using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using BGCS;
using BGCS.Contracts.Inputs;
using BGCS.Contracts.Targets;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class NativeBindingGenerationIdentity
{
    private static readonly JsonSerializerOptions S_OPTIONS = new()
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    internal static string Compute(
        GenerationRequest request,
        BindingGenerationConfiguration definition,
        NativeBuildContext context,
        CancellationToken cancellation
    ) {
        var builder = BindingHostFactory.CreateBuilder(cancellation);
        foreach (string plugin in request.configuration.extensions.plugins)
            builder.AddPluginManifest(plugin);
        BindingHost host = builder.Build();
        try
        {
            var config = request.configuration;
            var target = config.targets.Single();
            var settings = target.settings;
            var source = config.source;
            AnalysisResult analysis = host.AnalyzeAsync(new AnalysisRequest(source.headers, source.language, request.baseDirectory)
            {
                targetId = target.id,
                triple = target.triple,
                toolchain = new NativeToolchainDescriptor(compilerPath: settings.compilerPath, sysRoot: settings.sysRoot,
                    systemIncludeFolders: settings.systemIncludeDirectories, cxxSystemIncludeFolders: settings.cxxSystemIncludeDirectories,
                    defines: settings.defines, arguments: settings.arguments),
                languageStandard = source.languageStandard!,
                includeDirectories = source.includeDirectories,
                systemIncludeDirectories = source.systemIncludeDirectories,
                defines = source.defines,
                arguments = source.arguments,
                preamble = source.preamble,
                postamble = source.postamble,
                parseMacros = source.parseMacros,
                parseComments = source.parseComments,
                parseSystemIncludes = source.includeSystemDeclarations
            }, cancellation).GetAwaiter().GetResult();
            if (!analysis.success || analysis.snapshot is null)
                throw new InvalidDataException("BGCS input analysis failed: " + analysis.GetDiagnosticSummary());
            var declarations = new List<string>
            {
                JsonSerializer.Serialize(config, S_OPTIONS),
                JsonSerializer.Serialize(analysis.target, S_OPTIONS),
                JsonSerializer.Serialize(analysis.snapshot, S_OPTIONS),
                JsonSerializer.Serialize(analysis.argumentInputs, S_OPTIONS),
                definition.component.nativeProject,
                definition.request.targetId,
                definition.request.outputMode.ToString()
            };
            var inputs = request.configurationInputs.Concat(request.pluginInputs).Concat(analysis.snapshot.files)
                .Concat(analysis.argumentInputs?.files ?? []).Select(input => NativeBuildInput.FromPath(context.engineRoot, input.path)).ToList();
            Assembly implementation = typeof(NativeBindingGenerator).Assembly;
            foreach (Assembly assembly in new[] { implementation, typeof(NativeBindingCompilation).Assembly,
                         typeof(BindingHost).Assembly, typeof(BGCS.Contracts.BindingDiagnostic).Assembly,
                         typeof(BGCS.Runtime.NativeModuleSession).Assembly }.Distinct())
                inputs.Add(new NativeBuildInput("binding-implementation/" + Path.GetFileName(assembly.Location), assembly.Location));
            string resources = Path.Combine(Path.GetDirectoryName(typeof(BindingHost).Assembly.Location)!, "bgcs", "worker-assets");
            if (!Directory.Exists(resources))
                throw new DirectoryNotFoundException("The restored BGCS SDK worker content is missing: " + resources);
            inputs.Add(new NativeBuildInput("binding-worker-assets", resources));
            return NativeBuildFingerprint.Create(context, declarations, inputs, cancellation);
        }
        finally
        {
            host.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
    }
}
