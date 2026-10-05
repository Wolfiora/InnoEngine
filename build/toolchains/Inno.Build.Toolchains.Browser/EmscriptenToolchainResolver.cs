using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Browser;

/// <summary>
/// Resolves Emscripten from the workload selected by one application project.
/// </summary>
public static class EmscriptenToolchainResolver
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
    public static async Task<IReadOnlyDictionary<string, string>> ResolveAsync(
        string dotnetHost,
        string projectPath,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);
        string host = ToolchainEnvironment.ResolveExecutable(dotnetHost);
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
            : ToolchainEnvironment.ResolveExecutable("python3");
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

}
