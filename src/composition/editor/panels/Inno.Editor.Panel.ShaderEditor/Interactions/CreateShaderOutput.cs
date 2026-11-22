using System;
using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/create-output", ShaderEditorCanvas.C_AREA)]
internal sealed class CreateShaderOutput(ShaderEditorDocuments documents) : EditorAction<AssetFileEntry, string>
{
    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected override EditorActionState Query(EditorActionContext<AssetFileEntry, string> context)
    {
        if (!documents.TryGet(context.target, out ShaderEditorDocuments.Draft draft)
            || draft.readOnly
            || !Enum.TryParse(context.argument, out ShaderStage stage)
        ) {
            return EditorActionState.disabled;
        }
        GraphDocument graph = documents.Controller(draft).document;
        if (ShaderGraphDocument.ReadTarget(graph, documents.serialization, documents.context).Length != 0)
        {
            return EditorActionState.disabled;
        }
        bool exists = graph.nodes
            .Where(static node => node.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID)
            .Any(
                node => (
                    ShaderGraphDocument.Read(
                        node,
                        "settings",
                        new ShaderGraphStageSettings(),
                        documents.serialization,
                        documents.context
                    ).stage == stage
                )
            );
        return exists ? EditorActionState.disabled : EditorActionState.enabled;
    }

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry, string> context)
    {
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        GraphDocumentController controller = documents.Controller(draft);
        GraphDocument graph = controller.document.Clone();
        ShaderDefinition definition = ShaderGraphDocument.ReadDefinition(
            graph,
            documents.serialization,
            documents.context
        );
        ShaderStage stage = Enum.Parse<ShaderStage>(context.argument);
        ShaderProgramKind kind = stage == ShaderStage.Compute
                                 ? ShaderProgramKind.Compute
                                 : ShaderProgramKind.Raster;
        ShaderGraphPassProgram[] programs = ShaderGraphPrograms.Read(graph, documents.serialization, documents.context);
        ShaderGraphPassProgram? incomplete = kind == ShaderProgramKind.Raster
                                             ? programs.FirstOrDefault(
                                                 program => (
                                                     definition.passes.Any(pass => pass.name == program.pass && pass.programKind == kind)
                                                     && !program.stages
                                                         .Select(id => graph.FindNode(new GraphNodeID(id)))
                                                         .OfType<GraphNodeRecord>()
                                                         .Any(
                                                             node => (
                                                                 ShaderGraphDocument.Read(
                                                                     node,
                                                                     "settings",
                                                                     new ShaderGraphStageSettings(),
                                                                     documents.serialization,
                                                                     documents.context
                                                                 ).stage == stage
                                                             )
                                                         )
                                                 )
                                             )
                                             : null;
        string name;
        string[] stages;
        if (incomplete is ShaderGraphPassProgram existing && existing.pass is not null)
        {
            name = existing.pass;
            stages = existing.stages;
        }
        else
        {
            string prefix = kind.ToString();
            name = prefix;
            for (int suffix = 2; definition.passes.Any(pass => pass.name == name); suffix++)
            {
                name = prefix + " " + suffix;
            }
            definition.passes = [..definition.passes, new ShaderPassDefinition(name, kind)];
            graph.SetMetadata(
                ShaderGraphDocument.C_DEFINITION_KEY,
                ShaderGraphDocument.Encode(
                    documents.serialization.Serialize(definition, documents.context),
                    documents.serialization,
                    documents.context
                )
            );
            stages = [];
        }

        var id = new GraphNodeID(Guid.NewGuid().ToString("N"));
        var node = new GraphNodeRecord(id, ShaderGraphDocument.C_OUTPUT_DEFINITION_ID)
        {
            position = draft.menuPosition
        };
        node.SetValue(
            "settings",
            ShaderGraphDocument.Encode(
                new ShaderGraphStageSettings
                {
                    stage = stage,
                    outputs = stage == ShaderStage.Compute
                              ? []
                              : [new ShaderGraphOutput()
                              {
                                  id = stage == ShaderStage.Vertex ? "position" : "color",
                                  kind = stage == ShaderStage.Vertex ? ShaderIrOutputKind.ClipPosition : ShaderIrOutputKind.Color
                              }]
                },
                documents.serialization,
                documents.context
            )
        );
        graph.AddNode(node);
        graph = ShaderGraphPrograms.Bind(
            graph,
            name,
            stages.Select(static value => new GraphNodeID(value)).Append(id),
            documents.serialization,
            documents.context
        );
        controller.ReplaceDocument(graph, "Create " + stage + " Output");
        draft.canvas.SelectNodes([id]);
        draft.activeStage = id;
        documents.Changed(draft);
    }
}
