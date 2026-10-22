using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using BGCS;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal sealed class BindingGenerationConfiguration
{
    internal BindingGenerationConfiguration(
        NativeComponentDescriptor component,
        string definition,
        string configPath,
        NativeBindingGenerationRequest request,
        NativeBindingProfile profile
    ) {
        this.component = component;
        this.definition = definition;
        this.configPath = configPath;
        this.request = request;
        this.profile = profile;
    }

    internal NativeComponentDescriptor component { get; }
    internal string definition { get; }
    internal string configPath { get; }
    internal NativeBindingGenerationRequest request { get; }
    internal NativeBindingProfile profile { get; }

    internal GenerationRequest Load(NativeBuildContext context)
    {
        if (context.toolchain is NativeToolchainSelection selected && selected.targetId != request.targetId)
            throw new InvalidOperationException("The selected SDK does not match the binding target.");
        GenerationRequest captured = GenerationRequest.FromFile(configPath);
        if (captured.configuration.targets.Length != 1)
            throw new InvalidDataException("A component binding request must select exactly one native target.");
        if (captured.configuration.output.layout != "singleFile" || captured.configuration.output.singleFileName != "Bindings.cs"
            || captured.configuration.runtime.mode != "package")
            throw new InvalidDataException("Native components require one Bindings.cs source and the package Runtime.");
        captured = captured.WithConfiguration(captured.configuration with
        {
            cache = captured.configuration.cache with
            {
                directory = Path.Combine(context.engineRoot, "artifacts", "build-tools", "bindings", "sdk-workspaces", component.id, request.targetId)
            }
        });
        return profile.Apply(captured, context.toolchain?.compilerInputs,
            request.componentOptions.GetValueOrDefault(component.nativeProject), context.engineRoot);
    }

    internal string GetOutputRoot(NativeBuildContext context) => request.outputMode == NativeBindingOutputMode.TargetArtifacts
        ? Path.Combine(context.engineRoot, "artifacts", "bindings", component.id, request.targetId)
        : Path.Combine(component.GetNativeRoot(context.engineRoot), "Generated");

    internal string GetHostNativeRoot(NativeBuildContext context) => Path.Combine(component.GetNativeRoot(context.engineRoot), "Native", "Generated");
}
