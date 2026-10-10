using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Browser;

/// <summary>
/// Prepares Browser runtime sources and one frozen native closure for isolated publication.
/// </summary>
public sealed class BrowserPlayerSupportPackSource : IPlayerSupportPackSource
{
    private readonly BuildHostDescriptor m_host;
    private readonly BuildTargetId m_toolsTarget;
    private readonly ProductNativeBuildPlan m_nativePlan;
    private readonly INativeBindingGenerator m_bindingGenerator;

    /// <summary>
    /// Captures independent Browser product and offline compiler selections.
    /// </summary>
    /// <param name="host">
    /// The execution machine permitted to run the selected SDK.
    /// </param>
    /// <param name="toolsTarget">
    /// The explicit target of offline tools used while preparing the product.
    /// </param>
    /// <param name="nativePlan">
    /// The borrowed immutable Player component closure.
    /// </param>
    /// <param name="bindingGenerator">
    /// The borrowed generation service used by the Browser component batch.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The host or native plan is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The offline tool target is unassigned.
    /// </exception>
    public BrowserPlayerSupportPackSource(
        BuildHostDescriptor host,
        BuildTargetId toolsTarget,
        ProductNativeBuildPlan nativePlan,
        INativeBindingGenerator bindingGenerator
    ) {
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(nativePlan);
        ArgumentException.ThrowIfNullOrWhiteSpace(toolsTarget.value);
        ArgumentNullException.ThrowIfNull(bindingGenerator);
        m_bindingGenerator = bindingGenerator;
        m_host = host;
        m_toolsTarget = toolsTarget;
        m_nativePlan = nativePlan;
    }

    /// <inheritdoc />
    public BuildTargetId target => BuildTargetId.browserWasm;

    /// <inheritdoc />
    public async ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
        PlayerSupportPackPlanningContext context,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        string project = Path.Combine(context.engineRoot, "platforms", "Browser", "player",
            "Inno.Player.Browser", "Inno.Player.Browser.csproj");
        string template = Path.Combine(BrowserToolchain.componentDescriptor.GetToolchainRoot(context.engineRoot),
            "Templates", "BrowserPlayer.project.xml");
        if (!File.Exists(template))
            throw new FileNotFoundException("The Browser publication template is absent.", template);
        var nativeContext = new NativeBuildContext(context.engineRoot, "release").WithBindingGenerator(m_bindingGenerator);
        NativeToolchainSelection tools = await new EmscriptenNativeToolchainProvider(context.dotnetHost)
            .ResolveAsync(nativeContext, m_host, target.value, cancellationToken).ConfigureAwait(false);
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(
            tools.ResolveExecutable("dotnet"), project, cancellationToken).ConfigureAwait(false);
        return new PreparationPlan(this, context.engineRoot, project, sdk, nativeContext.WithToolchain(tools));
    }

    private async ValueTask ExecuteAsync(
        PlayerSupportPackBuildContext context,
        string project,
        NativeBuildContext nativeContext,
        CancellationToken cancellationToken
    ) {
        NativeToolchainSelection tools = nativeContext.RequireToolchain();
        BrowserNativeArtifacts artifacts = await BrowserToolchain.BuildAsync(
            nativeContext, m_nativePlan, cancellationToken).ConfigureAwait(false);
        string profileRoot = Path.Combine(context.engineRoot, "artifacts", "managed", "browser-references", artifacts.fingerprint);
        using FileLease ownership = await FileLease.AcquireAsync(
            profileRoot + ".lock", Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        string[] properties = ["-p:Configuration=Release", "-p:InnoNativeTarget=" + target.value,
            "-p:InnoToolTarget=" + m_toolsTarget.value, "-p:InnoNativeBuildFingerprint=" + artifacts.fingerprint,
            "-p:InnoNativeBindingSelection=" + artifacts.bindingSelectionPath,
            "-p:DebugType=None", "-p:DebugSymbols=false"];
        await ToolchainEnvironment.RunAsync(context.sdk.hostPath,
            [context.sdk.cliPath, "build", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "--nologo", .. properties],
            context.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(context.sdk.hostPath, tools.environment)).ConfigureAwait(false);
        string buildOutput = (await ToolchainEnvironment.CaptureOutputAsync(context.sdk.hostPath,
            [context.sdk.cliPath, "msbuild", project, "-getProperty:TargetDir", "-p:DesignTimeBuild=true", "-nologo", .. properties],
            context.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(context.sdk.hostPath, tools.environment)).ConfigureAwait(false)).Trim();
        PlayerSupportPackFiles.CopyReferences(buildOutput, Path.Combine(context.stagingDirectory, "References"), "Inno.Player.Browser");
        CopyLinkTemplate(context.engineRoot, context.stagingDirectory, artifacts, project);
        await PlayerSupportPackFiles.CopyPlayerSourcesAsync(context, project,
            Path.Combine(context.stagingDirectory, "PlayerLink"), cancellationToken, tools.environment).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void Validate(string directory) => new BrowserSupportPackValidator(m_nativePlan).Validate(directory);

    private static void CopyLinkTemplate(
        string engineRoot,
        string staging,
        BrowserNativeArtifacts artifacts,
        string productProject
    ) {
        string template = Path.Combine(staging, "PlayerLink");
        string native = Path.Combine(template, "Native");
        string references = Path.Combine(template, "References");
        string webRoot = Path.Combine(template, "wwwroot");
        Directory.CreateDirectory(native);
        Directory.CreateDirectory(references);
        Directory.CreateDirectory(webRoot);
        string owner = BrowserToolchain.componentDescriptor.GetToolchainRoot(engineRoot);
        CopyRequired(Path.Combine(owner, "Templates", "BrowserPlayer.project.xml"), Path.Combine(template, "Player.csproj"));
        PlayerSupportPackFiles.CopyCompositionInputs(engineRoot, template);
        foreach (string asset in new[] { "index.html", "main.js" })
            CopyRequired(Path.Combine(Path.GetDirectoryName(productProject)!, "wwwroot", asset), Path.Combine(webRoot, asset));
        foreach (string archive in Directory.EnumerateFiles(artifacts.directory, "*.a", SearchOption.AllDirectories))
        {
            string destination = PathBoundary.Resolve(native, Path.GetRelativePath(artifacts.directory, archive));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            CopyRequired(archive, destination);
        }
        CopyRequired(Path.Combine(owner, "Native", "wasm_sjlj_shim.c"), Path.Combine(native, "wasm_sjlj_shim.c"));
        foreach (string file in Directory.EnumerateFiles(Path.Combine(staging, "References"), "*.dll"))
            CopyRequired(file, Path.Combine(references, Path.GetFileName(file)));
    }

    private static void CopyRequired(
        string source,
        string destination
    ) {
        if (!File.Exists(source))
            throw new FileNotFoundException($"Browser Support Pack input is missing: '{source}'.", source);
        File.Copy(source, destination, overwrite: true);
    }
    private sealed class PreparationPlan(
        BrowserPlayerSupportPackSource owner,
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
