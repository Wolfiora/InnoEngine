using System;
using System.Collections.Generic;
using System.Linq;
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
