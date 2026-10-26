using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Composition;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.BrowserWasm32;

/// <summary>
/// Prepares Browser runtime sources and one frozen native closure for isolated publication.
/// </summary>
public sealed class BrowserWasm32PlayerSupportPackSource : IPlayerSupportPackSource
{
    private readonly BuildHostDescriptor m_host;
    private readonly BuildTargetId m_toolsTarget;
    private readonly NativeProductContribution m_nativeProduct;
    private readonly INativeBindingGenerator m_bindingGenerator;
    private readonly IPlayerSupportPackValidator m_validator;

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
    /// <param name="supportPackValidator">
    /// The borrowed platform and selected static-closure validator.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The host or native plan is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The offline tool target is unassigned.
    /// </exception>
    public BrowserWasm32PlayerSupportPackSource(
        BuildHostDescriptor host,
        BuildTargetId toolsTarget,
        NativeProductContribution nativePlan,
        INativeBindingGenerator bindingGenerator,
        IPlayerSupportPackValidator supportPackValidator
    ) {
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(nativePlan);
        ArgumentException.ThrowIfNullOrWhiteSpace(toolsTarget.value);
        ArgumentNullException.ThrowIfNull(bindingGenerator);
        m_bindingGenerator = bindingGenerator;
        m_host = host;
        m_toolsTarget = toolsTarget;
        m_nativeProduct = nativePlan;
        m_validator = supportPackValidator ?? throw new ArgumentNullException(nameof(supportPackValidator));
    }

    /// <inheritdoc />
    public BuildTargetId target => Inno.Build.BrowserWasm32.BrowserWasm32BuildModule.target.id;

    /// <inheritdoc />
    public async ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
        PlayerSupportPackPlanningContext context,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        string project = Path.Combine(context.engineRoot, "platforms", "BrowserWasm32", "player",
            "Inno.Player.BrowserWasm32", "Inno.Player.BrowserWasm32.csproj");
        string template = Path.Combine(BrowserWasm32NativeProductExecutor.componentDescriptor.GetToolchainRoot(context.engineRoot),
            "Templates", "BrowserWasm32Player.project.xml");
        if (!File.Exists(template))
            throw new FileNotFoundException("The Browser publication template is absent.", template);
        var nativeContext = new NativeBuildContext(context.engineRoot, "release").WithBindingGenerator(m_bindingGenerator);
        NativeToolchainSelection tools = await new EmscriptenNativeToolchainProvider(context.dotnetHost)
            .ResolveAsync(nativeContext, m_host, target.value, cancellationToken).ConfigureAwait(false);
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(
            tools.ResolveExecutable("dotnet"), project, cancellationToken).ConfigureAwait(false);
        nativeContext = nativeContext.WithToolchain(tools);
        var properties = new Dictionary<string, string>
        {
            ["Configuration"] = "Release", ["InnoNativeTarget"] = target.value, ["InnoToolTarget"] = m_toolsTarget.value,
            ["InnoShaderToolSourceRoot"] = context.engineRoot,
            ["DebugType"] = "None", ["DebugSymbols"] = "false"
        };
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "restore", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "--nologo", "-v:quiet",
                .. properties.Select(static pair => "-p:" + pair.Key + "=" + pair.Value)],
            context.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(sdk.hostPath, tools.environment)).ConfigureAwait(false);
        string owner = BrowserWasm32NativeProductExecutor.componentDescriptor.GetToolchainRoot(context.engineRoot);
        PlayerSupportPackPreparationIdentity identity = await PlayerSupportPackPreparationIdentity.CaptureAsync(
            nativeContext, m_nativeProduct.plan, sdk, project, properties,
            [template, Path.Combine(Path.GetDirectoryName(project)!, "wwwroot", "index.html"),
                Path.Combine(Path.GetDirectoryName(project)!, "wwwroot", "main.js"),
                Path.Combine(owner, "Native", "BrowserWasm32NativeProductExecutor.cs"),
                Path.Combine(owner, "Native", "CMakeLists.txt"), Path.Combine(owner, "Native", "wasm_sjlj_shim.c"),
                Path.Combine(owner, "SupportPacks", "BrowserWasm32PlayerSupportPackSource.cs"),
                Path.Combine(owner, "SupportPacks", "BrowserWasm32SupportPackValidator.cs")], cancellationToken).ConfigureAwait(false);
        return new PreparationPlan(this, context.engineRoot, project, sdk, nativeContext, identity);
    }

    private async ValueTask ExecuteAsync(
        PlayerSupportPackBuildContext context,
        string project,
        NativeBuildContext nativeContext,
        PlayerSupportPackPreparationIdentity identity,
        CancellationToken cancellationToken
    ) {
        NativeToolchainSelection tools = nativeContext.RequireToolchain();
        using ProductNativeBuildResult artifacts = await m_nativeProduct.BuildAsync(
            nativeContext, cancellationToken).ConfigureAwait(false);
        using ManagedBuildExecution execution = await identity.CreateExecutionAsync(cancellationToken).ConfigureAwait(false);
        context = new PlayerSupportPackBuildContext(execution.engineRoot, context.stagingDirectory, context.sdk);
        project = execution.project;
        string profileRoot = Path.Combine(context.engineRoot, "artifacts", "managed", "browser-references", artifacts[0].fingerprint);
        using FileLease ownership = await FileLease.AcquireAsync(
            profileRoot + ".lock", Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        string[] properties = ["-p:Configuration=Release", "-p:InnoNativeTarget=" + target.value,
            "-p:InnoToolTarget=" + m_toolsTarget.value, "-p:InnoNativeBuildFingerprint=" + artifacts[0].fingerprint,
            "-p:InnoShaderToolSourceRoot=" + nativeContext.engineRoot,
            "-p:InnoNativeBindingSelection=" + Path.Combine(artifacts.aggregateDirectory!, "Metadata", "BindingSelection.props"),
            "-p:DebugType=None", "-p:DebugSymbols=false"];
        await ToolchainEnvironment.RunAsync(context.sdk.hostPath,
            [context.sdk.cliPath, "build", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "--nologo", .. properties],
            context.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(context.sdk.hostPath, tools.environment)).ConfigureAwait(false);
        string buildOutput = (await ToolchainEnvironment.CaptureOutputAsync(context.sdk.hostPath,
            [context.sdk.cliPath, "msbuild", project, "-getProperty:TargetDir", "-p:DesignTimeBuild=true", "-nologo", .. properties],
            context.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(context.sdk.hostPath, tools.environment)).ConfigureAwait(false)).Trim();
        PlayerSupportPackFiles.CopyReferences(buildOutput, Path.Combine(context.stagingDirectory, "References"), "Inno.Player.BrowserWasm32");
        CopyLinkTemplate(context.engineRoot, context.stagingDirectory, artifacts, project);
        await PlayerSupportPackFiles.CopyPlayerSourcesAsync(context, project,
            Path.Combine(context.stagingDirectory, "PlayerLink"), cancellationToken, tools.environment).ConfigureAwait(false);
        await execution.VerifyAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void Validate(string directory) => m_validator.Validate(directory);

    private static void CopyLinkTemplate(
        string engineRoot,
        string staging,
        ProductNativeBuildResult artifacts,
        string productProject
    ) {
        string template = Path.Combine(staging, "PlayerLink");
        string native = Path.Combine(template, "Native");
        string references = Path.Combine(template, "References");
        string webRoot = Path.Combine(template, "wwwroot");
        Directory.CreateDirectory(native);
        Directory.CreateDirectory(references);
        Directory.CreateDirectory(webRoot);
        string owner = BrowserWasm32NativeProductExecutor.componentDescriptor.GetToolchainRoot(engineRoot);
        CopyRequired(Path.Combine(owner, "Templates", "BrowserWasm32Player.project.xml"), Path.Combine(template, "Player.csproj"));
        PlayerSupportPackFiles.CopyCompositionInputs(engineRoot, template);
        foreach (string asset in new[] { "index.html", "main.js" })
            CopyRequired(Path.Combine(Path.GetDirectoryName(productProject)!, "wwwroot", asset), Path.Combine(webRoot, asset));
        foreach (string archive in Directory.EnumerateFiles(artifacts.aggregateDirectory!, "*.a", SearchOption.AllDirectories))
        {
            string destination = PathBoundary.Resolve(native, Path.GetRelativePath(artifacts.aggregateDirectory!, archive));
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
        BrowserWasm32PlayerSupportPackSource owner,
        string engineRoot,
        string project,
        DotNetSdkDescriptor sdk,
        NativeBuildContext native,
        PlayerSupportPackPreparationIdentity identity
    ) : PlayerSupportPackPlan(owner.target, identity.fingerprint) {
        /// <inheritdoc />
        public override ValueTask VerifyInputsAsync(CancellationToken cancellationToken) => identity.VerifyAsync(cancellationToken);

        /// <inheritdoc />
        public override ValueTask PrepareAsync(
            string stagingDirectory,
            CancellationToken cancellationToken
        ) => owner.ExecuteAsync(new PlayerSupportPackBuildContext(engineRoot, stagingDirectory, sdk),
            project, native, identity, cancellationToken);
    }

}
