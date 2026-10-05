using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Core.IO;
using System.Xml.Linq;

namespace Inno.Build.Toolchains.Browser;

/// <summary>
/// Builds static native artifacts with the Emscripten SDK owned by the selected .NET workload.
/// </summary>
public static class BrowserToolchain
{
    private static readonly string[] NativeOutputDirectories = ["bgfx", "sdl3", "miniaudio", "text", "ui", "Metadata"];

    /// <summary>
    /// Builds the native closure from source and publishes complete outputs by immutable input identity.
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
    /// The selected native artifacts after generation, compilation and integrity validation.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// A native build or binding generator process fails.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller canceled the build.
    /// </exception>
    public static async Task<BrowserNativeArtifacts> BuildAsync(
        string engineRoot,
        string dotnetHost,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        string root = Path.GetFullPath(engineRoot);
        string playerProject = Path.Combine(root, "src", "composition", "player", "Inno.Player.Browser", "Inno.Player.Browser.csproj");
        IReadOnlyDictionary<string, string> environment = await EmscriptenToolchainResolver.ResolveAsync(
            dotnetHost, playerProject, cancellationToken).ConfigureAwait(false);
        string owner = Path.Combine(root, "build", "toolchains", "Inno.Build.Toolchains.Browser");
        string toolchain = Path.Combine(environment["EMSCRIPTEN"], "cmake", "Modules", "Platform", "Emscripten.cmake");
        string cmake = ToolchainEnvironment.ResolveExecutable("cmake");
        string ninja = ToolchainEnvironment.ResolveExecutable("ninja");
        Dictionary<string, NativeBindingGenerationDescriptor> generations = new(StringComparer.Ordinal);
        string requests = Path.Combine(owner, "obj", "requests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(requests);
        try
        {
            foreach (string component in new[] { "Bgfx", "Sdl3", "MiniAudio", "Text", "UI" })
            {
                string project = Path.Combine(root, "native", "Inno.Native." + component,
                    "Inno.Native." + component + ".csproj");
                string descriptor = Path.Combine(requests, component + ".json");
                await ToolchainEnvironment.RunAsync(environment["DOTNET_HOST_PATH"],
                    ["build", project, "-t:GenerateBindings", "--configuration", "Release",
                        "--disable-build-servers", "-m:1", "-nodeReuse:false",
                        "-p:InnoNativeTarget=browser-wasm", "-p:BindGenDescriptorOutput=" + descriptor],
                    root, cancellationToken, environment).ConfigureAwait(false);
                generations.Add(component, NativeBindingGenerationDescriptor.Load(descriptor));
            }
        }
        finally
        {
            Directory.Delete(requests, recursive: true);
        }
        cancellationToken.ThrowIfCancellationRequested();
        string[] declarations = environment.Select(pair => pair.Key + "=" + pair.Value)
                .Concat(generations.Select(pair => pair.Key + "=" + pair.Value.fingerprint))
                .Append("browser-wasm/Release/Ninja")
                .Append(typeof(BrowserToolchain).Assembly.ManifestModule.ModuleVersionId.ToString()).ToArray();
        string[] inputs = EnumerateNativeSources(Path.Combine(root, "extern"))
                .Concat(EnumerateNativeSources(Path.Combine(root, "native")))
                .Concat(EnumerateNativeSources(Path.Combine(owner, "Native")))
                .Concat(EnumerateToolchainInputs(Path.GetDirectoryName(environment["EMSCRIPTEN"])!))
                .Concat([toolchain, cmake, ninja, environment["EMSDK_NODE"], environment["EMSDK_PYTHON"]])
                .ToArray();
        string fingerprint = NativeBuildFingerprint.Create(declarations, inputs);
        string intermediate = Path.Combine(owner, "obj", "native", "browser-wasm", fingerprint);
        string destination = Path.Combine(root, "artifacts", "native", "browser", "browser-wasm", fingerprint);
        using FileLease ownership = await FileLease.AcquireAsync(
            intermediate + ".lock", Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        if (BuildArtifactManifest.IsComplete(destination, fingerprint, NativeOutputDirectories))
            return new BrowserNativeArtifacts(fingerprint, destination);

        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(staging);
        try
        {
            await ToolchainEnvironment.RunAsync(cmake,
                ["-S", Path.Combine(owner, "Native"), "-B", intermediate, "-G", "Ninja",
                    "-DCMAKE_MAKE_PROGRAM=" + ninja,
                    "-DCMAKE_BUILD_TYPE=Release", "-DCMAKE_TOOLCHAIN_FILE=" + toolchain,
                    "-DCMAKE_INSTALL_PREFIX=" + staging,
                    "-DINNO_UI_BRIDGE_ROOT=" + generations["UI"].bridgeDirectory,
                    "-DINNO_TEXT_BRIDGE_ROOT=" + generations["Text"].bridgeDirectory,
                    "-DPython3_EXECUTABLE=" + environment["EMSDK_PYTHON"]],
                root, cancellationToken, environment).ConfigureAwait(false);
            await ToolchainEnvironment.RunAsync(cmake, ["--build", intermediate],
                root, cancellationToken, environment).ConfigureAwait(false);
            await ToolchainEnvironment.RunAsync(cmake, ["--install", intermediate, "--component", "Inno"],
                root, cancellationToken, environment).ConfigureAwait(false);
            ValidateNativeOutputs(staging);
            if (NativeBuildFingerprint.Create(declarations, inputs) != fingerprint)
                throw new InvalidOperationException("Native build inputs changed during compilation; the candidate was not published.");
            string metadata = Path.Combine(staging, "Metadata");
            Directory.CreateDirectory(metadata);
            new XDocument(new XElement("Project", generations.OrderBy(pair => pair.Key, StringComparer.Ordinal)
                .Select(pair => new XElement("PropertyGroup",
                    new XAttribute("Condition", "'$(MSBuildProjectName)' == 'Inno.Native." + pair.Key + "'"),
                    new XElement("BindGenExpectedFingerprint", pair.Value.fingerprint)))))
                .Save(Path.Combine(metadata, "BindingSelection.props"));
            BuildArtifactManifest.Write(staging, fingerprint, NativeOutputDirectories);
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Install(staging, destination);
            return new BrowserNativeArtifacts(fingerprint, destination);
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }

    private static IEnumerable<string> EnumerateToolchainInputs(string directory)
    {
        foreach (string file in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
        {
            string extension = Path.GetExtension(file).ToLowerInvariant();
            if (extension is ".exe" or ".dll" or ".py" or ".js" or ".mjs" or ".cmake" or ".sh"
                || !OperatingSystem.IsWindows() && extension.Length == 0
                    && Path.GetFileName(Path.GetDirectoryName(file)) == "bin")
                yield return file;
        }
    }

    private static void ValidateNativeOutputs(string directory)
    {
        foreach ((string component, string archive) in new (string, string)[]
        {
            ("bgfx", "bgfxRelease.a"), ("bgfx", "bxRelease.a"),
            ("bgfx", "bimgRelease.a"), ("bgfx", "bimg_decodeRelease.a"),
            ("sdl3", "libSDL3.a"), ("miniaudio", "libminiaudio.a"),
            ("text", "libinno-text.a"), ("text", "libfreetype.a"), ("text", "libharfbuzz.a"),
            ("ui", "libinno-ui.a"), ("ui", "librmlui.a")
        })
        {
            string file = Path.Combine(directory, component, "browser-wasm", archive);
            if (!File.Exists(file) || new FileInfo(file).Length == 0)
                throw new InvalidDataException($"The native candidate is missing a complete archive at '{file}'.");
        }
    }

    private static IEnumerable<string> EnumerateNativeSources(string root)
    {
        foreach (string path in Directory.EnumerateFiles(root))
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            if (Path.GetFileName(path) == "CMakeLists.txt"
                || extension is ".c" or ".cpp" or ".cxx" or ".cc" or ".h" or ".hpp" or ".hxx"
                    or ".inc" or ".in" or ".cmake" or ".def" or ".rc" or ".s" or ".asm")
                yield return Path.GetFullPath(path);
        }
        foreach (string directory in Directory.EnumerateDirectories(root))
        {
            if (Path.GetFileName(directory) is ".git" or ".build" or "obj" or "bin" or "Generated" or "Bindings")
                continue;
            foreach (string path in EnumerateNativeSources(directory))
                yield return path;
        }
    }

}
