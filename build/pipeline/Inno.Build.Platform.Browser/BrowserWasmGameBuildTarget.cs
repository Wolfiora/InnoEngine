using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Build.Platform.Browser;

/// <summary>
/// Compiles WebGL 2 content and packages a browser Player as a static site.
/// </summary>
public sealed class BrowserWasmGameBuildTarget : IGameBuildTarget
{
    private static readonly IPlayerSupportPackValidator S_SUPPORT_PACK_VALIDATOR = new BrowserSupportPackValidator();
    private readonly BgfxGameContentCompiler m_contentCompiler;

    /// <summary>
    /// Creates a browser target over the active authoring generation.
    /// </summary>
    /// <param name="assets">
    /// The authoring asset pipeline used to compile target artifacts.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry that owns shader contracts.
    /// </param>
    /// <param name="types">
    /// The active shader extension generation owner.
    /// </param>
    public BrowserWasmGameBuildTarget(
        AssetPipeline assets,
        SerializationRegistry serialization,
        TypeCatalog types
    ) {
        m_contentCompiler = BgfxGameContentCompiler.CreateBrowserWasm(assets, serialization, types);
    }

    /// <summary>
    /// Validates the target closure before runtime script compilation.
    /// </summary>
    /// <param name="directory">
    /// The Support Pack directory selected by the build catalog.
    /// </param>
    /// <exception cref="System.IO.InvalidDataException">
    /// The required platform inputs are incomplete or incompatible.
    /// </exception>
    public void Validate(string directory) => S_SUPPORT_PACK_VALIDATOR.Validate(directory);

    /// <summary>
    /// Gets the browser WebAssembly target identity.
    /// </summary>
    public BuildTargetId id => BuildTargetId.browserWasm;

    /// <summary>
    /// Gets the name shown by authoring hosts.
    /// </summary>
    public string displayName => "Web (WebGL 2)";

    /// <summary>
    /// Gets whether this target should replace the host's native default.
    /// </summary>
    public bool isPreferredOnCurrentHost => false;

    /// <summary>
    /// Compiles browser-compatible shader and texture artifacts into staging.
    /// </summary>
    /// <param name="context">
    /// The isolated target-content staging context.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels content compilation before package commit.
    /// </param>
    /// <returns>
    /// An operation that completes after browser artifacts are staged.
    /// </returns>
    public ValueTask BuildContentAsync(
        GameBuildContentContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        return m_contentCompiler.CompileAsync(context, cancellationToken);
    }

    /// <summary>
    /// Composes an independently hostable site from a verified browser Support Pack and content pack.
    /// </summary>
    /// <param name="context">
    /// The verified runtime and packaged project content.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels copying before the build pipeline commits the site.
    /// </param>
    /// <returns>
    /// The staged static-site directory.
    /// </returns>
    public async ValueTask<string> PackageAsync(
        GameBuildPackageContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        string site = Path.Combine(context.outputDirectory, context.profile.productName + "-Web");
        string link = Path.Combine(context.outputDirectory, "PlayerLink");
        await CopyDirectoryAsync(
            Path.Combine(context.supportPackDirectory, "PlayerLink"),
            link,
            cancellationToken);
        string published = Path.Combine(context.outputDirectory, "Published");
        await PublishGameAsync(
            link,
            context.runtimeAssemblyDirectory,
            published,
            cancellationToken);
        await CopyDirectoryAsync(
            Path.Combine(published, "wwwroot"),
            site,
            cancellationToken);
        if (!File.Exists(Path.Combine(site, "index.html")))
            throw new InvalidDataException("The browser Support Pack has no index.html entry point.");
        string contentDirectory = Path.Combine(site, "Content");
        await CopyDirectoryAsync(context.contentDirectory, contentDirectory, cancellationToken);
        string[] packs = Directory.GetFiles(contentDirectory, "content-*.pack", SearchOption.TopDirectoryOnly);
        if (packs.Length != 1)
            throw new InvalidDataException("The browser deployment must contain exactly one content pack.");
        await File.WriteAllTextAsync(
            Path.Combine(contentDirectory, "content-pack.txt"),
            Path.GetFileName(packs[0]),
            cancellationToken);
        return site;
    }

    private static async ValueTask PublishGameAsync(
        string linkDirectory,
        string runtimeAssemblyDirectory,
        string outputDirectory,
        CancellationToken cancellationToken
    ) {
        string project = Path.Combine(linkDirectory, "BrowserPlayer.csproj");
        if (!File.Exists(project))
            throw new FileNotFoundException("The browser Support Pack has no game linker project.", project);
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";
        if (string.IsNullOrWhiteSpace(host))
            throw new InvalidOperationException("Browser export requires a .NET SDK host with wasm-tools installed.");
        var startInfo = new ProcessStartInfo
        {
            FileName = host,
            WorkingDirectory = linkDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        foreach (string argument in new[]
        {
            "publish", project, "--disable-build-servers", "-m:1", "-nodeReuse:false",
            "--configuration", "Release", "--output", outputDirectory, "--nologo",
            "-p:DebugType=None", "-p:DebugSymbols=false",
            "-p:InnoBrowserGameManagedRoot=" + Path.GetFullPath(runtimeAssemblyDirectory)
        })
        {
            startInfo.ArgumentList.Add(argument);
        }
        using var process = new Process { StartInfo = startInfo };
        try
        {
            if (!process.Start())
                throw new InvalidOperationException("The browser game linker could not start the .NET SDK.");
        }
        catch (Win32Exception exception)
        {
            throw new InvalidOperationException(
                "Browser export requires a .NET SDK host with wasm-tools installed. " +
                "Set DOTNET_HOST_PATH to its executable if it is not on PATH.", exception);
        }
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        try
        {
            await process.WaitForExitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(CancellationToken.None);
            throw;
        }
        finally
        {
            await Task.WhenAll(output, error);
        }
        string standardOutput = await output;
        string standardError = await error;
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Browser game link failed with exit code {process.ExitCode}." +
                Environment.NewLine + standardOutput + Environment.NewLine + standardError);
        }
    }

    private static async ValueTask CopyDirectoryAsync(
        string source,
        string destination,
        CancellationToken cancellationToken
    ) {
        Directory.CreateDirectory(destination);
        foreach (string file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string relativePath = Path.GetRelativePath(source, file);
            string target = Path.Combine(destination, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            await using FileStream input = new(file, FileMode.Open, FileAccess.Read, FileShare.Read, 131072, true);
            await using FileStream output = new(target, FileMode.CreateNew, FileAccess.Write, FileShare.None, 131072, true);
            await input.CopyToAsync(output, cancellationToken);
        }
    }
}
