using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.FileBrowser;

[EditorMenuSource(FileBrowserInteractionIDs.C_AREA)]
internal sealed class AssetCreationMenu(AssetBrowserModule assets) : EditorMenuSource
{
    /// <summary>
    /// Builds a validated result from the current immutable input snapshot.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="builder">
    /// The builder consumed by build; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void Build(
        EditorMenuContext context,
        EditorMenuBuilder builder
    ) {
        IReadOnlyList<AssetCreationMenuAttribute> templates = assets.edits.creationTemplates;
        var declaredGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (AssetCreationMenuAttribute declaration in templates)
        {
            string[] segments = declaration.menuPath.Split('/');
            if (segments.Length > 1)
            {
                string topLevel = "Create/" + segments[0];
                if (declaredGroups.Add(topLevel))
                {
                    builder.AddGroup(topLevel, declaration.groupOrder, declaration.separatorBeforeGroup);
                }

                for (int depth = 1; depth < segments.Length - 1; depth++)
                {
                    string group = "Create/" + string.Join('/', segments.Take(depth + 1));
                    if (declaredGroups.Add(group))
                    {
                        builder.AddGroup(group, declaration.itemOrder);
                    }
                }
            }

            builder.Add(
                "Create/" + declaration.menuPath,
                FileBrowserInteractionIDs.C_CREATE_ASSET,
                declaration.itemOrder,
                argument: declaration.id
            );
        }
    }
}
