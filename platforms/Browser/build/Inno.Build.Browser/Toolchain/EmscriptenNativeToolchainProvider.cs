using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Browser;

/// <summary>
/// Selects Emscripten from the Browser product's actual SDK workload for an explicit wasm32 target.
/// </summary>
public sealed class EmscriptenNativeToolchainProvider : INativeToolchainProvider
{
    private readonly string m_dotnetHost;

    /// <summary>
    /// Captures the SDK executable selected by the build host without probing installed SDK versions.
    /// </summary>
    /// <param name="dotnetHost">
    /// The host-selected SDK executable or explicit command.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The SDK command is blank.
    /// </exception>
    public EmscriptenNativeToolchainProvider(string dotnetHost)
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
        if (targetId != "browser-wasm" || host.system is not ("Windows" or "MacOS" or "Linux"))
            throw new PlatformNotSupportedException($"Emscripten cannot build '{targetId}' on {host.system}/{host.architecture}.");
        string project = Path.Combine(context.engineRoot, "platforms", "Browser", "player",
            "Inno.Player.Browser", "Inno.Player.Browser.csproj");
        IReadOnlyDictionary<string, string> sdk = await EmscriptenToolchainResolver.ResolveAsync(
            m_dotnetHost, project, cancellationToken).ConfigureAwait(false);
        Dictionary<string, string> environment = NativeToolchainPreparation.CreateEnvironment();
        foreach ((string name, string value) in sdk)
            environment[name] = value;
        var tools = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["cmake"] = ToolchainEnvironment.ResolveExecutable("cmake"),
            ["ninja"] = ToolchainEnvironment.ResolveExecutable("ninja"),
            ["dotnet"] = sdk["DOTNET_HOST_PATH"],
            ["python"] = sdk["EMSDK_PYTHON"],
            ["node"] = sdk["EMSDK_NODE"]
        };
        string toolchain = Path.Combine(sdk["EMSCRIPTEN"], "cmake", "Modules", "Platform", "Emscripten.cmake");
        return NativeToolchainPreparation.Freeze(targetId, host, tools, environment,
            [Path.GetDirectoryName(sdk["EMSCRIPTEN"])!, Path.Combine(sdk["EM_CACHE"], "sysroot", "include")],
            ["-G", "Ninja", "-DCMAKE_MAKE_PROGRAM=" + tools["ninja"], "-DCMAKE_TOOLCHAIN_FILE=" + toolchain],
            ".a", multiConfiguration: false);
    }
}
