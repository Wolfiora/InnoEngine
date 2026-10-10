using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.MacOS;

/// <summary>
/// Resolves the MacOS SDK for a declared target without deriving publication policy from this process.
/// </summary>
public sealed class MacOSNativeToolchainProvider : INativeToolchainProvider
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
    public MacOSNativeToolchainProvider(string dotnetHost)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        m_dotnetHost = dotnetHost;
    }

    /// <inheritdoc />
    public async ValueTask<NativeToolchainSelection> ResolveAsync(
        NativeBuildContext context,
        BuildHostDescriptor host,
        string targetId,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(host);
        cancellationToken.ThrowIfCancellationRequested();
        if (!(host.system == "MacOS" && host.architecture == "arm64" && targetId == "macos-arm64"))
            throw new PlatformNotSupportedException($"The MacOS SDK cannot build '{targetId}' on {host.system}/{host.architecture}.");
        var tools = new Dictionary<string, string>(StringComparer.Ordinal) { ["dotnet"] = ToolchainEnvironment.ResolveExecutable(m_dotnetHost) };
        Dictionary<string, string> environment = NativeToolchainPreparation.CreateEnvironment();
        var inputs = new List<string>();
        var cmakeArguments = new List<string>();
        string xcrun = ToolchainEnvironment.ResolveExecutable("xcrun");
        tools["clang"] = (await ToolchainEnvironment.CaptureOutputAsync(xcrun,
            ["--find", "clang"], context.engineRoot, cancellationToken).ConfigureAwait(false)).Trim();
        tools["clang++"] = (await ToolchainEnvironment.CaptureOutputAsync(xcrun,
            ["--find", "clang++"], context.engineRoot, cancellationToken).ConfigureAwait(false)).Trim();
        string sdk = (await ToolchainEnvironment.CaptureOutputAsync(xcrun,
            ["--sdk", "macosx", "--show-sdk-path"], context.engineRoot, cancellationToken).ConfigureAwait(false)).Trim();
        environment["SDKROOT"] = sdk;
        inputs.AddRange([xcrun, Path.Combine(sdk, "usr", "include"), Path.Combine(sdk, "usr", "lib")]);
        cmakeArguments.AddRange(["-DCMAKE_OSX_SYSROOT=" + sdk, "-DCMAKE_OSX_ARCHITECTURES=arm64"]);
        SelectUnixTools(tools, environment, cmakeArguments);

        return NativeToolchainPreparation.Freeze(targetId, host, tools, environment, inputs,
            cmakeArguments, ".dylib", false);
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
