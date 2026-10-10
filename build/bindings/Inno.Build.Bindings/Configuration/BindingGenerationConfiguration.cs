using System.IO;
using BGCS.Configuration;
using BGCS.Cpp2C.Configuration;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal sealed class BindingGenerationConfiguration
{
    internal BindingGenerationConfiguration(
        NativeComponentDescriptor component,
        string definition,
        string configPath,
        string? bridgePath,
        string? hostBridgePath,
        string? extensionProject,
        NativeBindingGenerationRequest request
    ) {
        this.component = component;
        this.definition = definition;
        this.configPath = configPath;
        this.bridgePath = bridgePath;
        this.hostBridgePath = hostBridgePath;
        this.extensionProject = extensionProject;
        this.request = request;
    }

    internal string? extensionAssembly { get; set; }
    internal NativeComponentDescriptor component { get; }
    internal string definition { get; }
    internal string configPath { get; }
    internal string? bridgePath { get; }
    internal string? hostBridgePath { get; }
    internal string? extensionProject { get; }
    internal NativeBindingGenerationRequest request { get; }

    internal (CsCodeGeneratorConfig managed, Cpp2CGeneratorConfig? bridge, string? hostBridgeRoot) Load(NativeBuildContext context)
    {
        var managed = new ConfigLoader().Load(configPath);
        if (extensionAssembly is not null)
            BGCS.Core.Extensibility.BindingPluginLoader.Load(extensionAssembly, managed.plugins);
        Cpp2CGeneratorConfig? bridge = bridgePath is null ? null : Cpp2CGeneratorConfig.Load(bridgePath);
        Cpp2CGeneratorConfig? hostBridge = hostBridgePath is null ? null : Cpp2CGeneratorConfig.Load(hostBridgePath);
        BindingConfigurationPaths.Apply(managed, context.toolchain?.environment);
        if (bridge is not null)
            BindingConfigurationPaths.Apply(bridge, context.toolchain?.environment);
        if (hostBridge is not null)
            BindingConfigurationPaths.Apply(hostBridge, context.toolchain?.environment);
        string? hostBridgeRoot = hostBridge is null ? null
            : NativeBindingGenerationIdentity.Resolve(hostBridge.outputPath, hostBridge.configDirectory!);
        return (managed, bridge, hostBridgeRoot);
    }

    internal string GetOutputRoot(NativeBuildContext context)
    {
        if (request.outputMode == NativeBindingOutputMode.TargetArtifacts)
            return Path.Combine(component.GetNativeRoot(context.engineRoot), "obj", request.targetId);
        var managed = new ConfigLoader().Load(configPath);
        return NativeBindingGenerationIdentity.Resolve(BindingConfigurationPaths.Expand(managed.outputPath,
            context.toolchain?.environment)!, Path.GetDirectoryName(configPath)!);
    }
}
