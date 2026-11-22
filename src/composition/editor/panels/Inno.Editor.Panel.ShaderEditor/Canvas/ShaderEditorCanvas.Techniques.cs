using System;
using System.Linq;
using Inno.Rendering;
using Inno.Rendering.Assets;

using EditorImGui = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUISession;

namespace Inno.Editor.Panel.ShaderEditor;

internal sealed partial class ShaderEditorCanvas
{
    private bool TechniqueControls(ShaderDefinition definition)
    {
        if (!EditorImGui.CollapsingHeader("Techniques & Roles"))
        {
            return false;
        }
        EditorUI.current.widgets.WrappedText(
            "Techniques map pipeline-defined roles to this shader's passes. Contract and role identities are supplied by the rendering pipeline."
        );
        bool changed = false;
        for (int i = 0; i < definition.techniques.Length; i++)
        {
            EditorImGui.PushID("technique-" + i);
            try
            {
                ShaderTechniqueDefinition technique = definition.techniques[i];
                string id = technique.id.value ?? string.Empty;
                string contract = technique.contract.value ?? string.Empty;
                bool idChanged = false;
                InspectorRow("technique.id", "ID", () => idChanged = EditorImGui.InputText("##id", ref id, 128));
                if (idChanged)
                {
                    technique.id = new()
                    {
                        value = id
                    };
                    changed = true;
                }

                Gesture();
                bool contractChanged = false;
                InspectorRow(
                    "technique.contract",
                    "Contract",
                    () => contractChanged = EditorImGui.InputText("##contract", ref contract, 256)
                );
                if (contractChanged)
                {
                    technique.contract = new()
                    {
                        value = contract
                    };
                    changed = true;
                }

                Gesture();
                GraphicsCapability features = technique.requiredFeatures;
                if (FeatureControls(ref features))
                {
                    technique.requiredFeatures = features;
                    changed = true;
                }

                for (int j = 0; j < technique.passes.Length; j++)
                {
                    EditorImGui.PushID(j);
                    try
                    {
                        ShaderTechniquePass mapping = technique.passes[j];
                        string role = mapping.role.value ?? string.Empty;
                        bool roleChanged = false;
                        InspectorRow("technique.role", "Role", () => roleChanged = EditorImGui.InputText("##role", ref role, 128));
                        if (roleChanged)
                        {
                            mapping.role = new()
                            {
                                value = role
                            };
                            changed = true;
                        }

                        Gesture();
                        bool passChanged = false;
                        InspectorRow("technique.pass", "Pass", () =>
                        {
                            if (!EditorUI.current.widgets.BeginBoundedCombo("##pass", mapping.passName))
                            {
                                return;
                            }
                            try
                            {
                                foreach (ShaderPassDefinition pass in definition.passes)
                                {
                                    if (EditorImGui.Selectable(pass.name, pass.name == mapping.passName))
                                    {
                                        mapping.passName = pass.name;
                                        passChanged = true;
                                    }
                                }
                            }
                            finally
                            {
                                EditorUI.current.widgets.EndBoundedCombo();
                            }
                        });
                        changed |= passChanged;
                        technique.passes[j] = mapping;
                        if (EditorImGui.SmallButton("Remove Role"))
                        {
                            technique.passes = technique.passes.Where((
                                _,
                                index
                            ) => index != j).ToArray();
                            changed = true;
                            break;
                        }
                    }
                    finally
                    {
                        EditorImGui.PopID();
                    }
                }

                if (CenteredAddButton("Add Role"))
                {
                    technique.passes =
                    [
                        ..technique.passes,
                        new ShaderTechniquePass()
                        {
                            role = new("role-" + technique.passes.Length),
                            passName = definition.passes.FirstOrDefault().name ?? string.Empty
                        }
                    ];
                    changed = true;
                }

                definition.techniques[i] = technique;
                if (EditorImGui.SmallButton("Remove Technique"))
                {
                    definition.techniques = definition.techniques.Where((
                        _,
                        index
                    ) => index != i).ToArray();
                    changed = true;
                    break;
                }

                EditorImGui.Separator();
            }
            finally
            {
                EditorImGui.PopID();
            }
        }

        if (CenteredAddButton("Add Technique"))
        {
            definition.techniques =
            [
                ..definition.techniques,
                new ShaderTechniqueDefinition()
                {
                    id = new("technique-" + Guid.NewGuid().ToString("N")),
                    contract = default,
                    passes = []
                }
            ];
            changed = true;
        }

        return changed;
    }

    private static bool FeatureControls(ref GraphicsCapability features)
    {
        if (!EditorImGui.TreeNode("Required Capabilities", 0))
        {
            return false;
        }
        try
        {
            bool changed = false;
            GraphicsCapability updated = features;
            foreach (GraphicsCapability feature in Enum.GetValues<GraphicsCapability>())
            {
                ulong bits = Convert.ToUInt64(feature);
                if (bits == 0 || (bits & (bits - 1)) != 0)
                {
                    continue;
                }
                bool enabled = (updated & feature) != 0;
                bool featureChanged = false;
                InspectorRow(
                    "feature." + feature,
                    feature.ToString(),
                    () => featureChanged = EditorImGui.Checkbox("##enabled", ref enabled)
                );
                if (featureChanged)
                {
                    updated = enabled ? updated | feature : updated & ~feature;
                    changed = true;
                }
            }

            features = updated;
            return changed;
        }
        finally
        {
            EditorImGui.TreePop();
        }
    }
}
