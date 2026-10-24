using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using BGCS;
using BGCS.Core.Hosting;
using BGCS.Core.Inputs;
using BGCS.Core.Targets;
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
        using HostServices services = BindingHostFactory.CreateHostServices(context.engineRoot);
        var builder = BindingHostFactory.CreateBuilder(definition.profile, cancellation);
        foreach (string plugin in request.configuration.extensions.plugins)
            builder.AddPluginManifest(plugin, services.paths);
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
                SerializeIdentity(config),
                SerializeIdentity(analysis.target),
                SerializeIdentity(analysis.snapshot),
                SerializeIdentity(analysis.argumentInputs),
                SerializeIdentity(request.configurationSnapshot),
                SerializeIdentity(request.pluginSnapshots),
                definition.component.nativeProject,
                definition.request.targetId,
                definition.request.outputMode.ToString()
            };
            var profileInputs = definition.profile.inputPaths.Select(path => NativeBuildInput.FromPath(context.engineRoot, Path.GetFullPath(path, context.engineRoot)));
            var inputs = request.configurationSnapshot.files.Concat(request.pluginSnapshots.SelectMany(static snapshot => snapshot.files)).Concat(analysis.snapshot.files)
                .Concat(analysis.argumentInputs?.files ?? []).Select(input => NativeBuildInput.FromPath(context.engineRoot, input.path)).ToList();
            inputs.AddRange(profileInputs);
            Assembly implementation = typeof(NativeBindingGenerator).Assembly;
            string implementationIdentity = implementation.GetCustomAttributes<AssemblyMetadataAttribute>()
                .Single(attribute => attribute.Key == "Inno.BindingImplementation").Value
                ?? throw new InvalidDataException("The binding implementation must declare its content identity.");
            declarations.Add("bindingImplementation=" + implementationIdentity);
            var assemblies = new[] { typeof(BindingHost).Assembly, typeof(BGCS.Core.BindingDiagnostic).Assembly,
                typeof(BGCS.Runtime.NativeModuleSession).Assembly }.Concat(definition.profile.GetExtensionAssemblies()).ToList();
            foreach (AssemblyName reference in assemblies.Prepend(implementation).SelectMany(static assembly => assembly.GetReferencedAssemblies())
                         .Where(static reference => reference.Name == "BGCS" || reference.Name!.StartsWith("BGCS.", StringComparison.Ordinal))
                         .DistinctBy(static reference => reference.FullName).ToArray())
                assemblies.Add(Assembly.Load(reference));
            foreach (Assembly assembly in assemblies.Distinct())
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

    private static string SerializeIdentity<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        WriteIdentity(writer, JsonSerializer.SerializeToElement(value, S_OPTIONS));
        writer.Flush();
        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }

    private static void WriteIdentity(
        Utf8JsonWriter writer,
        JsonElement value
    ) {
        // Dictionary enumeration is process-dependent; only object keys are unordered.
        if (value.ValueKind == JsonValueKind.Object)
        {
            writer.WriteStartObject();
            foreach (JsonProperty property in value.EnumerateObject().OrderBy(static property => property.Name, StringComparer.Ordinal))
            {
                writer.WritePropertyName(property.Name);
                WriteIdentity(writer, property.Value);
            }
            writer.WriteEndObject();
        }
        else if (value.ValueKind == JsonValueKind.Array)
        {
            writer.WriteStartArray();
            foreach (JsonElement element in value.EnumerateArray())
                WriteIdentity(writer, element);
            writer.WriteEndArray();
        }
        else
            value.WriteTo(writer);
    }
}
