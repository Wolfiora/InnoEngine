using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using BGCS.Runtime;
using Xunit;
using Inno.Adapter.Content.FileSystem;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.MacOSArm64;
using Inno.Build.Managed;
using Inno.Build.WindowsX64;
using Inno.Content;
using Inno.Core.Execution;
using Inno.Core.IO;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Editor.Core;
using Inno.Editor.Exporting;
using Inno.Editor.Interactions;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scripting.Compiler;
using Inno.Testing.Build;

using GuiSession = Inno.Editor.ImGui.EditorUISession;
using NativeGui = Inno.Native.ImGui.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;
using UIRuntime = Inno.Editor.ImGui.ImGuiEditorRuntime;

namespace Inno.Editor.Exporting.Native.Tests;

public sealed class ExportPresentationTests : BuildPipelineFixture
{
    [Fact]
    [Trait("ExternalDependency", "NativeProduct")]
    public unsafe void EditorShutdownDrainsSupportPackCleanupBeforeReleasingExportServices()
    {
        SaveStartupScene();
        Directory.Delete(Path.Combine(m_supportPackRoot, new BuildTargetID("macos-arm64").value), recursive: true);
        var provisioner = new DrainingSupportPackProvisioner();
        BuildPipeline pipeline = CreatePipeline(provisioner);
        BuildSettings defaults = BuildSettings.CreateDefault("Test Game", "Scenes/Startup.iscene", new BuildTargetID("macos-arm64"));
        var buildSettings = new BuildSettingsStore(
            Path.Combine(m_projectRoot, "Settings.Build.inno"),
            m_engine.serialization,
            defaults
        );
        m_engine.modules.Register("Tests.Exporting", [
            typeof(EditorExportService).Assembly,
            Assembly.Load(new AssemblyName("Inno.Editor.Workbench"))
        ]);
        var context = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super"));
        using var runtime = new EditorInteractionRuntime(
            context,
            m_engine.types,
            m_engine.logs,
            [pipeline, buildSettings, m_settings]
        );
        runtime.Start();
        Assert.True(runtime.interactions.For("editor/main-menu").Execute("export/game"));
        EditorModalExtension modal = Assert.Single(runtime.modals.Where(static item => item.id == "export.game"));
        EditorModalExtension progress = Assert.Single(runtime.modals.Where(static item => item.id == "export.game.progress"));
        ImGuiNativeContext* imgui = NativeImGui.Raw.CreateContext(null);
        try
        {
            NativeImGui.Raw.GetIO()->IniFilename = null;
            using var uiSession = new GuiSession();
            using IDisposable uiScope = uiSession.Enter();
            ImGuiIO* io = NativeImGui.Raw.GetIO();
            io->DisplaySize = new ImVec2C { X = 1200f, Y = 900f };
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            using NativeStringLease title = Utf8Codec.EncodeNullTerminated("Export lifecycle verification", allowsNull: false);
            Vector2 exportCenter = Vector2.Zero;
            for (int frame = 0; frame < 4; frame++)
            {
                if (frame == 2)
                {
                    NativeImGui.Raw.AddMousePosEvent(io, exportCenter.X, exportCenter.Y);
                    NativeImGui.Raw.AddMouseButtonEvent(io, 0, 1);
                }
                if (frame == 3)
                {
                    NativeImGui.Raw.AddMouseButtonEvent(io, 0, 0);
                }
                NativeImGui.NewFrame();
                NativeImGui.SetNextWindowPos(new Vector2(20f), (int)ImGuiCond.Always);
                NativeImGui.SetNextWindowSize(new Vector2(1000f, 800f), (int)ImGuiCond.Always);
                _ = NativeImGui.Raw.Begin((sbyte*)title.pointer, null, (int)ImGuiWindowFlags.NoSavedSettings);
                Assert.True(modal.Draw(context));
                exportCenter = (NativeImGui.GetItemRectMin() + NativeImGui.GetItemRectMax()) * 0.5f;
                NativeImGui.End();
                NativeImGui.Render();
            }
            Assert.True(provisioner.started);
            Assert.True(progress.TryGetPresentation(out EditorModalExtension.Presentation presentation));
            Assert.True(presentation.isVisible);
            _ = runtime.interactions.For("editor/main-menu").Execute("export/plugin");
            EditorModalExtension plugin = Assert.Single(runtime.modals.Where(static item => item.id == "export.plugin"));
            Assert.True(plugin.TryGetPresentation(out EditorModalExtension.Presentation pluginPresentation));
            Assert.False(pluginPresentation.isVisible);

            Assert.True(runtime.interactions.TryGetModule<EditorExportService>(out EditorExportService? module));
            Assert.NotNull(module);
            Assert.Equal("Inno.Editor.Exporting", module.GetType().Assembly.GetName().Name);
            Assert.Throws<RetirementPendingException>(() => module.Stop(context));
            Assert.True(provisioner.canceled.Task.IsCompletedSuccessfully);
            Assert.False(provisioner.completed);
            provisioner.cleanup.SetResult();
            Stopwatch retirement = Stopwatch.StartNew();
            while (true)
            {
                try
                {
                    runtime.Dispose();
                    break;
                }
                catch (Exception pending) when (RetirementPendingException.Find(pending) is not null)
                {
                    if (retirement.Elapsed >= TimeSpan.FromSeconds(5))
                    {
                        throw;
                    }
                    Thread.Sleep(1);
                }
            }
            Assert.True(provisioner.completed);
            m_engine.generations.EnsureReady("verify export retirement drained its services");
        }
        finally
        {
            provisioner.cleanup.TrySetResult();
            NativeImGui.Raw.DestroyContext(imgui);
        }
    }

    [Fact]
    [Trait("ExternalDependency", "NativeProduct")]
    public unsafe void ExportFormBalancesItsTablesWhenARegisteredTargetLabelFails()
    {
        var target = new ExportLabelTarget();
        var pipeline = new BuildPipeline(
            m_assets,
            m_plugins,
            m_settings,
            m_engine.serialization,
            m_engine.generations,
            m_compiler,
            m_supportPackRoot,
            [new GameBuildTargetBinding(target, target)],
            CreateManagedDeployments()
        );
        BuildSettings defaults = BuildSettings.CreateDefault("Test Game", "Scenes/Startup.iscene", target.id);
        var settings = new BuildSettingsStore(
            Path.Combine(m_projectRoot, "Settings.Build.inno"),
            m_engine.serialization,
            defaults
        );
        m_engine.modules.Register("Tests.Exporting", [
            typeof(EditorExportService).Assembly,
            Assembly.Load(new AssemblyName("Inno.Editor.Workbench"))
        ]);
        var context = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl"));
        var logs = new ExportLogSink();
        ImGuiNativeContext* native = NativeGui.Raw.CreateContext(null);
        try
        {
            NativeGui.Raw.GetIO()->IniFilename = null;
            m_engine.logs.RegisterSink(logs);
            using var session = new GuiSession();
            using IDisposable scope = session.Enter();
            using var runtime = new EditorInteractionRuntime(
                context,
                m_engine.types,
                m_engine.logs,
                [pipeline, settings, m_settings, session]
            );
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
        BuildSettings defaults = BuildSettings.CreateDefault("Test Game", "Scenes/Startup.iscene", new BuildTargetID("macos-arm64"));
        var settings = new BuildSettingsStore(
            Path.Combine(m_projectRoot, "Settings.Build.inno"),
            m_engine.serialization,
            defaults
        );
        m_engine.modules.Register("Tests.ExportPresentation", [
            typeof(EditorExportService).Assembly,
            Assembly.Load(new AssemblyName("Inno.Editor.Workbench"))
        ]);
        ImGuiNativeContext* native = NativeGui.Raw.CreateContext(null);
        try
        {
            NativeGui.Raw.GetIO()->IniFilename = null;
            ImGuiIO* io = NativeGui.Raw.GetIO();
            io->DisplaySize = new ImVec2C { X = width, Y = height };
            io->DeltaTime = 0.2f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;
            using var runtime = new UIRuntime(
                new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl")),
                m_engine.types,
                m_engine.logs,
                [m_pipeline, settings, m_settings]
            );
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
            using NativeStringLease title = Utf8Codec.EncodeNullTerminated(
                "Export as Plugin##export.plugin",
                allowsNull: false
            );
            ImGuiWindow* window = NativeGui.Raw.FindWindowByName((sbyte*)title.pointer);
            Assert.True(window is not null && window->WasActive != 0);
            Assert.True(window->Size.X <= width && window->Size.Y <= height);
            if (width == 1280)
            {
                Assert.Equal(680f, window->Size.X);
                Assert.InRange(window->ScrollMax.X, 0f, 0.01f);
                Assert.InRange(window->ScrollMax.Y, 0f, 0.01f);
            }
            else
            {
                Assert.True(window->ScrollMax.Y > 0f);
            }
            Assert.True(runtime.interactions.TryGetModule<EditorExportService>(out EditorExportService? exports));
            Assert.False(exports!.progress.isBusy);
        }
        finally
        {
            NativeGui.Raw.DestroyContext(native);
        }
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
            bool leaked = NativeGui.Raw.GetCurrentTable() is not null;
            // Retire only a table opened by this fixture's modal before reporting a failed regression.
            if (leaked)
            {
                NativeGui.EndTable();
            }
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
        public void Receive(LogEntry entry) => messages.Add(entry.message);

        internal List<string> messages { get; } = [];
    }

    private sealed class ExportLabelTarget : IGameBuildTarget, IGameContentCompiler
    {
        public BuildTargetID id => new("fixture-export");
        public BuildTargetID target => id;
        public ManagedDeploymentID defaultManagedDeployment => ManagedDeploymentID.coreClr;
        public string runtimeIdentifier => "fixture";
        public string displayName => reject ? throw new InvalidOperationException("Export target label failed.") : "Fixture target";

        public void Validate(string directory)
            => throw new InvalidOperationException("This presentation fixture cannot validate products.");

        public ValueTask<string> PackageAsync(
            GameBuildPackageContext context,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("This presentation fixture cannot package products.");

        public ValueTask CompileAsync(
            GameBuildContentContext context,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("This presentation fixture cannot compile content.");

        internal bool reject { get; set; }
    }
}
