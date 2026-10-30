using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ImGuiEditorRuntime = Inno.Editor.ImGui.ImGuiEditorRuntime;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BGCS.Runtime;
using Inno.Build.Managed;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.Exporting;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using Xunit;
using NativeGui = Inno.Native.ImGui.ImGui;
using EditorUiSession = Inno.Editor.ImGui.EditorUiSession;

namespace Inno.Build.Tests;

public sealed partial class BuildPipelineTests
{
    [Fact]
    [Trait("ExternalDependency", "NativeProduct")]
    public unsafe void ExportFormBalancesItsTablesWhenARegisteredTargetLabelFails()
    {
        var target = new ExportLabelTarget();
        var pipeline = new BuildPipeline(m_assets, m_plugins, m_settings, m_engine.serialization,
            m_engine.generations, m_compiler, m_supportPackRoot,
            [new GameBuildTargetBinding(target, target)], CreateManagedDeployments());
        var defaults = BuildSettings.CreateDefault("Test Game", "Scenes/Startup.iscene", target.id);
        var settings = new BuildSettingsStore(System.IO.Path.Combine(m_projectRoot, "Settings.Build.inno"),
            m_engine.serialization, defaults);
        m_engine.modules.Register("Tests.Exporting", [typeof(EditorExportService).Assembly,
            Assembly.Load(new AssemblyName("Inno.Editor.Workbench"))]);
        var context = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl"));
        var logs = new ExportLogSink();
        ImGuiNativeContext* native = NativeGui.Raw.CreateContext(null);
        try
        {
            m_engine.logs.RegisterSink(logs);
            using var session = new EditorUiSession();
            using IDisposable scope = session.Enter();
            using var runtime = new EditorInteractionRuntime(context, m_engine.types, m_engine.logs,
                [pipeline, settings, m_settings, session]);
            runtime.Start();
            Assert.True(runtime.interactions.For("editor/main-menu").Execute("export/game"));
            EditorModalExtension modal = Assert.Single(runtime.modals, item => item.id == "export.game");
            ImGuiIO* io = NativeGui.Raw.GetIO();
            io->DisplaySize = new ImVec2C { X = 1200f, Y = 900f };
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            Assert.True(DrawExportFrame(context, modal));
            target.reject = true;
            Assert.False(DrawExportFrame(context, modal));
            Assert.False(modal.TryGetPresentation(out _));
            m_engine.logs.Flush();
            Assert.Contains(logs.messages, message => message.Contains("Export target label failed", StringComparison.Ordinal));
            target.reject = false;
            m_engine.types.Rebuild();
            runtime.Update(new EditorFrame(1f / 60f, 1f, true));
            Assert.True(runtime.interactions.For("editor/main-menu").Execute("export/game"));
            modal = Assert.Single(runtime.modals, item => item.id == "export.game");
            Assert.True(DrawExportFrame(context, modal));
            Assert.True(DrawExportFrame(context, modal));
        }
        finally
        {
            m_engine.logs.UnregisterSink(logs);
            NativeGui.Raw.DestroyContext(native);
        }
    }

    [Theory]
    [InlineData(1280, 720, 1f)]
    [InlineData(320, 480, 2f)]
    [Trait("ExternalDependency", "NativeProduct")]
    public unsafe void ExportPresentationUsesTheDeclaredWidthAndRealOverflow(
        int width,
        int height,
        float dpi
    ) {
        var defaults = BuildSettings.CreateDefault("Test Game", "Scenes/Startup.iscene", new BuildTargetId("macos-arm64"));
        var settings = new BuildSettingsStore(Path.Combine(m_projectRoot, "Settings.Build.inno"), m_engine.serialization, defaults);
        m_engine.modules.Register("Tests.ExportPresentation", [typeof(EditorExportService).Assembly,
            Assembly.Load(new AssemblyName("Inno.Editor.Workbench"))]);
        var native = NativeGui.Raw.CreateContext(null);
        try
        {
            var io = NativeGui.Raw.GetIO();
            io->DisplaySize = new ImVec2C { X = width, Y = height };
            io->DeltaTime = 0.2f;
            io->IniFilename = null;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            using var runtime = new ImGuiEditorRuntime(new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl")),
                m_engine.types, m_engine.logs, [m_pipeline, settings, m_settings]);
            runtime.Start();
            Assert.True(runtime.interactions.For("editor/main-menu").Execute("export/plugin"));
            NativeGui.Raw.GetStyle()->FontScaleDpi = dpi;
            for (int frame = 0; frame < 4; frame++)
            {
                runtime.PrepareFrame();
                NativeGui.NewFrame();
                runtime.Draw();
                NativeGui.Render();
            }
            using var title = BGCS.Runtime.Utf8Codec.EncodeNullTerminated("Export as Plugin##export.plugin", allowsNull: false);
            var window = NativeGui.Raw.FindWindowByName((sbyte*)title.pointer);
            Assert.True(window != null && window->WasActive != 0);
            Assert.True(window->Size.X <= width && window->Size.Y <= height);
            if (width == 1280)
            {
                Assert.Equal(680f, window->Size.X);
                Assert.InRange(window->ScrollMax.X, 0f, 0.01f);
                Assert.InRange(window->ScrollMax.Y, 0f, 0.01f);
            }
            else
                Assert.True(window->ScrollMax.Y > 0f);
            Assert.True(runtime.interactions.TryGetModule<EditorExportService>(out var exports));
            Assert.False(exports!.progress.isBusy);
        }
        finally
        {
            NativeGui.Raw.DestroyContext(native);
        }
    }

    [Fact]
    public void ExportPresentationKeepsUnknownProgressAndRejectsStaleCancellation()
    {
        var defaults = BuildSettings.CreateDefault("Test Game", "Scenes/Startup.iscene", new BuildTargetId("macos-arm64"));
        var settings = new BuildSettingsStore(Path.Combine(m_projectRoot, "Settings.Build.inno"), m_engine.serialization, defaults);
        m_engine.modules.Register("Tests.ExportProgress", [typeof(EditorExportService).Assembly]);
        var context = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl"));
        using var runtime = new Inno.Editor.Interactions.EditorInteractionRuntime(context, m_engine.types, m_engine.logs,
            [m_pipeline, settings, m_settings]);
        runtime.Start();
        Assert.True(runtime.interactions.TryGetModule<EditorExportService>(out var exports));
        exports!.StartGame(new GameBuildRequest { profile = CreateProfile(new BuildTargetId("macos-arm64")),
            outputDirectory = Path.Combine(m_root, "ProgressOutput") });
        var observed = exports.progress;
        Assert.Null(observed.fraction);
        Assert.True(observed.isBusy);
        exports.Cancel(observed.operationId - 1);
        Assert.False(exports.progress.cancellationRequested);
        exports.Cancel(observed.operationId);
        Assert.True(exports.progress.cancellationRequested);
        Assert.False(observed.cancellationRequested);
        exports.Stop(context);
        Assert.False(exports.progress.isBusy);
    }

    private static unsafe bool DrawExportFrame(
        EditorContext context,
        EditorModalExtension modal
    ) {
        NativeGui.NewFrame();
        NativeGui.SetNextWindowPos(new Vector2(20f), (int)ImGuiCond.Always);
        NativeGui.SetNextWindowSize(new Vector2(1000f, 800f), (int)ImGuiCond.Always);
        using NativeStringLease title = Utf8Codec.EncodeNullTerminated("Export exception ownership", allowsNull: false);
        _ = NativeGui.Raw.Begin((sbyte*)title.pointer, null, (int)ImGuiWindowFlags.NoSavedSettings);
        try
        {
            bool drawn = modal.Draw(context);
            bool leaked = NativeGui.Raw.GetCurrentTable() != null;
            // Retire only a table opened by this fixture's modal before reporting a failed regression.
            if (leaked)
                NativeGui.EndTable();
            Assert.False(leaked, "A modal failure left its own native Table active.");
            return drawn;
        }
        finally
        {
            NativeGui.End();
            NativeGui.Render();
        }
    }

    private sealed class ExportLogSink : ILogSink
    {
        internal List<string> messages { get; } = [];
        public void Receive(LogEntry entry) => messages.Add(entry.message);
    }

    private sealed class ExportLabelTarget : IGameBuildTarget, IGameContentCompiler
    {
        internal bool reject { get; set; }
        public BuildTargetId id => new("fixture-export");
        public BuildTargetId target => id;
        public ManagedDeploymentId defaultManagedDeployment => ManagedDeploymentId.coreClr;
        public string runtimeIdentifier => "fixture";
        public string displayName => reject ? throw new InvalidOperationException("Export target label failed.") : "Fixture target";
        public void Validate(string directory) => throw new InvalidOperationException("This presentation fixture cannot validate products.");

        public ValueTask<string> PackageAsync(
            GameBuildPackageContext context,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("This presentation fixture cannot package products.");

        public ValueTask CompileAsync(
            GameBuildContentContext context,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("This presentation fixture cannot compile content.");
    }
}
