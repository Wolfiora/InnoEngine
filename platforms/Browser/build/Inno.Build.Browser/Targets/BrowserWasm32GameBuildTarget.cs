using System;
using Inno.Build.Managed;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Browser;

/// <summary>
/// Packages a verified browser Player as an independently hostable static site.
/// </summary>
public sealed class BrowserWasm32GameBuildTarget : IGameBuildTarget
{
    private readonly IPlayerSupportPackValidator m_supportPackValidator;


    /// <summary>
    /// Creates a site packager bound to the selected native closure validator.
    /// </summary>
    /// <param name="supportPackValidator">
    /// The borrowed validator for the distribution's browser Support Pack.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The validator is null.
    /// </exception>
    public BrowserWasm32GameBuildTarget(IPlayerSupportPackValidator supportPackValidator)
    {
        ArgumentNullException.ThrowIfNull(supportPackValidator);
        m_supportPackValidator = supportPackValidator;
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
    public void Validate(string directory) => m_supportPackValidator.Validate(directory);

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
