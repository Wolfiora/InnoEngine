using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Execution;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Shaders.ImGui;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorMenuSource(ShaderEditorCanvas.C_AREA)]
internal sealed class ShaderCanvasMenu(ShaderEditorDocuments documents) : EditorMenuSource
{
    /// <summary>
    /// Builds a validated result from the current immutable input snapshot.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="builder">
    /// The builder consumed by build; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void Build(
        EditorMenuContext context,
        EditorMenuBuilder builder
    ) {
        if (context.target is not AssetFileEntry entry || !documents.TryGet(entry, out ShaderEditorDocuments.Draft draft))
        {
            return;
        }
        builder.AddGroup("Create/Functions", order: 500, separatorBefore: true);
        builder.AddGroup("Create/Graph Nodes", order: 550, separatorBefore: true);
        builder.AddGroup("Create/Domain Outputs", order: 800, separatorBefore: true);
        bool explicitStages = ShaderGraphDocument.ReadTarget(
            documents.Controller(draft).document,
            documents.serialization,
            documents.context
        ).Length == 0;
        if (draft.createFromPort is null && explicitStages)
        {
            builder.Add("Create/Outputs/Vertex Output", "shader/create-output", order: 0, argument: "Vertex");
            builder.Add("Create/Outputs/Fragment Output", "shader/create-output", order: 10, argument: "Fragment");
            builder.Add("Create/Outputs/Compute Output", "shader/create-output", order: 20, argument: "Compute");
        }

        foreach (string definition in documents.nodes.definitionIDs)
        {
            if (definition is not ("inno.shader.source" or ShaderGraphNodes.C_CALL_DEFINITION_ID)
                && documents.CanCreate(draft, new ShaderNodeCreation(definition))
            ) {
                string category = Category(definition);
                int order = CategoryOrder(category);
                if (documents.drawers?.TryGetPresentation(definition, out ShaderNodePresentation presentation) == true
                    && presentation.createPath.Length != 0
                ) {
                    category = presentation.createPath;
                    order = presentation.createOrder;
                    builder.AddGroup("Create/" + category, order, presentation.separatorBefore);
                }

                builder.Add(
                    "Create/"
                    + category
                    + "/"
                    + (documents.drawers?.GetDisplayName(definition)
                       ?? EditorDisplayNames.Nicify(
                           definition.Replace("inno.shader.", string.Empty, StringComparison.Ordinal).Replace('-', ' ')
                       )),
                    "shader/create-node",
                    order,
                    argument: new ShaderNodeCreation(definition)
                );
            }
        }

        List<(AssetFileEntry source, AssetInfo info, ShaderFunctionAsset library)> functionLibraries = [];
        foreach (AssetFileEntry source in documents.assets.GetFileSystemEntries(includeDirectories: false))
        {
            if (source.extension == ".ishadersource"
                && documents.assets.TryGetInfo(source.assetPath, out AssetInfo? info)
                && info is not null
                && documents.assets.TryLoad(info.persistentID, out ShaderFunctionAsset? library)
                && library is not null
                && !library.isMissing
            ) {
                functionLibraries.Add((source, info, library));
            }
        }
        foreach ((string name, int order) catalog in functionLibraries
            .Select(static value => (path: NormalizeCatalog(value.library.catalogPath), value.library.catalogOrder))
            .Where(static value => value.path.Length != 0)
            .GroupBy(static value => value.path.Split('/')[0], StringComparer.Ordinal)
            .Select(static group => (name: group.Key, order: group.Min(static value => value.catalogOrder)))
            .OrderBy(static value => value.order)
            .ThenBy(static value => value.name, StringComparer.Ordinal)
        ) {
            builder.AddGroup("Create/Functions/" + catalog.name, order: 500 + catalog.order, separatorBefore: true);
        }
        foreach ((AssetFileEntry source, AssetInfo info, ShaderFunctionAsset library)in functionLibraries
            .OrderBy(static value => value.library.catalogOrder)
            .ThenBy(static value => value.source.nameWithoutExtension, StringComparer.Ordinal)
        ) {
            foreach (string function in library.exports)
            {
                if (documents.CanCreate(draft, new ShaderNodeCreation("inno.shader.source", info.persistentID, function)))
                {
                    string catalog = NormalizeCatalog(library.catalogPath);
                    string prefix = "Create/Functions/" + (catalog.Length == 0 ? "General" : catalog);
                    builder.Add(
                        prefix + "/" + source.nameWithoutExtension + "/" + function,
                        "shader/create-node",
                        order: 500 + library.catalogOrder,
                        argument: new ShaderNodeCreation("inno.shader.source", info.persistentID, function)
                    );
                }
            }
        }

        foreach (AssetFileEntry source in documents.assets
            .GetFileSystemEntries(includeDirectories: false)
            .Where(static entry => entry.extension == ".ishader")
            .OrderBy(static entry => entry.assetPath.ToString(), StringComparer.Ordinal)
        ) {
            if (!documents.assets.TryGetInfo(source.assetPath, out AssetInfo? info)
                || info is null
                || info.persistentID == draft.id
                || info.status != AssetImportStatus.Imported
            ) {
                continue;
            }
            ShaderGraphNodeInterface nodeInterface;
            try
            {
                nodeInterface = documents.LoadGraphNodeInterface(info.persistentID);
            }
            catch (Exception failure) when ((failure is InvalidOperationException or ArgumentException or FormatException)
                                            && RetirementPendingException.Find(failure) is null)
            {
                continue;
            }

            var creation = new ShaderNodeCreation(ShaderGraphNodes.C_CALL_DEFINITION_ID, info.persistentID);
            if (!documents.CanCreate(draft, creation))
            {
                continue;
            }
            string catalog = NormalizeCatalog(nodeInterface.createPath);
            string root = nodeInterface.kind == ShaderGraphNodeKind.DomainOutput ? "Domain Outputs" : "Graph Nodes";
            string group = "Create/" + root + (catalog.Length == 0 ? string.Empty : "/" + catalog);
            builder.AddGroup(
                group,
                order: nodeInterface.createOrder,
                separatorBefore: nodeInterface.kind == ShaderGraphNodeKind.DomainOutput
            );
            builder.Add(
                group + "/" + nodeInterface.displayName,
                "shader/create-node",
                order: nodeInterface.createOrder,
                argument: creation
            );
        }

        builder.Add("View/Focus Selection", "shader/focus", order: 900);
        builder.Add("Edit/Copy", "shader/copy", order: 1000);
        builder.Add("Edit/Cut", "shader/cut", order: 110);
        builder.Add("Edit/Paste", "shader/paste", order: 120);
        builder.Add("Edit/Duplicate", "shader/duplicate", order: 130);
        builder.Add("Edit/Delete", "shader/delete", order: 140, separatorBefore: true);
        builder.Add("Connections/Disconnect", "shader/disconnect", order: 1500);
        builder.Add("Connections/Insert Reroute", "shader/reroute", order: 160);
        builder.Add("Organize/Group Selection", "shader/group", order: 1700);
        builder.Add("Organize/Ungroup Selection", "shader/ungroup", order: 180);
        builder.Add("Organize/Collapse to Subgraph", "shader/collapse-subgraph", order: 190, separatorBefore: true);
        builder.Add("Assets/Show Shader in File Browser", "shader/reveal-shader", order: 1900);
        builder.Add("Assets/Show Source in File Browser", "shader/reveal-source", order: 1910);
        builder.Add("Assets/Copy Shader To Project", "shader/copy-to-project", order: 1920, separatorBefore: true);
        static string Category(string definition)
            => definition switch
        {
            "inno.shader.constant" or "inno.shader.stage-input" => "Inputs",
            ShaderGraphNodes.C_INPUT_DEFINITION_ID or ShaderGraphNodes.C_OUTPUT_DEFINITION_ID => "Interface",
            "inno.shader.sample" => "Textures",
            "inno.shader.binary" or "inno.shader.construct" or "inno.shader.extract" or "inno.shader.select" => "Math",
            "inno.shader.storage-load" or "inno.shader.storage-store" or "inno.shader.storage-atomic-add" => "Resources",
            "inno.shader.discard" => "Flow",
            "inno.shader.reroute" => "Utility",
            _ when definition.Contains("surface-output", StringComparison.Ordinal) => "Domain Outputs",
            _ => "Domain"
        };
        static int CategoryOrder(string category)
            => category switch
        {
            "Outputs" => 0,
            "Interface" => 50,
            "Inputs" => 100,
            "Textures" => 200,
            "Math" => 300,
            "Resources" => 400,
            "Flow" => 600,
            "Utility" => 700,
            "Domain Outputs" => 800,
            _ => 850
        };
        static string NormalizeCatalog(string value)
            => string.Join('/', value.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }
}
