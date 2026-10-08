using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Linux;

/// <summary>
/// Resolves the Linux SDK for a declared target without deriving publication policy from this process.
/// </summary>
public sealed class LinuxNativeToolchainProvider : INativeToolchainProvider
{
    private readonly string m_dotnetHost;

    /// <summary>
    /// Captures the explicit managed executable used by Native binding generation.
    /// </summary>
    /// <param name="dotnetHost">
    /// The explicitly selected SDK executable path or command.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The host command is empty.
    /// </exception>
    public LinuxNativeToolchainProvider(string dotnetHost)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        m_dotnetHost = dotnetHost;
    }

    /// <inheritdoc />
    public ValueTask<NativeToolchainSelection> ResolveAsync(
        NativeBuildContext context,
        BuildHostDescriptor host,
        string targetId,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(host);
        cancellationToken.ThrowIfCancellationRequested();
        if (!(host.system == "Linux" && (targetId == "linux-x64" && host.architecture == "x64"
            || targetId == "linux-arm64" && host.architecture == "arm64")))
            throw new PlatformNotSupportedException($"The Linux SDK cannot build '{targetId}' on {host.system}/{host.architecture}.");
        var tools = new Dictionary<string, string>(StringComparer.Ordinal) { ["dotnet"] = ToolchainEnvironment.ResolveExecutable(m_dotnetHost) };
        Dictionary<string, string> environment = NativeToolchainPreparation.CreateEnvironment();
        var inputs = new List<string>();
        var cmakeArguments = new List<string>();
        tools["clang"] = ToolchainEnvironment.ResolveExecutable("clang");
        tools["clang++"] = ToolchainEnvironment.ResolveExecutable("clang++");
        tools["gcc"] = ToolchainEnvironment.ResolveExecutable("gcc");
        tools["g++"] = ToolchainEnvironment.ResolveExecutable("g++");
        SelectUnixTools(tools, environment, cmakeArguments);

        return ValueTask.FromResult(NativeToolchainPreparation.Freeze(targetId, host, tools, environment, inputs,
            cmakeArguments, ".so", false));
    }

    private static void SelectUnixTools(
        Dictionary<string, string> tools,
        Dictionary<string, string> environment,
        List<string> cmakeArguments
    ) {
        tools["make"] = ToolchainEnvironment.ResolveExecutable("make");
        tools["cmake"] = ToolchainEnvironment.ResolveExecutable("cmake");
        environment["CC"] = tools.GetValueOrDefault("gcc", tools["clang"]);
        environment["CXX"] = tools.GetValueOrDefault("g++", tools["clang++"]);
        cmakeArguments.AddRange(["-DCMAKE_C_COMPILER=" + environment["CC"],
            "-DCMAKE_CXX_COMPILER=" + environment["CXX"]]);
    }
}
