using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BGCS.Runtime;
using Xunit;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Diagnostics;
using Inno.Core.Events;
using Inno.Core.Graphs;
using Inno.Core.Input;
using Inno.Core.Serialization;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Core;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Editor.Shaders;
using Inno.Editor.Shaders.ImGui;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

using EngineColor = Inno.Core.Mathematics.Color;
using EngineVector4 = Inno.Core.Mathematics.Vector4;
using ImGuiApi = Inno.Native.ImGui.ImGui;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;

namespace Inno.Editor.Shaders.Native.Tests;

public sealed class ShaderGraphEditingTests : ShaderEditorFixture
{
    [Fact]
    public void ParameterPresentationRoundTripsWithoutChangingTheRuntimeDefinitionOrSemanticProgram()
    {
        AssetFileEntry entry = Create("Presentation.ishader");
        GraphDocument graph = m_source.Read(entry.assetPath).document;
        SerializationContext context = AssetSerializationContext.Create(m_assets);
        byte[] beforeDefinition = m_serialization.Serialize(ShaderGraphDocument.ReadDefinition(graph, m_serialization, context), context);
        string beforeHash = ShaderGraphArtifact.GetSemanticHash(
            ShaderGraphArtifact.Encode(graph, new Dictionary<GraphNodeID, byte[]>(), m_serialization),
            m_serialization
        );
        var id = new ShaderPropertyID("stable.tint");
        Assert.True(ShaderParameterPresentation.Read(graph, id, m_serialization, context).visible);
        ShaderParameterPresentation.Write(
            graph,
            id,
            new ShaderParameterPresentation()
            {
                group = "Surface",
                description = "Linear tint",
                visible = false,
                hasRange = true,
                minimum = -2,
                maximum = 8
            },
            m_serialization,
            context
        );
        GraphDocument restored = GraphDocumentCodec.Decode(GraphDocumentCodec.Encode(graph, m_serialization), m_serialization);
        ShaderParameterPresentation presentation = ShaderParameterPresentation.Read(restored, id, m_serialization, context);
        Assert.Equal("Surface", presentation.group);
        Assert.Equal("Linear tint", presentation.description);
        Assert.False(presentation.visible);
        Assert.True(presentation.hasRange);
        Assert.Equal(-2, presentation.minimum);
        Assert.Equal(8, presentation.maximum);
        presentation.group = "Detached";
        Assert.Equal("Surface", ShaderParameterPresentation.Read(restored, id, m_serialization, context).group);
        Assert.Equal(
            beforeDefinition,
            m_serialization.Serialize(ShaderGraphDocument.ReadDefinition(restored, m_serialization, context), context)
        );
        Assert.Equal(
            beforeHash,
            ShaderGraphArtifact.GetSemanticHash(
                ShaderGraphArtifact.Encode(restored, new Dictionary<GraphNodeID, byte[]>(), m_serialization),
                m_serialization
            )
        );
    }

    [Fact]
    public void ParameterPresentationRejectsInvalidBoundsBeforeMutatingTheGraph()
    {
        GraphDocument graph = m_source.Read(Create("Range.ishader").assetPath).document;
        byte[] before = GraphDocumentCodec.Encode(graph, m_serialization);
        SerializationContext context = AssetSerializationContext.Create(m_assets);
        foreach ((double, double) range in new[]
        {
            (double.NaN, 1d),
            (0d, double.PositiveInfinity),
            (2d, 1d),
            (-1e100, 0d),
            (0d, 1e100)
        }

        ) {
            Assert.Throws<ArgumentException>(
                () => ShaderParameterPresentation.Write(
                    graph,
                    new ShaderPropertyID("range"),
                    new ShaderParameterPresentation() { hasRange = true, minimum = range.Item1, maximum = range.Item2 },
                    m_serialization,
                    context
                )
            );
        }
        Assert.Equal(before, GraphDocumentCodec.Encode(graph, m_serialization));
    }

    [Theory]
    [MemberData(nameof(CanvasMetrics))]
    [Trait("Acceptance", "Visual")]
    public unsafe void NativeCanvasResizeCompletesLayoutAtFractionalUIScales(
        float zoom,
        float density,
        float width,
        float height,
        bool emptyGraph
    ) {
        AssetFileEntry entry = Create("Resize.ishader");
        SelectAndDraw(entry);
        GraphDocumentController controller = Controller(entry);
        if (emptyGraph)
        {
            controller.ReplaceDocument(new GraphDocument(), "Prepare Empty Canvas");
        }
        ulong revision = controller.revision;
        byte[] disk = File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath));
        ImGuiApi.Raw.GetIO()->DisplaySize = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(width, height));
        ImGuiApi.Raw.GetIO()->DisplayFramebufferScale = Unsafe.BitCast<Vector2, NativeVector2>(new Vector2(density));
        m_ui.widgets.style.SetZoom(zoom);
        using (m_ui.Enter())
        {
            m_ui.widgets.SetupStyle();
        }
        Vector2[] sizes =
        [
            new Vector2(width, height),
            new Vector2(640, 400),
            new Vector2(280, 220),
            new Vector2(96, 64),
            new Vector2(32, 32),
            new Vector2(width, 32),
            new Vector2(32, height),
            new Vector2(width, height)
        ];
        for (int cycle = 0; cycle < 3; cycle++)
        {
            foreach (Vector2 size in sizes)
            {
                // Run both the resize frame and its settled layout with native assertions enabled.
                Draw(size);
                Draw(size);
            }
        }

        Assert.Equal(revision, controller.revision);
        Assert.Equal(disk, File.ReadAllBytes(Path.Combine(m_root, "Assets", entry.assetPath.localPath)));
        Assert.Single(m_runtime.interactions.documents.documents);
    }

    [Fact]
    public void NativeMouseGesturesMoveNodesAndEachReleaseHasItsOwnUndo()
    {
        AssetFileEntry entry = Create("Pointer.ishader");
        SelectAndDraw(entry);
        GraphDocumentController controller = Controller(entry);
        var graph = new GraphDocument();
        var node = new GraphNodeRecord(new GraphNodeID("probe"), "tests.shader-ui-probe")
        {
            position = new(0, 0)
        };
        graph.AddNode(node);
        controller.ReplaceDocument(graph, "Prepare Canvas Probe");
        Assert.True(m_runtime.interactions.For("panel/rendering.shader-editor", entry).Execute("shader/focus"));
        Draw();
        Draw();
        GraphPosition original = controller.document.FindNode(node.id)!.position;
        DragProbe(new Vector2(52, 37));
        GraphPosition first = controller.document.FindNode(node.id)!.position;
        Assert.NotEqual(original, first);
        DragProbe(new Vector2(41, 23));
        GraphPosition second = controller.document.FindNode(node.id)!.position;
        Assert.NotEqual(first, second);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Tick();
        Assert.Equal(first, controller.document.FindNode(node.id)!.position);
        Assert.Null(m_source.Read(entry.assetPath).document.FindNode(node.id));
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Tick();
        Assert.Equal(original, controller.document.FindNode(node.id)!.position);
    }

    [Fact]
    public void FormatPlacesOutputsOnTheRightWithoutChangingShaderSemanticsAndIsUndoable()
    {
        AssetFileEntry entry = Create("Format.ishader");
        SelectAndDraw(entry);
        GraphDocumentController controller = Controller(entry);
        GraphDocument reversed = controller.document.Clone();
        foreach (GraphNodeRecord node in reversed.nodes)
        {
            node.position = node.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID
                            ? new GraphPosition(0, node.position.y)
                            : new GraphPosition(900, node.position.y);
        }
        controller.ReplaceDocument(reversed, "Reverse Shader Layout");
        byte[] before = GraphDocumentCodec.Encode(controller.document, m_serialization);
        string semantic = ShaderGraphArtifact.GetSemanticHash(
            ShaderGraphArtifact.Encode(controller.document, new Dictionary<GraphNodeID, byte[]>(), m_serialization),
            m_serialization
        );
        Assert.True(m_runtime.interactions.For("panel/rendering.shader-editor", entry).Execute("shader/format"));
        GraphNodeRecord[] outputs = controller.document.nodes
            .Where(static node => node.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID)
            .ToArray();
        Assert.All(
            outputs,
            output => Assert.True(
                output.position.x > controller.document.nodes
                    .Where(
                        node => (
                            node.id != output.id
                            && ShaderGraphDocument.Read(
                                node,
                                "stage",
                                string.Empty,
                                m_serialization,
                                AssetSerializationContext.Create(m_assets)
                            ) == output.id.value
                        )
                    )
                    .Max(static node => node.position.x)
            )
        );
        Assert.Equal(
            semantic,
            ShaderGraphArtifact.GetSemanticHash(
                ShaderGraphArtifact.Encode(controller.document, new Dictionary<GraphNodeID, byte[]>(), m_serialization),
                m_serialization
            )
        );
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(before, GraphDocumentCodec.Encode(controller.document, m_serialization));
    }

    [Theory]
    [InlineData(KeyCode.Delete)]
    [InlineData(KeyCode.Backspace)]
    public void DeleteKeysRemoveTheSelectedComputeStageAndItsDeclarationsAsOneUndo(KeyCode key)
    {
        AssetFileEntry entry = Create("Delete.ishader");
        SelectAndDraw(entry);
        EditorInteraction interaction = m_runtime.interactions.For("panel/rendering.shader-editor", entry);
        Assert.True(interaction.Execute("shader/create-output", "Compute"));
        Assert.False(interaction.Execute("shader/create-output", "Compute"));
        GraphDocumentController controller = Controller(entry);
        GraphNodeRecord output = controller.document.nodes.Last();
        var input = new GraphNodeRecord(new GraphNodeID("compute-parameter"), "inno.shader.stage-input");
        input.SetValue("stage", ShaderGraphDocument.Encode(output.id.value, m_serialization, AssetSerializationContext.Create(m_assets)));
        GraphDocument graph = controller.document.Clone();
        graph.AddNode(input);
        graph = ShaderGraphBindings.ChangeInput(
            graph,
            input.id,
            new ShaderGraphInputSettings() { id = "computeParameter", kind = ShaderIrInputKind.Uniform },
            m_serialization,
            AssetSerializationContext.Create(m_assets)
        );
        controller.ReplaceDocument(graph, "Add Compute Parameter");
        byte[] before = GraphDocumentCodec.Encode(controller.document, m_serialization);
        Assert.True(interaction.Query("shader/delete").isEnabled);
        Assert.True(interaction.Query("shader/cut").isEnabled, "Stage selection was lost before keyboard dispatch.");
        Assert.True(interaction.TryGetShortcut("shader/delete", out HotKeyGesture deleteGesture));
        Assert.Equal(KeyCode.Delete, deleteGesture.key);
        interaction.Focus();
        Assert.Equal("panel/rendering.shader-editor", m_runtime.interactions.focusedArea);
        var keyEvent = new KeyPressedEvent(0, key);
        m_runtime.HandleKeyPressed(keyEvent);
        m_logs.Flush();
        Assert.True(
            controller.document.FindNode(output.id)is null,
            "Focus: "
            + m_runtime.interactions.focusedArea
            + "; cut: "
            + interaction.Query("shader/cut").isEnabled
            + "; undo: "
            + m_runtime.interactions.history.undoName
            + "; "
            + string.Join("\n", m_messages.messages)
        );
        Assert.Null(controller.document.FindNode(input.id));
        ShaderDefinition definition = ShaderGraphDocument.ReadDefinition(
            controller.document,
            m_serialization,
            AssetSerializationContext.Create(m_assets)
        );
        Assert.Single(definition.passes);
        Assert.DoesNotContain(definition.properties, property => property.id.value == "computeParameter");
        using var nodes = new ShaderNodeCompilerRegistry(m_types);
        Assert.True(
            new ShaderGraphProgramCompiler(nodes).Lower(
                controller.document,
                "bgfx",
                new Dictionary<GraphNodeID, ShaderSourceModuleAnalysis>(),
                m_serialization,
                AssetSerializationContext.Create(m_assets)
            ).succeeded
        );
        Tick();
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(before, GraphDocumentCodec.Encode(controller.document, m_serialization));
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        Assert.Null(controller.document.FindNode(output.id));
    }

    [Fact]
    public void CutAndPasteStageKeepsItsContentsAndDoesNotCreateDuplicatePassDeclarations()
    {
        AssetFileEntry entry = Create("Clipboard.ishader");
        SelectAndDraw(entry);
        EditorInteraction interaction = m_runtime.interactions.For("panel/rendering.shader-editor", entry);
        Assert.True(interaction.Execute("shader/create-output", "Compute"));
        GraphDocumentController controller = Controller(entry);
        GraphNodeID stage = controller.document.nodes.Last().id;
        GraphNodeID child = controller.AddNode(
            "inno.shader.constant",
            new GraphPosition(10, 20),
            new Dictionary<string, GraphSerializedValue>
            {
                ["stage"] = ShaderGraphDocument.Encode(stage.value, m_serialization, AssetSerializationContext.Create(m_assets))
            }
        );
        Assert.True(interaction.Execute("shader/cut"));
        Assert.Null(controller.document.FindNode(child));
        Assert.True(interaction.Execute("shader/paste"));
        Assert.Null(controller.document.FindNode(child));
        Assert.Equal(
            2,
            controller.document.nodes.Count(
                node => node.definitionID == "inno.shader.stage-output" && ReadStage(node).stage != ShaderStage.Compute
            )
        );
        GraphNodeRecord pasted = controller.document.nodes.Single(
            node => node.definitionID == "inno.shader.stage-output" && ReadStage(node).stage == ShaderStage.Compute
        );
        Assert.Contains(
            controller.document.nodes,
            node => (
                ShaderGraphDocument.Read(
                    node,
                    "stage",
                    string.Empty,
                    m_serialization,
                    AssetSerializationContext.Create(m_assets)
                ) == pasted.id.value
            )
        );
        Assert.False(interaction.Execute("shader/duplicate"));
        ShaderDefinition definition = ShaderGraphDocument.ReadDefinition(
            controller.document,
            m_serialization,
            AssetSerializationContext.Create(m_assets)
        );
        Assert.Equal(2, definition.passes.Length);
        Assert.Equal(2, definition.passes.Select(pass => pass.name).Distinct().Count());
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Single(
            ShaderGraphDocument.ReadDefinition(controller.document, m_serialization, AssetSerializationContext.Create(m_assets)).passes
        );
        ShaderGraphStageSettings ReadStage(GraphNodeRecord node)
            => ShaderGraphDocument.Read(
                node,
                "settings",
                new ShaderGraphStageSettings(),
                m_serialization,
                AssetSerializationContext.Create(m_assets)
            );
    }

    [Fact]
    public void SourceMenuUsesAssetIdentityAndDisconnectCommitsOneUndoWithoutSaving()
    {
        AssetPath source = CreateFunction("Evaluate", "Secondary");
        AssetFileEntry entry = Create("Source.ishader");
        SelectAndDraw(entry);
        EditorInteraction interaction = m_runtime.interactions.For("panel/rendering.shader-editor", entry);
        EditorMenuItem create = interaction.BuildMenu().items.Single(item => item.label == "Create");
        EditorMenuItem functions = create.children.Single(item => item.label == "Functions");
        EditorMenuItem general = Assert.Single(functions.children);
        Assert.Equal("General", general.label);
        EditorMenuItem library = Assert.Single(general.children);
        Assert.Equal(["Evaluate", "Secondary"], library.children.Select(static item => item.label));
        EditorMenuItem function = library.children.Single(item => item.label == "Evaluate");
        Assert.True(interaction.Execute(function.actionID!, function.argument));
        GraphDocumentController controller = Controller(entry);
        GraphNodeRecord node = controller.document.nodes.Single(node => node.definitionID == "inno.shader.source");
        Assert.True(m_assets.TryGetInfo(source, out AssetInfo? info));
        Assert.Equal(
            info!.persistentID,
            ShaderGraphDocument.Read(node, "sourceId", Guid.Empty, m_serialization, AssetSerializationContext.Create(m_assets))
        );
        Assert.Equal(
            "Evaluate",
            ShaderGraphDocument.Read(node, "function", string.Empty, m_serialization, AssetSerializationContext.Create(m_assets))
        );
        Tick();
        using var nodes = new ShaderNodeCompilerRegistry(m_types);
        using var frontends = new ShaderSourceFrontendRegistry(m_types);
        ShaderFunctionAsset functionAsset = m_assets.Load<ShaderFunctionAsset>(source);
        ShaderSourceModuleAnalysis module = frontends.AnalyzeModule(
            ShaderSourceBundle.Decode(ShaderSourceBundle.Read(functionAsset, m_assets), "Evaluate", m_serialization)
        );
        IReadOnlyList<ShaderNodePort> ports = nodes.DescribePorts(
            node,
            m_serialization,
            AssetSerializationContext.Create(m_assets),
            module,
            functionAsset.implementationId,
            null
        );
        Assert.Contains(ports, port => port.direction == GraphPortDirection.Input);
        Assert.Contains(ports, port => port.direction == GraphPortDirection.Output);
        GraphNodeID constant = controller.AddNode("inno.shader.constant", new GraphPosition(0, 0));
        GraphDocument candidate = controller.document.Clone();
        var edge = new GraphEdgeRecord(
            new GraphEdgeID("source-link"),
            new GraphEndpoint(constant, new GraphPortID("value")),
            new GraphEndpoint(node.id, new GraphPortID(ports.First(port => port.direction == GraphPortDirection.Input).id))
        );
        candidate.AddEdge(edge);
        controller.ReplaceDocument(candidate, "Connect Source");
        Assert.True(interaction.Execute("shader/disconnect"));
        Assert.DoesNotContain(controller.document.edges, value => value.id == edge.id);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Contains(controller.document.edges, value => value.id == edge.id);
        Assert.Null(m_source.Read(entry.assetPath).document.FindNode(node.id));
    }

    [Fact]
    public void SourceLibrariesOwnTheirNestedCreationCatalogWithoutLeakingSeparatorsToParentMenus()
    {
        AssetPath source = CreateFunction("Evaluate");
        AssetImportSettingsSnapshot snapshot = m_assets.GetImportSettings(source);
        ShaderSourceImportSettings settings = Assert.IsType<ShaderSourceImportSettings>(snapshot.value);
        settings.catalogPath = "Rendering 2D/Post Processing";
        settings.catalogOrder = 400;
        Assert.True(m_assets.SaveImportSettings(source, settings, snapshot.fingerprint));
        AssetFileEntry entry = Create("Catalog.ishader");
        SelectAndDraw(entry);
        EditorMenuItem create = m_runtime.interactions
            .For("panel/rendering.shader-editor", entry)
            .BuildMenu().items
            .Single(item => item.label == "Create");
        EditorMenuItem functions = create.children.Single(item => item.label == "Functions");
        EditorMenuItem rendering2D = functions.children.Single(item => item.label == "Rendering 2D");
        Assert.True(rendering2D.separatorBefore);
        Assert.True(functions.separatorBefore);
        Assert.False(create.separatorBefore);
        EditorMenuItem post = Assert.Single(rendering2D.children);
        Assert.Equal("Post Processing", post.label);
        Assert.Equal("Evaluate", Assert.Single(Assert.Single(post.children).children).label);
    }

    [Fact]
    public void PluginNodePresentationOwnsItsCreationGroupAndSeparator()
    {
        AssetFileEntry entry = Create("NodeCatalog.ishader");
        SelectAndDraw(entry);
        EditorMenuItem create = m_runtime.interactions
            .For("panel/rendering.shader-editor", entry)
            .BuildMenu().items
            .Single(item => item.label == "Create");
        EditorMenuItem domain = create.children.Single(item => item.label == "Domain");
        EditorMenuItem extension = domain.children.Single(item => item.label == "Test Extension");
        Assert.True(extension.separatorBefore);
        EditorMenuItem node = Assert.Single(extension.children);
        Assert.Equal("Probe Node", node.label);
        Assert.False(domain.separatorBefore);
        Assert.False(create.separatorBefore);
    }

    [Fact]
    public void FormatOrdersSourcesByDestinationPortsToAvoidInputWireCrossings()
    {
        AssetFileEntry entry = Create("Crossings.ishader");
        SelectAndDraw(entry);
        var graph = new GraphDocument();
        var top = new GraphNodeRecord(new GraphNodeID("top"), "inno.shader.constant")
        {
            position = new(800, 600)
        };
        var middle = new GraphNodeRecord(new GraphNodeID("middle"), "inno.shader.constant")
        {
            position = new(800, 400)
        };
        var bottom = new GraphNodeRecord(new GraphNodeID("bottom"), "inno.shader.constant")
        {
            position = new(800, 200)
        };
        var evaluate = new GraphNodeRecord(new GraphNodeID("evaluate"), "inno.shader.construct")
        {
            position = new(400, 300)
        };
        evaluate.SetValue("type", ShaderGraphDocument.Encode("float3", m_serialization, SerializationContext.empty));
        var output = new GraphNodeRecord(new GraphNodeID("output"), "tests-output")
        {
            position = new(0, 300)
        };
        foreach (GraphNodeRecord node in new[]
        {
            top,
            middle,
            bottom,
            evaluate,
            output
        }

        ) {
            graph.AddNode(node);
        }
        graph.AddEdge(
            new GraphEdgeRecord(
                new GraphEdgeID("top-edge"),
                new GraphEndpoint(top.id, new GraphPortID("value")),
                new GraphEndpoint(evaluate.id, new GraphPortID("component.0"))
            )
        );
        graph.AddEdge(
            new GraphEdgeRecord(
                new GraphEdgeID("middle-edge"),
                new GraphEndpoint(middle.id, new GraphPortID("value")),
                new GraphEndpoint(evaluate.id, new GraphPortID("component.1"))
            )
        );
        graph.AddEdge(
            new GraphEdgeRecord(
                new GraphEdgeID("bottom-edge"),
                new GraphEndpoint(bottom.id, new GraphPortID("value")),
                new GraphEndpoint(evaluate.id, new GraphPortID("component.2"))
            )
        );
        graph.AddEdge(
            new GraphEdgeRecord(
                new GraphEdgeID("result-edge"),
                new GraphEndpoint(evaluate.id, new GraphPortID("value")),
                new GraphEndpoint(output.id, new GraphPortID("color"))
            )
        );
        GraphDocumentController controller = Controller(entry);
        controller.ReplaceDocument(graph, "Prepare crossing layout");
        Assert.True(m_runtime.interactions.For("panel/rendering.shader-editor", entry).Execute("shader/format"));
        GraphDocument formatted = controller.document;
        Assert.True(formatted.FindNode(top.id)!.position.y < formatted.FindNode(middle.id)!.position.y);
        Assert.True(formatted.FindNode(middle.id)!.position.y < formatted.FindNode(bottom.id)!.position.y);
        Assert.True(formatted.FindNode(top.id)!.position.x < formatted.FindNode(evaluate.id)!.position.x);
        Assert.True(formatted.FindNode(evaluate.id)!.position.x < formatted.FindNode(output.id)!.position.x);
    }

    public static IEnumerable<object[]> CanvasMetrics()
    {
        foreach (float zoom in new[] { 0.75f, 0.85f, 0.9f, 1f, 1.1f, 1.25f, 1.5f })
        {
            foreach (float density in new[] { 1f, 1.25f, 1.5f, 2f })
            {
                foreach ((float width, float height) in new[] { (1280f, 720f), (1920f, 1080f) })
                {
                    foreach (bool emptyGraph in new[] { false, true })
                    {
                        yield return [zoom, density, width, height, emptyGraph];
                    }
                }
            }
        }
    }

    private unsafe void DragProbe(Vector2 distance)
    {
        BeginDragProbe(distance);
        ImGuiApi.Raw.AddMouseButtonEvent(ImGuiApi.Raw.GetIO(), 0, 0);
        Draw();
        Tick();
        m_probeOffset += distance;
    }
}
