using System;
using System.Collections.Immutable;
using System.Linq;
using BGCS;
using BGCS.Configuration;
using BGCS.Contracts.Inputs;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class BindingConfigurationPaths
{
    internal static GenerationRequest Apply(
        GenerationRequest request,
        NativeToolchainSelection? toolchain
    ) {
        if (toolchain is null)
            return request;
        BindingConfiguration config = request.configuration;
        string[] compilerNames = config.source.language == SourceLanguage.Cpp ? ["clang++", "c++", "cl"] : ["clang", "cc", "cl"];
        string? compiler = null;
        foreach (string name in compilerNames)
            if (toolchain.TryResolveExecutable(name, out string? selected))
            {
                compiler = selected;
                break;
            }
        ImmutableArray<string> includes = toolchain.environment.TryGetValue("INCLUDE", out string? include)
            ? include.Split(';', StringSplitOptions.RemoveEmptyEntries).ToImmutableArray() : [];
        string? sysRoot = toolchain.environment.TryGetValue("EM_CACHE", out string? cache)
            ? System.IO.Path.Combine(cache, "sysroot")
            : (toolchain.environment.TryGetValue("SDKROOT", out string? sdkRoot) ? sdkRoot : null);
        return request.WithConfiguration(config with
        {
            targets = config.targets.Select(target => target with
            {
                settings = target.settings with
                {
                    compilerPath = compiler ?? target.settings.compilerPath,
                    sysRoot = sysRoot ?? target.settings.sysRoot,
                    systemIncludeDirectories = includes.IsDefaultOrEmpty ? target.settings.systemIncludeDirectories : includes
                }
            }).ToImmutableArray()
        });
    }
}
