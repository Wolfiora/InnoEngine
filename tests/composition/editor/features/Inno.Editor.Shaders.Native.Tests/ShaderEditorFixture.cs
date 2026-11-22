using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BGCS.Runtime;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Diagnostics;
using Inno.Core.Graphs;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Core;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Editor.Rendering;
using Inno.Editor.Shaders.ImGui;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using EngineVector4 = Inno.Core.Mathematics.Vector4;
using ImGuiApi = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.Shaders.Native.Tests;

public abstract class ShaderEditorFixture : IDisposable
{
    protected readonly EditorUI m_ui = new();
    protected readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoShaderEditorWorkflow", Guid.NewGuid().ToString("N"));
    private readonly IdentityAllocator m_identities = new();
    private readonly IDisposable m_identityScope;
    private readonly ModuleHost m_modules;
    protected readonly TypeCatalog m_types;
    protected readonly SerializationRegistry m_serialization;
    protected readonly LogRouter m_logs = new();
    protected readonly WorkflowLogs m_messages = new();
    private readonly DiagnosticHub m_diagnostics;
    private readonly IDisposable m_diagnosticScope;
    private readonly DiagnosticReporter m_reporter;
    protected readonly AssetPipeline m_assets;
    protected readonly EditorRenderTargetArtifactProvider m_artifacts;
    protected readonly WorkflowSink m_sink = new();
    protected readonly ShaderGraphSourceStore m_source;
    private readonly unsafe ImGuiNativeContext* m_imgui;
    protected EditorInteractionRuntime m_runtime;
    private Vector2 m_canvasMinimum;
    private Vector2 m_canvasMaximum;
    protected Vector2 m_probeOffset;

    protected unsafe ShaderEditorFixture()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "Assets"));
        m_logs.RegisterSink(m_messages);
        m_identityScope = m_identities.EnterScope();
        _ = typeof(ShaderNodeDrawer);
        _ = typeof(ShaderGraphSourceStore);
        _ = typeof(GraphEditorModule);
        m_modules = new(
            new ModuleHostOptions() { catalogSource = new DotNetAssemblyCatalogSource(typeof(ShaderEditorFixture).Assembly) }
        );
        m_types = new(m_modules, new ReflectionTypeCatalogSource());
        m_serialization = new(m_types, new ReflectionSerializationMetadataSource());
        m_diagnostics = new DiagnosticHub();
        m_diagnosticScope = m_diagnostics.EnterScope();
        m_reporter = m_diagnostics.CreateReporter(new DiagnosticSource("tests.shader-editor", "Shader Editor workflow"));
        m_assets = new(
            m_modules,
            m_types,
            m_serialization,
            m_identities,
            m_diagnostics,
            m_logs,
            AssetPipelineOptions.Create(
                Path.Combine(m_root, "Assets"),
                Path.Combine(m_root, "Library")
            )with { enableFileSystemWatcher = false }
        );
        m_source = new(m_assets, m_serialization);
        m_artifacts = new(
            m_assets,
            m_serialization,
            m_types,
            new ShaderCompiler(new WorkflowCompiler()),
            new BgfxTextureTargetCompiler(),
            m_reporter
        );
        m_runtime = CreateRuntime();
        m_imgui = ImGuiApi.Raw.CreateContext(null);
        ImGuiIO* io = ImGuiApi.Raw.GetIO();
        io->IniFilename = null;
        io->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(1200, 800));
        io->DeltaTime = 1f / 60;
        io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
        io->Fonts->RendererHasTextures = 1;
    }

    public unsafe void Dispose()
    {
        m_runtime.Dispose();
        m_ui.Dispose();
        ImGuiApi.Raw.DestroyContext(m_imgui);
        m_artifacts.Dispose();
        m_assets.Dispose();
        m_reporter.Dispose();
        m_serialization.Dispose();
        m_types.Dispose();
        m_modules.Dispose();
        m_logs.Dispose();
        m_diagnosticScope.Dispose();
        m_identityScope.Dispose();
        Directory.Delete(m_root, true);
    }

    protected AssetPath CreateFunction(params string[] exports)
    {
        if (exports.Length == 0)
        {
            exports = ["Evaluate"];
        }
        AssetPath path = AssetPath.Project("Function.ishadersource");
        File.WriteAllText(
            Path.Combine(m_root, "Assets", path.localPath),
            string.Join("\n", exports.Select(static function => $"float {function}(float input) {{ return input; }}"))
        );
        _ = m_assets.Import(path);
        var settings = new ShaderSourceImportSettings
        {
            languageId = "inno.shader-language.bgfx-sc",
            implementationId = "bgfx",
            exports = exports
        };
        Assert.True(m_assets.SaveImportSettings(path, settings, m_assets.GetImportSettings(path).fingerprint));
        return path;
    }

    protected unsafe void BeginDragProbe(Vector2 distance)
    {
        // A single framed summary-only node is centered in the canvas (82 px tall).
        Vector2 start = (m_canvasMinimum + m_canvasMaximum) * 0.5f + new Vector2(0, -25) + m_probeOffset;
        Assert.True(start.X > 0 && start.Y > 0);
        ImGuiApi.Raw.AddMousePosEvent(ImGuiApi.Raw.GetIO(), start.X, start.Y);
        Draw();
        ImGuiApi.Raw.AddMouseButtonEvent(ImGuiApi.Raw.GetIO(), 0, (true ? (byte)1 : (byte)0));
        Draw();
        ImGuiApi.Raw.AddMousePosEvent(ImGuiApi.Raw.GetIO(), start.X + distance.X, start.Y + distance.Y);
        Draw();
    }

    protected AssetFileEntry Create(string name)
    {
        File.WriteAllBytes(
            Path.Combine(m_root, "Assets", name),
            GraphDocumentCodec.Encode(
                ShaderGraphTemplates.CreateRaster(m_serialization, AssetSerializationContext.Create(m_assets)),
                m_serialization
            )
        );
        Assert.True(m_assets.Import(AssetPath.Project(name)));
        Assert.True(m_assets.TryGetFileSystemEntry(AssetPath.Project(name), out AssetFileEntry entry));
        return entry;
    }

    protected EditorInteractionRuntime CreateRuntime()
    {
        RenderTextureFormat[] formats = Enum.GetValues<RenderTextureFormat>();
        var capabilities = new GraphicsCapabilities(
            GraphicsApi.metal,
            GraphicsCapability.None,
            new GraphicsLimits(256, 8, 8192, 16),
            formats,
            formats,
            formats,
            formats,
            false,
            false
        );
        var runtime = new EditorInteractionRuntime(
            new EditorContext(m_root, new EditorKeyboardPolicy(KeyModifier.Control, "Super")),
            m_types,
            m_logs,
            [
                m_types,
                m_serialization,
                m_identities,
                m_assets,
                m_sink,
                m_ui,
                new EditorReloadCoordinator(),
                new EditorShaderCompilation(m_artifacts, capabilities),
                new EmptyPreviews()
            ]
        );
        runtime.Start();
        return runtime;
    }

    protected (AssetPath, ShaderPropertyID) CreateMaterial()
    {
        AssetFileEntry shaderEntry = Create("MaterialSurface.ishader");
        ShaderAsset shader = m_assets.Load<ShaderAsset>(shaderEntry.assetPath);
        AssetPath path = AssetPath.Project("Surface.imaterial");
        AssetSourceStore sources = m_assets.CreateSourceStore();
        sources.Save(path, sources.Encode(new MaterialAsset { shader = shader }), null);
        Assert.True(m_assets.Import(path));
        return (path, Assert.Single(shader.definition!.properties).id);
    }

    protected GraphDocumentController Controller(AssetFileEntry entry)
    {
        Assert.NotNull(m_sink.graphs);
        Assert.True(m_sink.graphs.TryOpenDocument(AssetID(entry), m_runtime.interactions.history, out GraphDocumentController? controller));
        return controller!;
    }

    protected Guid AssetID(AssetFileEntry entry)
    {
        Assert.True(m_assets.TryGetInfo(entry.assetPath, out AssetInfo? info));
        return info!.persistentID;
    }

    protected void SelectAndDraw(AssetFileEntry entry)
    {
        Assert.True(m_assets.TryGetFileSystemEntry(entry.assetPath, out AssetFileEntry current));
        m_runtime.interactions.SetSelection(current);
        Draw();
    }

    protected void Tick()
    {
        m_runtime.Update(new EditorFrame(1f / 60, 0, true));
        Draw();
    }

    protected unsafe void Draw(Vector2? size = null)
    {
        EditorPanelExtension panel = Assert.Single(m_runtime.panels, panel => panel.id == "rendering.shader-editor");
        panel.isOpen = true;
        using IDisposable uiScope = m_ui.Enter();
        ImGuiApi.NewFrame();
        ImGuiApi.SetNextWindowPos(new Vector2(0, 0));
        ImGuiApi.SetNextWindowSize(size ?? new Vector2(1200, 800));
        using NativeStringLease nativeTestText1 = Utf8Codec.EncodeNullTerminated("Shader Editor Workflow", allowsNull: false);
        _ = ImGuiApi.Raw.Begin((sbyte*)nativeTestText1.pointer, null, 0);
        try
        {
            Assert.True(panel.Draw(m_runtime.context));
            m_canvasMinimum = ImGuiApi.GetItemRectMin();
            m_canvasMaximum = ImGuiApi.GetItemRectMax();
        }
        finally
        {
            ImGuiApi.End();
            ImGuiApi.Render();
        }
    }

    public sealed unsafe class WorkflowSink
    {
        public GraphEditorModule? graphs { get; set; }
        public MaterialDocuments? materials { get; set; }
        public PipelineDocuments? pipelines { get; set; }
    }

    protected sealed class WorkflowLogs : ILogSink
    {
        internal readonly ConcurrentQueue<string> messages = new();

        public void Receive(LogEntry entry) => messages.Enqueue(entry.message);
    }

    [ShaderNodeDrawer("tests.shader-ui-probe", "Probe Node", "Domain/Test Extension", 850, separatorBefore: true)]
    public sealed unsafe class CanvasProbeDrawer : ShaderNodeDrawer
    {
        public static Vector2 header;

        public override void Draw(ShaderNodeDrawContext context)
        {
            float zoom = ImGuiApi.GetWindowSize().X / 246f;
            header = ImGuiApi.GetWindowPos() + new Vector2(60, -24) * zoom;
            ImGuiApi.TextUnformatted("Extension controls");
        }
    }

    public sealed unsafe class CanvasProbeCompiler : IShaderNodeCompiler
    {
        public string definitionID => "tests.shader-ui-probe";

        public IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context) => [];

        public IReadOnlyDictionary<string, ShaderIrValue> Lower(ShaderNodeLoweringContext context)
            => new Dictionary<string, ShaderIrValue>();
    }

    [EditorModule("tests.shader-workflow", order: 160)]
    public sealed unsafe class WorkflowProbe : EditorModule
    {
        public WorkflowProbe(
            GraphEditorModule graphs,
            MaterialDocuments materials,
            PipelineDocuments pipelines,
            WorkflowSink sink
        ) {
            sink.graphs = graphs;
            sink.materials = materials;
            sink.pipelines = pipelines;
        }
    }

    private sealed class WorkflowCompiler : IShaderCompilerToolchain
    {
        public string implementationID => "tests.workflow";
        public IReadOnlyList<string> supportedSourceLanguages => [];

        public ShaderCompileTarget CreateTarget(
            GraphicsCapabilities capabilities,
            bool optimize = true,
            bool debugInformation = false
        ) => new("tests:workflow", capabilities, optimize, debugInformation);

        public ValueTask<ShaderStageToolResult> CompileAsync(
            ShaderStageToolRequest request,
            CancellationToken cancellationToken
        ) => ValueTask.FromResult(
            new ShaderStageToolResult(
                [1, 2, 3],
                request.stage.inputs
                    .Where(input => input.kind == ShaderIrInputKind.Uniform)
                    .Select(input => new ShaderStageBinding(input.id, "native_" + input.id, 0)),
                []
            )
        );
    }

    private sealed class EmptyPreviews : IEditorPreviewService
    {
        public uint deviceGeneration => 1;

        public bool TryRender(
            EditorViewportComposition composition,
            out EditorPreviewHandle handle
        ) {
            handle = default;
            return false;
        }

        public void ReleaseRendered(string viewportID) {}

        public bool TryGetTexture(
            TextureAsset texture,
            out EditorPreviewHandle handle
        ) {
            handle = default;
            return false;
        }

        public bool TryGetTextureArtifact(
            RenderTextureArtifactReference texture,
            int pixelWidth,
            int pixelHeight,
            out EditorPreviewHandle handle
        ) {
            handle = default;
            return false;
        }

        public void Draw(
            EditorPreviewHandle handle,
            Vector2 logicalSize
        ) => throw new InvalidOperationException("No texture was requested by this workflow.");

        public bool Release(EditorPreviewHandle handle) => false;

        public void ReleaseAll() {}
    }
}
