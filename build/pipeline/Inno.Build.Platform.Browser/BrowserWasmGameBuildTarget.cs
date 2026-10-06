using System;
using Inno.Build.Managed;
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

    /// <inheritdoc />
    public ManagedDeploymentId defaultManagedDeployment => ManagedDeploymentId.monoWasm;

    /// <inheritdoc />
    public string runtimeIdentifier => "browser-wasm";

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
        await CopyDirectoryAsync(
            Path.Combine(context.managedDeployment.outputDirectory, "wwwroot"),
            site,
            cancellationToken);
        if (!File.Exists(Path.Combine(site, "index.html")))
            throw new InvalidDataException("The browser Support Pack has no index.html entry point.");
        string contentDirectory = Path.Combine(site, "Content");
        await CopyDirectoryAsync(context.contentDirectory, contentDirectory, cancellationToken);
        string[] packs = Directory.GetFiles(contentDirectory, "content-*.pack", SearchOption.TopDirectoryOnly);
        if (packs.Length != 1)
            throw new InvalidDataException("The browser deployment must contain exactly one content pack.");
        return site;
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
