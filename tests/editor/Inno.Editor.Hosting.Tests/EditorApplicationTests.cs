using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter;
using Inno.Adapter.Authoring.Default;
using Inno.Adapter.Default;
using Inno.Adapter.Presentation;
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.FileSystem;
using Inno.Build;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Input;
using Inno.Editor.Core;
using Inno.Editor.Hosting;
using Inno.Shell;
using Xunit;

namespace Inno.Editor.Hosting.Tests;

public sealed class EditorApplicationTests
{
    [Fact]
    public async Task CancellationBeforeStartupDoesNotCreateProjectOrAcquireProductResources()
    {
        EditorLaunchOptions options = CreateOptions();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => EditorApplication.RunAsync(options, cancellation.Token));
        Assert.False(Directory.Exists(options.projectDirectory));
    }

    [Fact]
    public async Task MissingBackendSelectionFailsBeforeProjectIoOrNativeInitialization()
    {
        EditorLaunchOptions options = CreateOptions(selection: new AdapterSelection());

        await Assert.ThrowsAsync<NotSupportedException>(() => EditorApplication.RunAsync(options));
        Assert.False(Directory.Exists(options.projectDirectory));
    }

    [Fact]
    public async Task UnregisteredDefaultTargetIsRejectedBeforeProductOwnershipBegins()
    {
        EditorLaunchOptions options = CreateOptions(target: new BuildTargetId("fixture-unregistered"));

        await Assert.ThrowsAsync<ArgumentException>(() => EditorApplication.RunAsync(options));
        Assert.False(Directory.Exists(options.projectDirectory));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidFrameLimitDoesNotCreateProject(int limit)
    {
        EditorLaunchOptions options = CreateOptions(frameLimit: limit);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => EditorApplication.RunAsync(options));
        Assert.False(Directory.Exists(options.projectDirectory));
    }

    [Fact]
    public async Task MissingKeyboardPolicyCannotSilentlyUseTheExecutingOperatingSystem()
    {
        EditorLaunchOptions options = CreateOptions(keyboard: false);

        await Assert.ThrowsAsync<ArgumentNullException>(() => EditorApplication.RunAsync(options));
        Assert.False(Directory.Exists(options.projectDirectory));
    }

    private static EditorLaunchOptions CreateOptions(
        AdapterSelection? selection = null,
        BuildTargetId? target = null,
        int? frameLimit = null,
        bool keyboard = true
    ) {
        string project = Path.Combine(Path.GetTempPath(), "InnoHostingContract", Guid.NewGuid().ToString("N"));
        var build = new BuildCompositionContext(
            "dotnet",
            AppContext.BaseDirectory,
            new BuildHostDescriptor("Windows", "x64"),
            BuildTargetId.windowsX64,
            new NativeBindingGenerator("dotnet"));
        StandardBuildDistribution standard = StandardBuildDistribution.Create(build);
        var adapters = new DefaultAuthoringAdapterCatalog(new DefaultAdapterCatalogOptions
        {
            platform = new Inno.Adapter.Platform.PlatformBackendCatalog([new Inno.Adapter.Platform.Sdl3.Sdl3PlatformBackendProvider(new Inno.Integration.Windows.Sdl3.WindowsSdl3HostIntegration())]),
            rendering = new Inno.Adapter.Rendering.RenderingBackendCatalog([new Inno.Adapter.Rendering.Bgfx.BgfxRenderingBackendProvider(new Inno.Integration.Windows.Bgfx.Runtime.WindowsBgfxSurfaceIntegration())]),
            storage = new StorageBackendCatalog([new FileSystemStorageBackendProvider(Path.Combine(project, "Data"))])
        }, [new BgfxAuthoringProvider(standard.ResolveShaderTarget(BuildTargetId.windowsX64))],
            [new Inno.Adapter.Presentation.ImGui.ImGuiPresentationProvider(new Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions(false, 1))]);
        return new EditorLaunchOptions
        {
            projectDirectory = project,
            keyboard = keyboard ? new EditorKeyboardPolicy(KeyModifier.Control, "Super") : null!,
            adapterCatalog = adapters,
            shell = new ShellOptions { adapters = selection ?? StandardAdapterSelection.Create(StorageBackendId.fileSystem) },
            presentation = PresentationBackendId.imGui,
            frameDriver = new PollingShellFrameDriver(),
            distribution = standard.build,
            defaultBuildTarget = target ?? BuildTargetId.windowsX64,
            buildContext = build,
            supportPackRoot = Path.Combine(project, "SupportPacks"),
            createEngineHost = () => throw new InvalidOperationException("Preflight must not acquire an engine."),
            createScriptModuleSource = _ => throw new InvalidOperationException("Preflight must not acquire a script source."),
            smokeFrameLimit = frameLimit
        };
    }
}
