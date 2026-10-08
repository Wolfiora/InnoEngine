using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Toolchains;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Prepares file-based product inputs using injected target tools, source locations and native closure.
/// </summary>
public sealed class FilePlayerSupportPackPreparation
{
    private readonly BuildTargetId m_target;
    private readonly IPlayerSupportPackValidator m_validator;
    private readonly string m_runtimeIdentifier;
    private readonly string m_nativePlatform;
    private readonly string m_nativeExtension;
    private readonly string m_productProject;
    private readonly string m_templateFile;
    private readonly ProductNativeBuildPlan m_nativePlan;
    private readonly INativeToolchainProvider m_toolchainProvider;
    private readonly BuildHostDescriptor m_host;

    /// <summary>
    /// Captures the platform layout and validator without preparing any tools or files.
    /// </summary>
    /// <param name="target">
    /// The publication platform identity.
    /// </param>
    /// <param name="runtimeIdentifier">
    /// The managed SDK runtime target.
    /// </param>
    /// <param name="nativePlatform">
    /// The native toolchain target identity required by this source.
    /// </param>
    /// <param name="nativeExtension">
    /// The runtime library suffix selected by this platform.
    /// </param>
    /// <param name="validator">
    /// The borrowed platform validator used before publication.
    /// </param>
    /// <param name="productProject">
    /// The explicit checkout-relative product project.
    /// </param>
    /// <param name="templateFile">
    /// The explicit checkout-relative publication template.
    /// </param>
    /// <param name="nativePlan">
    /// The frozen product component closure.
    /// </param>
    /// <param name="toolchainProvider">
    /// The SDK provider for the explicit native target.
    /// </param>
    /// <param name="host">
    /// The declared machine executing tools.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A platform identifier or required target string is blank.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The validator is null.
    /// </exception>
    public FilePlayerSupportPackPreparation(
        BuildTargetId target,
        string runtimeIdentifier,
        string nativePlatform,
        string nativeExtension,
        IPlayerSupportPackValidator validator,
        string productProject,
        string templateFile,
        ProductNativeBuildPlan nativePlan,
        INativeToolchainProvider toolchainProvider,
        BuildHostDescriptor host
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(target.value);
        ArgumentException.ThrowIfNullOrWhiteSpace(runtimeIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(nativePlatform);
        ArgumentException.ThrowIfNullOrWhiteSpace(nativeExtension);
        ArgumentNullException.ThrowIfNull(validator);
        ArgumentException.ThrowIfNullOrWhiteSpace(productProject);
        ArgumentException.ThrowIfNullOrWhiteSpace(templateFile);
        ArgumentNullException.ThrowIfNull(nativePlan);
        ArgumentNullException.ThrowIfNull(toolchainProvider);
        ArgumentNullException.ThrowIfNull(host);
        m_productProject = productProject;
        m_templateFile = templateFile;
        m_nativePlan = nativePlan;
        m_toolchainProvider = toolchainProvider;
        m_host = host;
        m_target = target;
        m_validator = validator;
        m_runtimeIdentifier = runtimeIdentifier;
        m_nativePlatform = nativePlatform;
        m_nativeExtension = nativeExtension;
    }

    /// <summary>
    /// Gets the platform identity whose closure this source prepares.
    /// </summary>
    public BuildTargetId target => m_target;

    /// <summary>
    /// Resolves tools and validates required source inputs before any staging exists.
    /// </summary>
    /// <param name="context">
    /// The read-only source checkout and explicitly selected managed host.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels discovery before publication is permitted.
    /// </param>
    /// <returns>
    /// A frozen plan borrowing this preparation definition, or a propagated preflight failure.
    /// </returns>
    public async ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
        PlayerSupportPackPlanningContext context,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        string project = Inno.Core.IO.PathBoundary.Resolve(context.engineRoot, m_productProject);
        string template = Inno.Core.IO.PathBoundary.Resolve(context.engineRoot, m_templateFile);
        if (!File.Exists(template))
            throw new FileNotFoundException("The Player publication template is absent.", template);
        var native = new NativeBuildContext(context.engineRoot, ToolchainLayout.C_RELEASE_CONFIGURATION);
        NativeToolchainSelection selection = await m_toolchainProvider.ResolveAsync(
            native, m_host, m_nativePlatform, cancellationToken).ConfigureAwait(false);
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(
            context.dotnetHost, project, cancellationToken).ConfigureAwait(false);
        return new PreparationPlan(this, context.engineRoot, project, sdk, native.WithToolchain(selection));
    }

    private async ValueTask ExecuteAsync(
        PlayerSupportPackBuildContext context,
        string project,
        NativeBuildContext native,
        CancellationToken cancellationToken
    ) {
        IReadOnlyList<NativeBuildProduct> products = await m_nativePlan.BuildAsync(native, cancellationToken).ConfigureAwait(false);
        await ToolchainEnvironment.RunAsync(context.sdk.hostPath,
            [context.sdk.cliPath, "build", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "--configuration", "Release",
                "--runtime", m_runtimeIdentifier, "--nologo", "-p:DebugType=None", "-p:DebugSymbols=false",
                "-p:InnoNativeTarget=" + m_target.value, "-p:InnoPrepareProductNative=false"],
            context.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(context.sdk.hostPath)).ConfigureAwait(false);
        string buildOutput = (await ToolchainEnvironment.CaptureOutputAsync(context.sdk.hostPath,
            [context.sdk.cliPath, "msbuild", project, "-getProperty:TargetDir", "-p:Configuration=Release", "-p:RuntimeIdentifier=" + m_runtimeIdentifier,
                "-p:InnoNativeTarget=" + m_target.value, "-p:DesignTimeBuild=true", "-nologo"],
            context.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(context.sdk.hostPath)).ConfigureAwait(false)).Trim();
        PlayerSupportPackFiles.CopyReferences(
            buildOutput,
            Path.Combine(context.stagingDirectory, "References"), Path.GetFileNameWithoutExtension(project));
        CopyNativeRuntime(products, context.stagingDirectory);
        string link = Path.Combine(context.stagingDirectory, "PlayerLink");
        Directory.CreateDirectory(link);
        File.Copy(Inno.Core.IO.PathBoundary.Resolve(context.engineRoot, m_templateFile), Path.Combine(link, "Player.csproj"));
        await PlayerSupportPackFiles.CopyPlayerSourcesAsync(
            context, project, link, cancellationToken).ConfigureAwait(false);
        PlayerSupportPackFiles.CopyCompositionInputs(context.engineRoot, link);
        string references = Path.Combine(link, "References");
        Directory.CreateDirectory(references);
        foreach (string reference in Directory.EnumerateFiles(Path.Combine(context.stagingDirectory, "References"), "*.dll"))
            File.Copy(reference, Path.Combine(references, Path.GetFileName(reference)));
        string runtime = Path.Combine(buildOutput, "BGCS.Runtime.dll");
        File.Copy(runtime, Path.Combine(references, "BGCS.Runtime.dll"));
        await CopyNativeToTemplateAsync(context.stagingDirectory, link, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Validates the prepared platform inputs before atomic installation.
    /// </summary>
    /// <param name="directory">
    /// The isolated target pack directory.
    /// </param>
    public void Validate(string directory) => m_validator.Validate(directory);

    private void CopyNativeRuntime(
        IReadOnlyList<NativeBuildProduct> products,
        string staging
    ) {
        string destination = Path.Combine(staging, "native");
        foreach (NativeBuildProduct product in products)
        {
            if (product.targetId != m_nativePlatform)
                throw new InvalidDataException($"Native product '{product.component}' targets '{product.targetId}', not '{m_nativePlatform}'.");
            if (!product.files.Any(file => string.Equals(Path.GetExtension(file), m_nativeExtension, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidDataException(
                    $"Native runtime product '{product.component}' and '{m_target}' is empty.");
            }
        }
        foreach ((string relative, string source) in m_nativePlan.CreateDeploymentFiles(products))
        {
            string output = Path.Combine(destination, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(output)!);
            File.Copy(source, output);
        }
    }

    private static async ValueTask CopyNativeToTemplateAsync(
        string staging,
        string playerDirectory,
        CancellationToken cancellationToken
    ) {
        string nativeRoot = Path.Combine(staging, "native");
        foreach (string input in Directory.EnumerateFiles(nativeRoot, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string destination = Path.Combine(playerDirectory, "native", Path.GetRelativePath(nativeRoot, input));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            await using FileStream source = File.OpenRead(input);
            await using FileStream output = new(destination, FileMode.CreateNew);
            await source.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
            if (!OperatingSystem.IsWindows())
                File.SetUnixFileMode(destination, File.GetUnixFileMode(input));
        }
    }
    private sealed class PreparationPlan(
        FilePlayerSupportPackPreparation owner,
        string engineRoot,
        string project,
        DotNetSdkDescriptor sdk,
        NativeBuildContext native
    ) : PlayerSupportPackPlan(owner.target) {
        /// <inheritdoc />
        public override ValueTask PrepareAsync(
            string stagingDirectory,
            CancellationToken cancellationToken
        ) => owner.ExecuteAsync(new PlayerSupportPackBuildContext(engineRoot, stagingDirectory, sdk),
            project, native, cancellationToken);
    }

}
