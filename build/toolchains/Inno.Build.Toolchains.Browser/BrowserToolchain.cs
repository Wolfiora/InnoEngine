using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Inno.Build.Toolchains.Browser;

/// <summary>
/// Builds static native artifacts with the Emscripten SDK owned by the selected .NET workload.
/// </summary>
public static class BrowserToolchain
{
    /// <summary>
    /// Resolves the native toolchain actually selected by MSBuild for one browser project.
    /// </summary>
    /// <param name="dotnetHost">
    /// The SDK host executable, either an absolute path or a name available on PATH.
    /// </param>
    /// <param name="projectPath">
    /// The browser project whose target framework and SDK determine workload selection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the hidden SDK query and waits for its exit.
    /// </param>
    /// <returns>
    /// Variables for native builds and binding generation, without changing the parent process.
    /// </returns>
    /// <remarks>
    /// Compiler worker budgets remain inherited from the host environment or tool defaults.
    /// Runtime threading policy does not constrain build-time parallelism.
    /// </remarks>
    /// <exception cref="FileNotFoundException">
    /// The SDK host or a required Emscripten workload component is missing.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The SDK host or project path is empty.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// A selected SDK directory or sysroot is missing.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Project evaluation fails or does not select a complete native toolchain.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static async Task<IReadOnlyDictionary<string, string>> ResolveEnvironmentAsync(
        string dotnetHost,
        string projectPath,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);
        string host = ResolveExecutable(dotnetHost);
        host = File.ResolveLinkTarget(host, returnFinalTarget: true)?.FullName ?? host;
        string project = Path.GetFullPath(projectPath);
        if (!File.Exists(project))
            throw new FileNotFoundException("The browser toolchain project is missing.", project);
        string output = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", project, "-nologo", "-nodeReuse:false",
                "-getProperty:EmscriptenSdkToolsPath,EmscriptenNodeToolsPath,EmscriptenPythonToolsPath,EmscriptenCacheSdkCacheDir"],
            Path.GetDirectoryName(project)!, cancellationToken,
            new Dictionary<string, string> { ["DOTNET_NOLOGO"] = "true" }).ConfigureAwait(false);
        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement properties = document.RootElement.GetProperty("Properties");
        string root = Path.GetDirectoryName(host)!;
        string sdk = ReadToolPath(properties, "EmscriptenSdkToolsPath");
        string cache = ReadToolPath(properties, "EmscriptenCacheSdkCacheDir");
        string node = Path.Combine(ReadToolPath(properties, "EmscriptenNodeToolsPath"), "bin",
            OperatingSystem.IsWindows() ? "node.exe" : "node");
        string python = OperatingSystem.IsWindows()
            ? Path.Combine(ReadToolPath(properties, "EmscriptenPythonToolsPath"), "python.exe")
            : ResolveExecutable("python3");
        string clang = Path.Combine(sdk, "bin", OperatingSystem.IsWindows() ? "clang.exe" : "clang");
        string clangCpp = Path.Combine(sdk, "bin", OperatingSystem.IsWindows() ? "clang++.exe" : "clang++");
        foreach (string file in new[] { host, node, python, clang, clangCpp })
            if (!File.Exists(file))
                throw new FileNotFoundException("An installed WebAssembly toolchain executable is missing.", file);
        if (!Directory.Exists(Path.Combine(cache, "sysroot")))
            throw new DirectoryNotFoundException($"The WebAssembly SDK sysroot is missing at '{cache}'.");
        return new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["DOTNET_HOST_PATH"] = host,
            ["DOTNET_ROOT"] = root,
            ["DOTNET_EMSCRIPTEN_LLVM_ROOT"] = Path.Combine(sdk, "bin"),
            ["BGCS_CC"] = clang,
            ["BGCS_CPP2C_CXX"] = clangCpp,
            ["DOTNET_EMSCRIPTEN_BINARYEN_ROOT"] = sdk,
            ["DOTNET_EMSCRIPTEN_NODE_JS"] = node,
            ["EM_CACHE"] = cache,
            ["EMSDK_PYTHON"] = python,
            ["EMSDK_NODE"] = node,
            ["EMSCRIPTEN"] = Path.Combine(sdk, "emscripten")
        };
    }

    /// <summary>
    /// Builds and installs the native closure from source in the owning toolchain intermediates.
    /// </summary>
    /// <param name="engineRoot">
    /// The engine checkout containing the pinned third-party sources.
    /// </param>
    /// <param name="dotnetHost">
    /// The .NET SDK host with wasm-tools installed.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels configuration, compilation, installation or binding generation.
    /// </param>
    /// <returns>
    /// Completion after native libraries and all five target binding assemblies are available.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// A native build or binding generator process fails.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller canceled the build.
    /// </exception>
    public static async Task BuildAsync(
        string engineRoot,
        string dotnetHost,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        string root = Path.GetFullPath(engineRoot);
        string playerProject = Path.Combine(root, "src", "composition", "player", "Inno.Player.Browser", "Inno.Player.Browser.csproj");
        IReadOnlyDictionary<string, string> environment = await ResolveEnvironmentAsync(
            dotnetHost, playerProject, cancellationToken).ConfigureAwait(false);
        string owner = Path.Combine(root, "build", "toolchains", "Inno.Build.Toolchains.Browser");
        string intermediate = Path.Combine(owner, "obj", "native", "browser-wasm");
        string toolchain = Path.Combine(environment["EMSCRIPTEN"], "cmake", "Modules", "Platform", "Emscripten.cmake");
        foreach (string component in new[] { "Bgfx", "SDL3", "MiniAudio", "Text", "UI" })
        {
            string project = Path.Combine(root, "native", "Inno.Native." + component,
                "Inno.Native." + (component == "SDL3" ? "Sdl3" : component) + ".csproj");
            await ToolchainEnvironment.RunAsync(environment["DOTNET_HOST_PATH"],
                ["build", project, "-t:GenerateBindings", "--configuration", "Release", "-nodeReuse:false",
                    "-p:InnoNativeTarget=browser-wasm"],
                root, cancellationToken, environment).ConfigureAwait(false);
        }
        await ToolchainEnvironment.RunAsync("cmake",
            ["-S", Path.Combine(owner, "Native"), "-B", intermediate, "-G", "Ninja",
                "-DCMAKE_BUILD_TYPE=Release", "-DCMAKE_TOOLCHAIN_FILE=" + toolchain,
                "-DCMAKE_INSTALL_PREFIX=" + Path.Combine(root, ".lib"),
                "-DPython3_EXECUTABLE=" + environment["EMSDK_PYTHON"]],
            root, cancellationToken, environment).ConfigureAwait(false);
        await ToolchainEnvironment.RunAsync("cmake", ["--build", intermediate],
            root, cancellationToken, environment).ConfigureAwait(false);
        await ToolchainEnvironment.RunAsync("cmake", ["--install", intermediate, "--component", "Inno"],
            root, cancellationToken, environment).ConfigureAwait(false);

    }

    private static string ReadToolPath(
        JsonElement properties,
        string name
    ) {
        if (!properties.TryGetProperty(name, out JsonElement value) || string.IsNullOrWhiteSpace(value.GetString()))
            throw new InvalidOperationException($"The browser project did not select '{name}'. Install wasm-tools with the selected SDK.");
        string path = Path.GetFullPath(value.GetString()!);
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"The browser SDK's '{name}' directory is missing at '{path}'.");
        return path;
    }

    private static string ResolveExecutable(string name)
    {
        if (File.Exists(name))
            return Path.GetFullPath(name);
        string fileName = OperatingSystem.IsWindows() && !Path.HasExtension(name) ? name + ".exe" : name;
        foreach (string directory in (Environment.GetEnvironmentVariable("PATH") ?? string.Empty).Split(Path.PathSeparator))
        {
            string candidate = Path.Combine(directory, fileName);
            if (File.Exists(candidate))
                return Path.GetFullPath(candidate);
        }
        throw new FileNotFoundException($"Cannot locate toolchain executable '{name}'.", name);
    }
}
