using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Toolchains;
using Inno.Core.IO;

using Inno.Build.Toolchains.Browser;

namespace Inno.Build.SupportPacks;

internal sealed class BrowserPlayerSupportPackSource : IPlayerSupportPackSource
{
    /// <summary>
    /// Gets the platform identity whose closure this source prepares.
    /// </summary>
    public BuildTargetId target => BuildTargetId.browserWasm;

    /// <summary>
    /// Prepares the target runtime and compilation inputs in isolated staging.
    /// </summary>
    /// <param name="context">
    /// The selected source checkout, SDK and staging directory.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the process tree before installation.
    /// </param>
    /// <returns>
    /// Completion after the platform closure has been staged, or a propagated build failure.
    /// </returns>
    public async ValueTask PrepareAsync(
        PlayerSupportPackBuildContext context,
        CancellationToken cancellationToken
    ) {
        BrowserNativeArtifacts artifacts = await BrowserToolchain.BuildAsync(
            context.engineRoot, context.dotnetHost, cancellationToken).ConfigureAwait(false);
        string project = Path.Combine(context.engineRoot, "src", "composition", "player", "Inno.Player.Browser", "Inno.Player.Browser.csproj");
        var environment = await EmscriptenToolchainResolver.ResolveAsync(
            context.dotnetHost, project, cancellationToken).ConfigureAwait(false);
        string profileRoot = Path.Combine(context.engineRoot, "artifacts", "managed", "browser-references", artifacts.fingerprint);
        using FileLease ownership = await FileLease.AcquireAsync(
            profileRoot + ".lock", Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        await ToolchainEnvironment.RunAsync(context.dotnetHost,
            ["build", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "--configuration", "Release",
                "--nologo", "-p:InnoNativeTarget=browser-wasm", "-p:InnoNativeBuildFingerprint=" + artifacts.fingerprint,
                "-p:InnoNativeBindingSelection=" + artifacts.bindingSelectionPath,
                "-p:DebugType=None", "-p:DebugSymbols=false"],
            context.engineRoot, cancellationToken, environment).ConfigureAwait(false);
        PlayerSupportPackFiles.CopyReferences(
            Path.Combine(Path.GetDirectoryName(project)!, "bin", "browser-wasm", artifacts.fingerprint, "Release", "net9.0-browser"),
            Path.Combine(context.stagingDirectory, "References"));
        CopyLinkTemplate(context.engineRoot, context.stagingDirectory, artifacts);
    }

    /// <summary>
    /// Validates the prepared platform inputs before atomic installation.
    /// </summary>
    /// <param name="directory">
    /// The isolated target pack directory.
    /// </param>
    public void Validate(string directory) => new Inno.Build.Platform.Browser.BrowserSupportPackValidator().Validate(directory);

    private static string GetNativeComponent(string archive)
        => archive switch
        {
            "libSDL3.a" => "sdl3",
            "libminiaudio.a" => "miniaudio",
            "libinno-text.a" or "libfreetype.a" or "libharfbuzz.a" => "text",
            "libinno-ui.a" or "librmlui.a" => "ui",
            _ => "bgfx"
        };

    private static void CopyLinkTemplate(
        string engineRoot,
        string staging,
        BrowserNativeArtifacts artifacts
    ) {
        string template = Path.Combine(staging, "PlayerLink");
        string native = Path.Combine(template, "Native");
        string references = Path.Combine(template, "References");
        string webRoot = Path.Combine(template, "wwwroot");
        Directory.CreateDirectory(native);
        Directory.CreateDirectory(references);
        Directory.CreateDirectory(webRoot);
        CopyRequired(
            Path.Combine(engineRoot, "build", "support", "Inno.Build.SupportPacks", "Templates", "Browser", "BrowserPlayer.project.xml"),
            Path.Combine(template, "Player.csproj"));
        PlayerSupportPackFiles.CopyCompositionInputs(engineRoot, template);
        string browserSource = Path.Combine(engineRoot, "src", "composition", "player", "Inno.Player.Browser");
        foreach (string sourceName in new[] { "Program.cs", "BrowserPlayerComposition.cs", "BrowserContentLoader.cs", "BrowserBridge.cs" })
            CopyRequired(Path.Combine(browserSource, sourceName), Path.Combine(template, sourceName));
        foreach (string assetName in new[] { "index.html", "main.js" })
            CopyRequired(Path.Combine(browserSource, "wwwroot", assetName), Path.Combine(webRoot, assetName));

        string browserNative = artifacts.directory;
        foreach (string archiveName in new[]
        {
            "bgfxRelease.a", "bxRelease.a", "bimgRelease.a", "bimg_decodeRelease.a",
            "libSDL3.a", "libminiaudio.a", "libinno-text.a", "libinno-ui.a",
            "librmlui.a", "libfreetype.a", "libharfbuzz.a"
        })
        {
            CopyRequired(Path.Combine(browserNative, GetNativeComponent(archiveName), "browser-wasm", archiveName), Path.Combine(native, archiveName));
        }
        CopyRequired(
            Path.Combine(engineRoot, "build", "toolchains", "Inno.Build.Toolchains.Browser", "Native", "wasm_sjlj_shim.c"),
            Path.Combine(native, "wasm_sjlj_shim.c"));
        foreach (string file in Directory.EnumerateFiles(Path.Combine(staging, "References"), "*.dll"))
            File.Copy(file, Path.Combine(references, Path.GetFileName(file)));
        string runtime = Path.Combine(
            engineRoot, "native", "Inno.Native.Bgfx", "bin", "browser-wasm", artifacts.fingerprint, "Release", "net9.0", "BGCS.Runtime.dll");
        CopyRequired(runtime, Path.Combine(references, "BGCS.Runtime.dll"));
        foreach (string nativeName in new[]
        {
            "Inno.Native.Bgfx", "Inno.Native.MiniAudio",
            "Inno.Native.Sdl3", "Inno.Native.Text", "Inno.Native.UI"
        })
        {
            string binding = Path.Combine(
                engineRoot, "native", nativeName, "bin", "browser-wasm", artifacts.fingerprint, "Release", "net9.0", nativeName + ".dll");
            CopyRequired(binding, Path.Combine(references, nativeName + ".dll"));
        }
    }

    private static void CopyRequired(
        string source,
        string destination
    ) {
        if (!File.Exists(source))
            throw new FileNotFoundException($"Browser Support Pack input is missing: '{source}'.", source);
        File.Copy(source, destination, overwrite: true);
    }

}
